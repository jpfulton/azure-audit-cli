using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.StorageAccounts;

public class HttpsAccessRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (!resource.SupportsHttpsTrafficOnly)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Storage account allows HTTP access in addition to HTTPS.",
                resource
            ));
        }

        return outputs;
    }
}