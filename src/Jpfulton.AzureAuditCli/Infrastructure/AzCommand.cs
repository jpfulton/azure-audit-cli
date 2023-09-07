using Jpfulton.AzureAuditCli.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Linq;

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

    public static async Task<Subscription> GetAzureSubscriptionAsync(Guid id)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = $"account subscription show --id {id}",
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
                throw new Exception($"Error executing 'az account subscription show': {error}");
            }

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;

                var subscription = new Subscription
                {
                    Id = root.GetStringPropertyValue("id"),
                    SubscriptionId = root.GetStringPropertyValue("subscriptionId"),
                    DisplayName = root.GetStringPropertyValue("displayName"),
                    State = root.GetStringPropertyValue("state")
                };

                return subscription;
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

                subscriptions.AddRange(
                    arrayEnumerator.Select(element =>
                    {
                        return new Subscription
                        {
                            Id = element.GetStringPropertyValue("id"),
                            SubscriptionId = element.GetStringPropertyValue("subscriptionId"),
                            DisplayName = element.GetStringPropertyValue("displayName"),
                            State = element.GetStringPropertyValue("state")
                        };
                    })
                );

                return subscriptions.OrderBy(s => s.DisplayName).ToArray();
            }
        }
    }

    public static async Task<ResourceGroup[]> GetAzureResourceGroupsAsync(Guid subscriptionId)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = $"group list --subscription {subscriptionId}",
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

            var resourceGroups = new List<ResourceGroup>();

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;
                var arrayEnumerator = root.EnumerateArray();

                resourceGroups.AddRange(
                    arrayEnumerator.Select(element =>
                    {
                        return new ResourceGroup
                        {
                            Id = element.GetStringPropertyValue("id"),
                            Location = element.GetStringPropertyValue("location"),
                            Name = element.GetStringPropertyValue("name")
                        };
                    })
                );

                return resourceGroups.OrderBy(g => g.Name).ToArray();
            }
        }
    }

    public static async Task<Resource[]> GetAzureResourcesAsync(Guid subscriptionId, string resourceGroup)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "az",
            Arguments = $"resource list --subscription {subscriptionId} --resource-group {resourceGroup}",
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

            var resources = new List<Resource>();

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;
                var arrayEnumerator = root.EnumerateArray();

                foreach (var element in arrayEnumerator)
                {
                    var resourceId = element.GetStringPropertyValue("id");
                    var resource = new Resource
                    {
                        Id = resourceId,
                        ResourceType = element.GetStringPropertyValue("type"),
                        PrimaryArmLocation = element.GetStringPropertyValue("location"),
                        Name = element.GetStringPropertyValue("name"),

                        // az list does not return a complete set of properties
                        CompleteJsonBody = await GetAzureResourceJsonByIdAsync(resourceId)
                    };

                    if (element.TryGetProperty("sku", out JsonElement skuElement))
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

                    resources.Add(resource);
                }
            }

            return resources.OrderBy(r => r.ResourceType).ThenBy(r => r.Name).ToArray();
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
                    Name = root.GetStringPropertyValue("name"),
                    CompleteJsonBody = root.ToString()
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

    public static async Task<string> GetAzureResourceJsonByIdAsync(string resourceId)
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

            return output;
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