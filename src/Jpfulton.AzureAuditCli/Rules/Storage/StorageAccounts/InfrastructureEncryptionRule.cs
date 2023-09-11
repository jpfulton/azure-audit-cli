using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.StorageAccounts;

public class InfrastructureEncryptionRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.RequireInfrastructureEncryption == true)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                "Storage account uses infrastructure encryption for double encryption.",
                resource
            ));
        }

        return outputs;
    }
}