using Jpfulton.AzureAuditCli.Commands.Networking;
using Jpfulton.AzureAuditCli.Commands.Storage;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.Models.Storage;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands;

public class AllRulesCommand : BaseRuleOutputCommand<ResourceSettings, Resource>
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

        var networkingTask = new NetworkingCommand().GetResourceDataAsync(null, subscriptions);
        var storageTask = new StorageCommand().GetResourceDataAsync(null, subscriptions);

        await Task.WhenAll(networkingTask, storageTask);
        rgTask?.StopTask();

        return MergeData(networkingTask.Result, storageTask.Result);
    }

    public override IEnumerable<IRuleOutput> EvaluateRules(Resource r)
    {
        if (r is NetworkInterfaceCard)
            return RuleEvaluator<NetworkInterfaceCard>.Evaluate(r);
        else if (r is NetworkSecurityGroup)
            return RuleEvaluator<NetworkSecurityGroup>.Evaluate(r);
        else if (r is ManagedDisk)
            return RuleEvaluator<ManagedDisk>.Evaluate(r);
        else if (r is StorageAccount)
            return RuleEvaluator<StorageAccount>.Evaluate(r);
        else
            return new List<IRuleOutput>();
    }
}