using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.StorageAccounts;

public class MinimumTlsVersionRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.MinimumTlsVersion == TlsVersion.TLS1_0)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Critical,
                "Storage account allows TLS version 1.0.",
                resource
            ));
        }
        else if (resource.MinimumTlsVersion == TlsVersion.TLS1_1)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Storage account minimum TLS version is 1.1. Upgrade to minimum of 1.2 if possible.",
                resource
            ));
        }

        return outputs;
    }
}