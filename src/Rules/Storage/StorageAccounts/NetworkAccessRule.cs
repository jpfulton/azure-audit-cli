using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.StorageAccounts;

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
        else if (
            !resource.AllowBlobPublicAccess &&
            resource.NetworkAcls != null &&
            !resource.NetworkAcls.Bypass.Equals("None") &&
            resource.NetworkAcls.DefaultAction == NetworkAclAction.Deny &&
            resource.NetworkAcls.IpRulesCount == 0 &&
            resource.NetworkAcls.IpV6RulesCount == 0 &&
            resource.NetworkAcls.VirtualNetworkRulesCount == 0 &&
            resource.PrivateEndpointConnectionsCount == 0
            )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Storage account is completely inaccessible to all networks. Only Azure services are provided access.",
                resource
            ));
        }
        else if (
            !resource.AllowBlobPublicAccess &&
            resource.NetworkAcls != null &&
            resource.NetworkAcls.DefaultAction == NetworkAclAction.Deny
            )
        {
            if (resource.NetworkAcls.IpRulesCount > 0)
            {
                outputs.Add(new DefaultRuleOutput(
                    Level.Info,
                    $"Storage account is accessible via {resource.NetworkAcls.IpRulesCount} IP rule(s).",
                    resource
                ));
            }

            if (resource.NetworkAcls.IpV6RulesCount > 0)
            {
                outputs.Add(new DefaultRuleOutput(
                    Level.Info,
                    $"Storage account is accessible via {resource.NetworkAcls.IpV6RulesCount} IPv6 rule(s).",
                    resource
                ));
            }

            if (resource.NetworkAcls.VirtualNetworkRulesCount > 0)
            {
                outputs.Add(new DefaultRuleOutput(
                    Level.Info,
                    $"Storage account is accessible via {resource.NetworkAcls.VirtualNetworkRulesCount} virtual network rule(s).",
                    resource
                ));
            }
        }

        return outputs;
    }
}