using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Rules.Storage.StorageAccounts;

public class OAuthRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.DefaultToOAuthAuthentication == true)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                "Storage account defaults to OAuth authentication.",
                resource
            ));
        }

        return outputs;
    }
}