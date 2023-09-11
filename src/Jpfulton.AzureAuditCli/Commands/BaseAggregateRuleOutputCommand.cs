using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands;

public abstract class BaseAggregateRuleOutputCommand : BaseRuleOutputCommand<ResourceSettings, Resource>
{
    protected abstract List<IRuleOutputCommand> GetSubCommands();

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

        var dataTasks = new List<Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>>();
        foreach (var command in GetSubCommands())
        {
            dataTasks.Add(command.GetResourceDataAsync(null, subscriptions));
        }

        await Task.WhenAll(dataTasks);
        rgTask?.StopTask();

        var outputs = new List<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>();
        dataTasks.ForEach(t => outputs.Add(t.Result));

        return MergeData(outputs.ToArray());
    }

    public override IEnumerable<IRuleOutput> EvaluateRules(Resource r)
    {
        var outputs = new List<IRuleOutput>();

        foreach (var command in GetSubCommands())
        {
            try
            {
                outputs.AddRange(command.EvaluateRules(r));
            }
            catch (ArgumentException) { }
        }

        return outputs;
    }
}