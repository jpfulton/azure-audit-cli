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

    public static async Task<Resource[]> GetAzureResourcesAsync(Guid subscriptionId, string resourceGroup, bool includeJsonBody = false)
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
                    resources.Add(await ResourceParser.ParseAsync(element, includeJsonBody));
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
                return await ResourceParser.ParseAsync(root);
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
}