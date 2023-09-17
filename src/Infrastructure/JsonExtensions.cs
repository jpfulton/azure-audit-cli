using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace AzureAuditCli.Infrastructure;

public static class JsonExtensions
{
    public static string GetStringPropertyValue(
        this JsonElement element,
        string propertyName,
        bool required = true
        )
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
                if (required)
                    throw new Exception($"Value of '{propertyName}' property is null.");
                else
                    return string.Empty;
            }
        }
        else
        {
            if (required)
                throw new Exception($"Unable to find the '{propertyName}' property in the JSON output.");
            else
                return string.Empty;
        }
    }

    public static int? GetIntegerPropertyValue(
        this JsonElement element,
        string propertyName,
        bool required = true
        )
    {
        if (element.TryGetProperty(propertyName, out JsonElement childElement))
        {
            int value = childElement.GetInt32();
            return value;
        }
        else
        {
            if (required)
                throw new Exception($"Unable to find the '{propertyName}' property in the JSON output.");
            else
                return null;
        }
    }

    public static bool? GetBooleanPropertyValue(
        this JsonElement element,
        string propertyName,
        bool required = true
        )
    {
        if (element.TryGetProperty(propertyName, out JsonElement childElement))
        {
            bool value = childElement.GetBoolean();
            return value;
        }
        else
        {
            if (required)
                throw new Exception($"Unable to find the '{propertyName}' property in the JSON output.");
            else
                return null;
        }
    }

    public static JsonElement? GetChildElement(
        this JsonElement element,
        string propertyName,
        bool required = false
    )
    {
        if (element.TryGetProperty(propertyName, out JsonElement childElement))
        {
            return childElement;
        }
        else
        {
            if (required)
                throw new Exception($"Unable to find the '{propertyName}' property in the JSON output.");
            else
                return null;
        }
    }
}