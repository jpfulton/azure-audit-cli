using System.Text.Json;
using Jpfulton.AzureAuditCli.Infrastructure;
using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Models;

public static class ResourceParser
{
    private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>
    {
        {"Microsoft.Network/networkSecurityGroups", typeof(NetworkSecurityGroup) },
        {"Microsoft.Network/networkSecurityGroups/securityRules", typeof(SecurityRule)}
    };

    public static ResourceRef ParseRef(JsonElement element)
    {
        return new ResourceRef
        {
            Id = element.GetStringPropertyValue("id"),
            ResourceGroup = element.GetStringPropertyValue("resourceGroup")
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

    private static async Task<Resource> ParseRemainingProperties(this Resource resource, JsonElement element)
    {
        var resourceType = resource.ResourceType;

        switch (resourceType)
        {
            case "Microsoft.Network/networkSecurityGroups":
                return await ParseNetworkSecurityGroup(resource, element);

            case "Microsoft.Network/networkSecurityGroups/securityRules":
                return ParseSecurityRule(resource, element);

            default:
                return resource;
        }
    }

    private static async Task<NetworkSecurityGroup> ParseNetworkSecurityGroup(Resource resource, JsonElement element)
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
                    nsg.NetworkInterfaces.Add(ParseRef(nicElement));
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
}