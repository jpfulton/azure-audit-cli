using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class UnattachedRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput<NetworkInterfaceCard>> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput<NetworkInterfaceCard>>();

        if (resource.VirtualMachine == null)
        {
            var level = Level.Warn;
            var message = "NIC is not attached to a virtual machine.";

            outputs.Add(new DefaultRuleOutput<NetworkInterfaceCard>(level, message, resource));
        }

        return outputs;
    }
}