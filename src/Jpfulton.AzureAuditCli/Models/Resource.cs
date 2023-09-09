using System.Text.Json.Serialization;

namespace Jpfulton.AzureAuditCli.Models;

public class Resource : ResourceRef
{
  public string ResourceType { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Location { get; set; } = string.Empty;
  public Dictionary<string, string> Tags { get; set; } = new();
  public string SkuName { get; set; } = string.Empty;

  [JsonIgnore]
  public string CompleteJsonBody { get; set; } = string.Empty;
}