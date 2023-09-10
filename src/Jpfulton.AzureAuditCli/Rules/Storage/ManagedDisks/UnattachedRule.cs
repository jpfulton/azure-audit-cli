using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.ManagedDisks;

public class UnattachedRule : IRule<ManagedDisk>
{
    public IEnumerable<IRuleOutput> Evaluate(ManagedDisk resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.DiskState == DiskState.Unattached)
        {
            var level = Level.Warn;
            var message = "Managed disk is not attached to a virtual machine.";

            outputs.Add(new DefaultRuleOutput(level, message, resource));
        }

        return outputs;
    }
}