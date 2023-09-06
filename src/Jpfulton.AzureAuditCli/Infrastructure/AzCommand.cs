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
                if (root.TryGetProperty("id", out JsonElement idElement))
                {
                    string? subscriptionId = idElement.GetString();
                    return subscriptionId;
                }
                else
                {
                    throw new Exception("Unable to find the 'id' property in the JSON output.");
                }
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
                    if (element.TryGetProperty("id", out JsonElement idElement))
                    {
                        string? resourceId = idElement.GetString();
                        if (resourceId != null) idList.Add(resourceId);
                    }
                    else
                    {
                        throw new Exception("Unable to find the 'id' property in the JSON output.");
                    }
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

            var resource = new Resource();

            using (var jsonDocument = JsonDocument.Parse(output))
            {
                JsonElement root = jsonDocument.RootElement;

                if (root.TryGetProperty("id", out JsonElement idElement))
                {
                    string? id = idElement.GetString();
                    resource.Id = id;
                }
                else
                {
                    throw new Exception("Unable to find the 'id' property in the JSON output.");
                }

                if (root.TryGetProperty("type", out JsonElement typeElement))
                {
                    string? value = typeElement.GetString();
                    resource.ResourceType = value;
                }
                else
                {
                    throw new Exception("Unable to find the 'type' property in the JSON output.");
                }

                if (root.TryGetProperty("location", out JsonElement locationElement))
                {
                    string? value = locationElement.GetString();
                    resource.PrimaryArmLocation = value;
                }
                else
                {
                    throw new Exception("Unable to find the 'location' property in the JSON output.");
                }

                if (root.TryGetProperty("name", out JsonElement nameElement))
                {
                    string? value = nameElement.GetString();
                    resource.Name = value;
                }
                else
                {
                    throw new Exception("Unable to find the 'name' property in the JSON output.");
                }

                if (root.TryGetProperty("sku", out JsonElement skuElement))
                {
                    if (skuElement.ValueKind != JsonValueKind.Null)
                    {
                        if (skuElement.TryGetProperty("name", out JsonElement skuNameElement))
                        {
                            string? value = skuNameElement.GetString();
                            resource.ArmSkuName = value;
                        }
                        else
                        {
                            throw new Exception("Unable to find the 'sku.name' element in the JSON output.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Unable to find the 'sku' element in the JSON output.");
                }
            }

            return resource;
        }
    }
}