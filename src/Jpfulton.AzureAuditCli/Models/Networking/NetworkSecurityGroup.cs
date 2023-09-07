using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Models.Networking;

public class NetworkSecurityGroup : Resource
{
    public List<SecurityRule> SecurityRules { get; set; } = new();
}