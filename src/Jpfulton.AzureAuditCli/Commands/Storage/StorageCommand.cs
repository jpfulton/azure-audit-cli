using Jpfulton.AzureAuditCli.Commands.Networking;
using Jpfulton.AzureAuditCli.Commands.Storage.ManagedDisks;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Storage;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands.Storage;

public class StorageCommand : BaseRuleOutputCommand<ResourceSettings, Resource>
{
    protected override string GetAzureType()
    {
        throw new NotImplementedException();
    }

    public override async Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>
        GetResourceDataAsync(
            ProgressTask? rgTask,
            List<Subscription> subscriptions
        )
    {
        if (rgTask != null) rgTask.IsIndeterminate = true;
        rgTask?.StartTask();

        var disksTask = new ManagedDisksCommand().GetResourceDataAsync(null, subscriptions);
        var storageAcctsTask = new StorageAccountsCommand().GetResourceDataAsync(null, subscriptions);

        await Task.WhenAll(disksTask, storageAcctsTask);
        rgTask?.StopTask();

        return MergeData(disksTask.Result, storageAcctsTask.Result);
    }

    public override IEnumerable<IRuleOutput> EvaluateRules(Resource r)
    {
        if (r is ManagedDisk)
            return RuleEvaluator<ManagedDisk>.Evaluate(r);
        else if (r is StorageAccount)
            return RuleEvaluator<StorageAccount>.Evaluate(r);
        else
            return new List<IRuleOutput>();
    }
}