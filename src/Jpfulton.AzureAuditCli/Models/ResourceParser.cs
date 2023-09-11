using System.Text.Json;
using Jpfulton.AzureAuditCli.Infrastructure;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.Models.Storage;
using Microsoft.VisualBasic;

namespace Jpfulton.AzureAuditCli.Models;

public static class ResourceParser
{
    private static readonly Dictionary<string, Type> typeMap = new()
    {
        {AzureResourceType.IpConfiguration, typeof(IpConfiguration)},
        {AzureResourceType.ManagedDisk, typeof(ManagedDisk)},
        {AzureResourceType.NetworkInterfaceCard, typeof(NetworkInterfaceCard)},
        {AzureResourceType.NetworkSecurityGroup, typeof(NetworkSecurityGroup)},
        {AzureResourceType.SecurityRule, typeof(SecurityRule)},
        {AzureResourceType.StorageAccount, typeof(StorageAccount)}
    };

    public static ResourceRef? ParseRef(JsonElement? element)
    {
        return !element.HasValue ? null : new ResourceRef
        {
            Id = element.Value.GetStringPropertyValue("id"),
            ResourceGroup = element.Value.GetStringPropertyValue("resourceGroup")
        };
    }

    public static async Task<Resource> ParseAsync(JsonElement element, bool queryForCompleteJson = false)
    {
        var resourceId = element.GetStringPropertyValue("id");
        var resourceType = element.GetStringPropertyValue("type");

        var resource = CreateResource(resourceType);

        resource.Id = resourceId;
        resource.ResourceGroup = element.GetStringPropertyValue("resourceGroup");
        resource.ResourceType = resourceType;
        resource.Location = element.GetStringPropertyValue("location", required: false);
        resource.Name = element.GetStringPropertyValue("name");
        resource.CompleteJsonBody = queryForCompleteJson ? await AzCommand.GetAzureResourceJsonByIdAsync(resourceId) : element.ToString();

        if (
            element.TryGetProperty("sku", out JsonElement skuElement) &&
            skuElement.ValueKind != JsonValueKind.Null
        )
        {
            resource.SkuName = skuElement.GetStringPropertyValue("name");
        }

        if (
            element.TryGetProperty("tags", out JsonElement tagsElement) &&
            tagsElement.ValueKind != JsonValueKind.Null
            )
        {
            foreach (var property in tagsElement.EnumerateObject())
            {
                resource.Tags.Add(property.Name, property.Value.GetString()!);
            }
        }

        return await resource.ParseRemainingProperties(element);
    }

    private static Resource CreateResource(string resourceType)
    {
        return typeMap.TryGetValue(resourceType, out Type? type) ?
            (Resource)Activator.CreateInstance(type)! :
            new Resource();
    }

    private static async Task<Resource> ParseRemainingProperties(
        this Resource resource,
        JsonElement element
        )
    {
        var resourceType = resource.ResourceType;

        switch (resourceType)
        {
            case AzureResourceType.IpConfiguration:
                return ParseIpConfiguration(resource, element);

            case AzureResourceType.ManagedDisk:
                return ParseManagedDisk(resource, element);

            case AzureResourceType.NetworkInterfaceCard:
                return await ParseNetworkInterfaceCardAsync(resource, element);

            case AzureResourceType.NetworkSecurityGroup:
                return await ParseNetworkSecurityGroupAsync(resource, element);

            case AzureResourceType.SecurityRule:
                return ParseSecurityRule(resource, element);

            case AzureResourceType.StorageAccount:
                return ParseStorageAccount(resource, element);

            default:
                return resource;
        }
    }

    private static async Task<NetworkSecurityGroup> ParseNetworkSecurityGroupAsync(
        Resource resource,
        JsonElement element
        )
    {
        var nsg = resource as NetworkSecurityGroup ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            if (propsElement.TryGetProperty("networkInterfaces", out JsonElement nicsElement))
            {
                foreach (var nicElement in nicsElement.EnumerateArray())
                {
                    nsg.NetworkInterfaces.Add(ParseRef(nicElement)!);
                }
            }

            if (propsElement.TryGetProperty("securityRules", out JsonElement securityRulesElement))
            {
                foreach (var ruleElement in securityRulesElement.EnumerateArray())
                {
                    nsg.SecurityRules.Add((SecurityRule)await ParseAsync(ruleElement));
                }
            }
        }

        return nsg;
    }

    private static SecurityRule ParseSecurityRule(Resource resource, JsonElement element)
    {
        var rule = resource as SecurityRule ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            rule.Access = Enum.Parse<Access>(propsElement.GetStringPropertyValue("access"));
            rule.DestinationAddressPrefix = propsElement.GetStringPropertyValue("destinationAddressPrefix");
            rule.DestinationPortRange = propsElement.GetStringPropertyValue("destinationPortRange");
            rule.Direction = Enum.Parse<Direction>(propsElement.GetStringPropertyValue("direction"));
            rule.Priority = propsElement.GetIntegerPropertyValue("priority") ?? -1;
            rule.Protocol = Enum.Parse<Protocol>(propsElement.GetStringPropertyValue("protocol"));
            rule.SourceAddressPrefix = propsElement.GetStringPropertyValue("sourceAddressPrefix");
            rule.SourcePortRange = propsElement.GetStringPropertyValue("sourcePortRange");
        }

        return rule;
    }

    private static async Task<NetworkInterfaceCard> ParseNetworkInterfaceCardAsync(
        Resource resource,
        JsonElement element
        )
    {
        var card = resource as NetworkInterfaceCard ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            card.EnableAcceleratedNetworking = propsElement.GetBooleanPropertyValue("enableAcceleratedNetworking", false) ?? false;
            card.Primary = propsElement.GetBooleanPropertyValue("primary") ?? false;
            card.NetworkSecurityGroup = ParseRef(propsElement.GetChildElement("networkSecurityGroup"));
            card.VirtualMachine = ParseRef(propsElement.GetChildElement("virtualMachine"));
            card.VnetEncryptionSupported = propsElement.GetBooleanPropertyValue("vnetEncryptionSupported") ?? false;

            if (propsElement.TryGetProperty("ipConfigurations", out JsonElement configsElement))
            {
                foreach (var configElement in configsElement.EnumerateArray())
                {
                    card.IpConfigurations.Add((IpConfiguration)await ParseAsync(configElement));
                }
            }
        }

        return card;
    }

    private static IpConfiguration ParseIpConfiguration(Resource resource, JsonElement element)
    {
        var config = resource as IpConfiguration ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            config.Primary = propsElement.GetBooleanPropertyValue("primary") ?? false;
            config.PrivateIpAddress = propsElement.GetStringPropertyValue("privateIPAddress");
            config.PublicIpAddress = ParseRef(propsElement.GetChildElement("publicIPAddress"));
            config.Subnet = ParseRef(propsElement.GetChildElement("subnet"));
        }

        return config;
    }

    private static ManagedDisk ParseManagedDisk(Resource resource, JsonElement element)
    {
        var disk = resource as ManagedDisk ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        disk.ManagedBy = element.GetStringPropertyValue("managedBy");

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            disk.DataAccessAuthMode = Enum.TryParse<DataAccessAuthMode>(propsElement.GetStringPropertyValue("dataAccessAuthMode", false), out var authType) ? authType : null;
            disk.DiskSizeGB = propsElement.GetIntegerPropertyValue("diskSizeGB") ?? 0;
            disk.DiskState = Enum.Parse<DiskState>(propsElement.GetStringPropertyValue("diskState"));
            disk.NetworkAccessPolicy = Enum.Parse<NetworkAccessPolicy>(propsElement.GetStringPropertyValue("networkAccessPolicy"));
            disk.OsType = Enum.TryParse<OsType>(propsElement.GetStringPropertyValue("osType", false), out var osType) ? osType : null;
            disk.PublicNetworkAccess = Enum.Parse<PublicNetworkAccess>(propsElement.GetStringPropertyValue("publicNetworkAccess"));

            if (propsElement.TryGetProperty("encryption", out var encryptionElement))
            {
                disk.EncryptionType = Enum.Parse<EncryptionType>(encryptionElement.GetStringPropertyValue("type"));
            }
        }

        return disk;
    }

    private static StorageAccount ParseStorageAccount(Resource resource, JsonElement element)
    {
        var account = resource as StorageAccount ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        account.Kind = Enum.Parse<Kind>(element.GetStringPropertyValue("kind"));

        if (
            element.TryGetProperty("properties", out JsonElement propsElement) &&
            propsElement.ValueKind != JsonValueKind.Null
            )
        {
            account.AllowBlobPublicAccess = propsElement.GetBooleanPropertyValue("allowBlobPublicAccess") ?? false;
            account.DefaultToOAuthAuthentication = propsElement.GetBooleanPropertyValue("defaultToOAuthAuthentication", false) ?? false;

            if (propsElement.TryGetProperty("encryption", out var encryptionElement))
            {
                account.EncryptionKeySource = Enum.Parse<KeySource>(encryptionElement.GetStringPropertyValue("keySource").Replace(".", ""));
                account.RequireInfrastructureEncryption = encryptionElement.GetBooleanPropertyValue("requireInfrastructureEncryption", false) ?? false;

                if (encryptionElement.TryGetProperty("services", out var servicesElement))
                {
                    if (servicesElement.TryGetProperty("blob", out var blobElement))
                    {
                        account.EncryptionServicesBlobEnabled = blobElement.GetBooleanPropertyValue("enabled") ?? false;
                        account.EncryptionServicesBlobKeyType = Enum.Parse<KeyType>(blobElement.GetStringPropertyValue("keyType"));
                    }

                    if (servicesElement.TryGetProperty("file", out var fileElement))
                    {
                        account.EncryptionServicesFileEnabled = fileElement.GetBooleanPropertyValue("enabled") ?? false;
                        account.EncryptionServicesFileKeyType = Enum.Parse<KeyType>(fileElement.GetStringPropertyValue("keyType"));
                    }

                    if (servicesElement.TryGetProperty("queue", out var queueElement))
                    {
                        account.EncryptionServicesQueueEnabled = queueElement.GetBooleanPropertyValue("enabled") ?? false;
                        account.EncryptionServicesQueueKeyType = Enum.Parse<KeyType>(queueElement.GetStringPropertyValue("keyType"));
                    }

                    if (servicesElement.TryGetProperty("table", out var tableElement))
                    {
                        account.EncryptionServicesTableEnabled = tableElement.GetBooleanPropertyValue("enabled") ?? false;
                        account.EncryptionServicesTableKeyType = Enum.Parse<KeyType>(tableElement.GetStringPropertyValue("keyType"));
                    }
                }

                if (propsElement.TryGetProperty("networkAcls", out var networkAclsElement))
                {
                    var acls = new NetworkAcls
                    {
                        Bypass = networkAclsElement.GetStringPropertyValue("bypass"),
                        DefaultAction = Enum.Parse<NetworkAclAction>(networkAclsElement.GetStringPropertyValue("defaultAction"))
                    };

                    if (networkAclsElement.TryGetProperty("ipRules", out var ipRulesElement))
                    {
                        acls.IpRulesCount = ipRulesElement.GetArrayLength();
                    }

                    if (networkAclsElement.TryGetProperty("ipv6Rules", out var ipv6RulesElement))
                    {
                        acls.IpV6RulesCount = ipv6RulesElement.GetArrayLength();
                    }

                    if (networkAclsElement.TryGetProperty("virtualNetworkRules", out var vnetRulesElement))
                    {
                        acls.VirtualNetworkRulesCount = vnetRulesElement.GetArrayLength();
                    }

                    account.NetworkAcls = acls;
                }

                account.MinimumTlsVersion = Enum.Parse<TlsVersion>(propsElement.GetStringPropertyValue("minimumTlsVersion"));

                if (propsElement.TryGetProperty("primaryEndpoints", out var primaryEndpointsElement))
                {
                    if (primaryEndpointsElement.TryGetProperty("blob", out _))
                    {
                        account.ServicesBlobEnabled = true;
                    }

                    if (primaryEndpointsElement.TryGetProperty("dfs", out _))
                    {
                        account.ServicesDfsEnabled = true;
                    }

                    if (primaryEndpointsElement.TryGetProperty("file", out _))
                    {
                        account.ServicesFileEnabled = true;
                    }

                    if (primaryEndpointsElement.TryGetProperty("queue", out _))
                    {
                        account.ServicesQueueEnabled = true;
                    }

                    if (primaryEndpointsElement.TryGetProperty("table", out _))
                    {
                        account.ServicesTableEnabled = true;
                    }

                    if (primaryEndpointsElement.TryGetProperty("web", out _))
                    {
                        account.ServicesWebEnabled = true;
                    }
                }

                if (propsElement.TryGetProperty("privateEndpointConnections", out var pecsElement))
                {
                    account.PrivateEndpointConnectionsCount = pecsElement.GetArrayLength();
                }

                account.SupportsHttpsTrafficOnly = propsElement.GetBooleanPropertyValue("supportsHttpsTrafficOnly") ?? false;
            }
        }

        return account;
    }
}