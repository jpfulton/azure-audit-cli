using System.Text.Json;

namespace Jpfulton.AzureAuditCli.Infrastructure;

public static class JsonExtensions
{
    public static string GetStringPropertyValue(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out JsonElement childElement))
        {
            string? value = childElement.GetString();
            if (value != null)
            {
                return value!;
            }
            else
            {
                throw new Exception($"Value of '${propertyName}' property is null.");
            }
        }
        else
        {
            throw new Exception($"Unable to find the '${propertyName}' property in the JSON output.");
        }
    }
}