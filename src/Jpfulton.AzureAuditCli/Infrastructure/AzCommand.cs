using Jpfulton.AzureAuditCli.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Jpfulton.AzureAuditCli.Infrastructure;

public static class AzCommand
{
    public static async Task<string?> GetDefaultAzureSubscriptionIdAsync()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = "account show",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Error executing 'az account show': {error}");
            }

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;
                return root.GetStringPropertyValue("id");
            }
        }
    }

    public static async Task<Subscription[]> GetAzureSubscriptionsAsync()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = "account subscription list",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Error executing 'az account subscription list': {error}");
            }

            var subscriptions = new List<Subscription>();

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;
                var arrayEnumerator = root.EnumerateArray();

                foreach (var element in arrayEnumerator)
                {
                    var subscription = new Subscription
                    {
                        Id = element.GetStringPropertyValue("subscriptionId"),
                        DisplayName = element.GetStringPropertyValue("displayName"),
                        State = element.GetStringPropertyValue("state")
                    };

                    subscriptions.Add(subscription);
                }

                return subscriptions.ToArray();
            }
        }
    }

    public static async Task<string[]> GetAzureResourceIdsAsync(string resourceGroup)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = $"resource list --resource-group {resourceGroup}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Error executing 'az resource list': {error}");
            }

            var idList = new List<string>();

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;
                var arrayEnumerator = root.EnumerateArray();

                foreach (var element in arrayEnumerator)
                {
                    idList.Add(element.GetStringPropertyValue("id"));
                }
            }

            return idList.ToArray();
        }
    }

    public static async Task<Resource> GetAzureResourceByIdAsync(string resourceId)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = $"resource show --ids \"{resourceId}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Error executing 'az resource show': {error}");
            }

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;

                var resource = new Resource
                {
                    Id = root.GetStringPropertyValue("id"),
                    ResourceType = root.GetStringPropertyValue("type"),
                    PrimaryArmLocation = root.GetStringPropertyValue("location"),
                    Name = root.GetStringPropertyValue("name")
                };

                if (root.TryGetProperty("sku", out JsonElement skuElement))
                {
                    if (skuElement.ValueKind != JsonValueKind.Null)
                    {
                        resource.ArmSkuName = skuElement.GetStringPropertyValue("name");
                    }
                }
                else
                {
                    throw new Exception("Unable to find the 'sku' element in the JSON output.");
                }

                return resource;
            }
        }
    }

    private static string GetStringPropertyValue(this JsonElement element, string propertyName)
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