using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.ManagedDisks;

public class EncryptionRule : IRule<ManagedDisk>
{
    public IEnumerable<IRuleOutput> Evaluate(ManagedDisk resource)
    {
        var outputs = new List<IRuleOutput>();

        if (
            resource.EncryptionType == null
        )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Critical,
                "Managed disk is not configured for encryption at rest.",
                resource
            ));
        }
        else if (
            resource.EncryptionType != null
        )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                $"Managed disk is encrypted at rest using {Enum.GetName(resource.EncryptionType.Value)}.",
                resource
            ));
        }

        return outputs;
    }
}