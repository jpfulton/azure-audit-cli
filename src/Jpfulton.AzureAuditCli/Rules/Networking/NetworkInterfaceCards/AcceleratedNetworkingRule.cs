using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class AcceleratedNetworkingRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput<NetworkInterfaceCard>> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput<NetworkInterfaceCard>>();

        if (!resource.EnableAcceleratedNetworking)
        {
            var level = Level.Note;
            var message = "Accelerated networking is not available or not enabled.";

            outputs.Add(new DefaultRuleOutput<NetworkInterfaceCard>(level, message, resource));
        }

        return outputs;
    }
}