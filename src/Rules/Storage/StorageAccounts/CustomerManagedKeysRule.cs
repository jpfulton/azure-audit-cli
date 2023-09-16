using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.StorageAccounts;

public class CustomerManagedKeysRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.EncryptionKeySource == KeySource.MicrosoftKeyvault)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Storage account uses customer managed keys. Key rotation and revocation onus is on customer.",
                resource
            ));
        }

        return outputs;
    }
}