using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.ManagedDisks;

public class ReservedRule : IRule<ManagedDisk>
{
    public IEnumerable<IRuleOutput> Evaluate(ManagedDisk resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.DiskState == DiskState.Reserved)
        {
            var level = Level.Note;
            var message = "Managed disk is reserved. Its managing virtual machine is currently deallocated.";

            outputs.Add(new DefaultRuleOutput(level, message, resource));
        }

        return outputs;
    }
}