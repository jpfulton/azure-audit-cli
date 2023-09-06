namespace Jpfulton.AzureAuditCli.Models;

public class Resource
{
  public string? Id { get; set; }
  public string? ResourceType { get; set; }
  public string? Name { get; set; }
  public string? PrimaryArmLocation { get; set; }
  public string? ArmSkuName { get; set; }
}