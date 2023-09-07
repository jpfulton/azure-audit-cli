using System.Text.Json;
using Jpfulton.AzureAuditCli.Infrastructure;

namespace Jpfulton.AzureAuditCli.Models;

public static class ResourceParser
{
    public static async Task<Resource> ParseAsync(JsonElement element, bool queryForCompleteJson = false)
    {
        var resourceId = element.GetStringPropertyValue("id");

        var resource = new Resource
        {
            Id = resourceId,
            ResourceGroup = element.GetStringPropertyValue("resourceGroup"),
            ResourceType = element.GetStringPropertyValue("type"),
            Location = element.GetStringPropertyValue("location"),
            Name = element.GetStringPropertyValue("name"),
            CompleteJsonBody = queryForCompleteJson ? await AzCommand.GetAzureResourceJsonByIdAsync(resourceId) : element.ToString()
        };

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

        return resource;
    }
}