using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.StorageAccounts;

public class PublicAccessRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.AllowBlobPublicAccess == true)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Storage account allows public access.",
                resource
            ));
        }

        return outputs;
    }
}