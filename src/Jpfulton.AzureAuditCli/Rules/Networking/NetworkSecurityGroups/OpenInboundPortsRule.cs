using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkSecurityGroups;

public class OpenInboundPortsRule : IRule<NetworkSecurityGroup>
{
    public IEnumerable<IRuleOutput<NetworkSecurityGroup>> Evaluate(NetworkSecurityGroup resource)
    {
        var outputs = new List<IRuleOutput<NetworkSecurityGroup>>();

        outputs.Add(new DefaultRuleOutput<NetworkSecurityGroup>(
            Level.Warn,
            "This is a test message.",
            resource
        ));

        return outputs.OrderByDescending(o => o.Level).ThenBy(o => o.Message);
    }
}