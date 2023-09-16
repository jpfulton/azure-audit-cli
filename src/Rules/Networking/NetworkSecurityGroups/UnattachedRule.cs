using AzureAuditCli.Models.Networking;

namespace AzureAuditCli.Rules.Networking.NetworkSecurityGroups;

public class UnattachedRule : IRule<NetworkSecurityGroup>
{
    public IEnumerable<IRuleOutput> Evaluate(NetworkSecurityGroup resource)
    {
        var outputs = new List<IRuleOutput>();

        outputs.AddRange(EvaluateUnattachedToNic(resource));

        return outputs;
    }

    private static IEnumerable<IRuleOutput> EvaluateUnattachedToNic(
        NetworkSecurityGroup resource
        )
    {
        var outputs = new List<IRuleOutput>();

        if (resource.NetworkInterfaces.Count == 0)
        {
            var level = Level.Info;
            var message = $"No network interfaces are attached.";

            outputs.Add(new DefaultRuleOutput(
                level,
                message,
                resource
            ));
        }

        return outputs;
    }
}