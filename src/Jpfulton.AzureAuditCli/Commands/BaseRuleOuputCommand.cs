using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands;

public abstract class BaseRuleOutputCommand<TSettings, TResource> : AsyncCommand<TSettings>
    where TSettings : ResourceSettings
    where TResource : Resource
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        TSettings settings
        )
    {
        if (settings.Debug)
            AnsiConsole.Write(
                new Markup($"[bold]Version:[/] {typeof(BaseRuleOutputCommand<TSettings, TResource>).Assembly.GetName().Version}\n")
                );

        var jmesQuery = $"[?type == `{GetAzureType()}`]";
        var data = new Dictionary<
            Subscription, Dictionary<ResourceGroup, List<Resource>>
        >();

        await AnsiConsole.Progress()
            .AutoRefresh(true) // Turn on auto refresh
            .AutoClear(false)   // Do not remove the task list when done
            .HideCompleted(false)   // Hide tasks as they are completed
            .Columns(new ProgressColumn[]
            {
                new SpinnerColumn(),            // Spinner
                new TaskDescriptionColumn(),    // Task description
                new ProgressBarColumn(),        // Progress bar
                new PercentageColumn(),         // Percentage
                new ElapsedTimeColumn(),        // Elapsed time
            })
            .StartAsync(async ctx =>
            {
                var subscriptionsTask = ctx.AddTask("[green]Getting subscriptions[/]", new ProgressTaskSettings { AutoStart = false });
                var rgTask = ctx.AddTask("[green]Getting resources[/]", new ProgressTaskSettings { AutoStart = false });

                var subscriptions = await SubscriptionHelpers.GetSubscriptionsAsync(settings, subscriptionsTask);
                await SubscriptionHelpers.GetResourceGroupsAsync(data, rgTask, subscriptions, true, jmesQuery);
            }
        );

        var outputData = new Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput<TResource>>
                >
            >
        >();

        data.Keys.ToList().ForEach(sub =>
        {
            var rgToResources = new Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput<TResource>>
                >
            >();

            data[sub].Keys.ToList().ForEach(rg =>
            {
                var resourceToRuleOutputs = new Dictionary<
                    Resource, List<IRuleOutput<TResource>>
                    >();

                data[sub][rg].ForEach(r =>
                {
                    var ruleOutputs = RuleEvaluator<TResource>.Evaluate(r);
                    resourceToRuleOutputs.Add(r, ruleOutputs.ToList());
                });

                rgToResources.Add(rg, resourceToRuleOutputs);
            });

            outputData.Add(sub, rgToResources);
        });

        await WriteOutput(settings, outputData);

        return 0;
    }

    protected abstract string GetAzureType();

    protected abstract Task WriteOutput(
        TSettings settings,
        Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput<TResource>>
                >
            >
        > outputData
    );
}