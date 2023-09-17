using AzureAuditCli.Models;
using AzureAuditCli.Rules;
using Spectre.Console;

namespace AzureAuditCli.Commands;

public interface IRuleOutputCommand
{
    public Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>
        GetResourceDataAsync(
            ProgressTask? rgTask,
            List<Subscription> subscriptions
        );

    public IEnumerable<IRuleOutput> EvaluateRules(Resource r);
}