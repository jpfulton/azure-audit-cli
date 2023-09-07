namespace Jpfulton.AzureAuditCli.Models;

public class Resource
{
  public string Id { get; set; } = string.Empty;
  public string ResourceType { get; set; } = string.Empty;
  public string ResourceGroup { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Location { get; set; } = string.Empty;
  public string SkuName { get; set; } = string.Empty;
  public string CompleteJsonBody { get; set; } = string.Empty;
}