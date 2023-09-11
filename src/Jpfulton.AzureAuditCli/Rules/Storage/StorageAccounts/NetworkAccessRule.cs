using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.StorageAccounts;

public class NetworkAccessRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (
            !resource.AllowBlobPublicAccess &&
            resource.NetworkAcls != null &&
            resource.NetworkAcls.Bypass.Equals("None") &&
            resource.NetworkAcls.DefaultAction == NetworkAclAction.Deny &&
            resource.NetworkAcls.IpRulesCount == 0 &&
            resource.NetworkAcls.IpV6RulesCount == 0 &&
            resource.NetworkAcls.VirtualNetworkRulesCount == 0 &&
            resource.PrivateEndpointConnectionsCount == 0
            )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Critical,
                "Storage account is completely inaccessible from all networks and Azure services.",
                resource
            ));
        }

        return outputs;
    }
}