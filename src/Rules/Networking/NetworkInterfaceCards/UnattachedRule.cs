using AzureAuditCli.Models.Networking;

namespace AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class UnattachedRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.VirtualMachine == null)
        {
            var level = Level.Warn;
            var message = "NIC is not attached to a virtual machine.";

            outputs.Add(new DefaultRuleOutput(level, message, resource));
        }

        return outputs;
    }
}