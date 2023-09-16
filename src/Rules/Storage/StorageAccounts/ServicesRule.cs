using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.StorageAccounts;

public class ServicesRule : IRule<StorageAccount>
{
    public IEnumerable<IRuleOutput> Evaluate(StorageAccount resource)
    {
        var outputs = new List<IRuleOutput>();

        if (resource.ServicesBlobEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account Blob service is enabled.",
                resource
            ));
        }

        if (resource.ServicesDfsEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account Dfs service is enabled.",
                resource
            ));
        }

        if (resource.ServicesFileEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account File service is enabled.",
                resource
            ));
        }

        if (resource.ServicesQueueEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account Queue service is enabled.",
                resource
            ));
        }

        if (resource.ServicesTableEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account Table service is enabled.",
                resource
            ));
        }

        if (resource.ServicesWebEnabled)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Note,
                "Storage account Web service is enabled.",
                resource
            ));
        }

        return outputs;
    }
}