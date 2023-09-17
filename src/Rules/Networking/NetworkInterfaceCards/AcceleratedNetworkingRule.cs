using AzureAuditCli.Models.Networking;

namespace AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class AcceleratedNetworkingRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput>();

        if (!resource.EnableAcceleratedNetworking)
        {
            var level = Level.Note;
            var message = "Accelerated networking is not available or not enabled.";

            outputs.Add(new DefaultRuleOutput(level, message, resource));
        }

        return outputs;
    }
}