using Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Networking;

public class NetworkingCommand : BaseRuleOutputCommand<ResourceSettings, Resource>
{
    public override async Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>
        GetResourceDataAsync(
            ProgressTask? rgTask,
            List<Subscription> subscriptions
        )
    {
        if (rgTask != null) rgTask.IsIndeterminate = true;
        rgTask?.StartTask();

        var nicDataTask = new NetworkInterfaceCardsCommand().GetResourceDataAsync(null, subscriptions);
        var nsgDataTask = new NetworkSecurityGroupsCommand().GetResourceDataAsync(null, subscriptions);

        await Task.WhenAll(nicDataTask, nsgDataTask);
        rgTask?.StopTask();

        return MergeData(nicDataTask.Result, nsgDataTask.Result);
    }

    protected override string GetAzureType()
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<IRuleOutput> EvaluateRules(Resource r)
    {
        if (r is NetworkInterfaceCard)
            return RuleEvaluator<NetworkInterfaceCard>.Evaluate(r);
        else if (r is NetworkSecurityGroup)
            return RuleEvaluator<NetworkSecurityGroup>.Evaluate(r);
        else
            return new List<IRuleOutput>();
    }
}