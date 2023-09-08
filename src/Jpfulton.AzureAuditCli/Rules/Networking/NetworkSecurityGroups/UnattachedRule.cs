using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkSecurityGroups;

public class UnattachedRule : IRule<NetworkSecurityGroup>
{
    public IEnumerable<IRuleOutput<NetworkSecurityGroup>> Evaluate(NetworkSecurityGroup resource)
    {
        var outputs = new List<IRuleOutput<NetworkSecurityGroup>>();

        outputs.AddRange(EvaluateUnattachedToNic(resource));

        return outputs;
    }

    private static IEnumerable<IRuleOutput<NetworkSecurityGroup>> EvaluateUnattachedToNic(
        NetworkSecurityGroup resource
        )
    {
        var outputs = new List<IRuleOutput<NetworkSecurityGroup>>();

        if (resource.NetworkInterfaces.Count == 0)
        {
            var level = Level.Info;
            var message = $"No network interfaces are attached.";

            outputs.Add(new DefaultRuleOutput<NetworkSecurityGroup>(
                level,
                message,
                resource
            ));
        }

        return outputs;
    }
}