namespace Jpfulton.AzureAuditCli.Models.Networking;

public class NetworkSecurityGroup : Resource
{
    public List<ResourceRef> NetworkInterfaces { get; set; } = new();
    public List<SecurityRule> SecurityRules { get; set; } = new();
}