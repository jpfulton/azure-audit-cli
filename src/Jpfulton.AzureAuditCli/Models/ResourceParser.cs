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

    public static async Task<Resource> ParseAsync(JsonElement element, bool queryForCompleteJson = false)
    {
        var resourceId = element.GetStringPropertyValue("id");
        var resourceType = element.GetStringPropertyValue("type");

        var resource = CreateResource(resourceType);

        resource.Id = resourceId;
        resource.ResourceGroup = element.GetStringPropertyValue("resourceGroup");
        resource.ResourceType = resourceType;
        resource.Location = element.GetStringPropertyValue("location");
        resource.Name = element.GetStringPropertyValue("name");
        resource.CompleteJsonBody = queryForCompleteJson ? await AzCommand.GetAzureResourceJsonByIdAsync(resourceId) : element.ToString();

        if (element.TryGetProperty("sku", out JsonElement skuElement))
        {
            if (skuElement.ValueKind != JsonValueKind.Null)
            {
                resource.SkuName = skuElement.GetStringPropertyValue("name");
            }
        }
        else
        {
            throw new Exception("Unable to find the 'sku' element in the JSON output.");
        }

        if (element.TryGetProperty("tags", out JsonElement tagsElement))
        {
            if (tagsElement.ValueKind != JsonValueKind.Null)
            {
                foreach (var property in tagsElement.EnumerateObject())
                {
                    resource.Tags.Add(property.Name, property.Value.GetString()!);
                }
            }
        }

        return await resource.ParseRemainingProperties(element);
    }

    private static Resource CreateResource(string resourceType)
    {
        if (typeMap.ContainsKey(resourceType))
        {
            var type = typeMap[resourceType];
            return (Resource)Activator.CreateInstance(type)!;
        }
        else
        {
            return new Resource();
        }
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

        if (element.TryGetProperty("properties", out JsonElement propsElement))
        {
            if (propsElement.ValueKind != JsonValueKind.Null)
            {
                if (propsElement.TryGetProperty("securityRules", out JsonElement securityRulesElement))
                {
                    var rulesArray = securityRulesElement.EnumerateArray();
                    foreach (var ruleElement in rulesArray)
                    {
                        nsg.SecurityRules.Add((SecurityRule)await ParseAsync(ruleElement));
                    }
                }
            }
        }

        return nsg;
    }

    private static SecurityRule ParseSecurityRule(Resource resource, JsonElement element)
    {
        var rule = resource as SecurityRule ?? throw new ArgumentException("Resource is not of correct Type.", "resource");

        if (element.TryGetProperty("properties", out JsonElement propsElement))
        {
            if (propsElement.ValueKind != JsonValueKind.Null)
            {
                rule.Access = Enum.Parse<Access>(propsElement.GetStringPropertyValue("access"));
                rule.DestinationAddressPrefix = propsElement.GetStringPropertyValue("destinationAddressPrefix");
                rule.DestinationPortRange = propsElement.GetStringPropertyValue("destinationPortRange");
                rule.Direction = Enum.Parse<Direction>(propsElement.GetStringPropertyValue("direction"));
                rule.Priority = int.Parse(propsElement.GetStringPropertyValue("priority"));
                rule.Protocol = Enum.Parse<Protocol>(propsElement.GetStringPropertyValue("protocol"));
                rule.SourceAddressPrefix = propsElement.GetStringPropertyValue("sourceAddressPrefix");
                rule.SourcePortRange = propsElement.GetStringPropertyValue("sourcePortRange");
            }
        }

        return rule;
    }
}