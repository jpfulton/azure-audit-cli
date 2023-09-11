using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands;

public abstract class BaseRuleOutputCommand<TSettings, TResource>
    : AsyncCommand<TSettings>, IRuleOutputCommand
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

        var outputData = new Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput>
                >
            >
        >();

        if (settings.Output == OutputFormat.Console)
        {
            AnsiConsole.Write(new Markup($"[bold]Executing command [italic]{context.Name}[/]...[/]").Centered());
            AnsiConsole.WriteLine();
        }

        await AnsiConsole.Progress()
            .AutoRefresh(true) // Turn on auto refresh
            .AutoClear(settings.Output != OutputFormat.Console)   // Do not remove the task list when done
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
                var rulesTask = ctx.AddTask("[green]Evaluating rules[/]", new ProgressTaskSettings { AutoStart = false });

                var subscriptions = await SubscriptionHelpers.GetSubscriptionsAsync(settings, subscriptionsTask);

                var data = await GetResourceDataAsync(rgTask, subscriptions);

                outputData = EvaluateRulesAndBuildOutputData(data, rulesTask);
            }
        );

        await WriteOutput(settings, context, outputData);

        return 0;
    }

    public virtual async Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>
        GetResourceDataAsync(
            ProgressTask? rgTask,
            List<Subscription> subscriptions
            )
    {
        var data = new Dictionary<
            Subscription, Dictionary<ResourceGroup, List<Resource>>
        >();

        var jmesQuery = $"[?type == `{GetAzureType()}`]";
        await SubscriptionHelpers.GetResourceGroupsAsync(data, rgTask, subscriptions, true, jmesQuery);

        return data;
    }

    private Dictionary<
        Subscription, Dictionary<
            ResourceGroup, Dictionary<
                Resource, List<IRuleOutput>
            >
        >
    > EvaluateRulesAndBuildOutputData(
        Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data,
        ProgressTask progressTask
    )
    {
        var resourceCount = GetResourceCount(data);
        var progressIncrement = 100.0 / resourceCount;

        progressTask.StartTask();

        var outputData = new Dictionary<
                    Subscription, Dictionary<
                        ResourceGroup, Dictionary<
                            Resource, List<IRuleOutput>
                        >
                    >
                >();

        data.Keys.ToList().ForEach(sub =>
        {
            var rgToResources = new Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput>
                >
            >();

            data[sub].Keys.ToList().ForEach(rg =>
            {
                var resourceToRuleOutputs = new Dictionary<
                    Resource, List<IRuleOutput>
                    >();

                data[sub][rg].ForEach(r =>
                {
                    IEnumerable<IRuleOutput> ruleOutputs = EvaluateRules(r);
                    resourceToRuleOutputs.Add(r, ruleOutputs.ToList());

                    progressTask.Increment(progressIncrement * resourceCount);
                });

                rgToResources.Add(rg, resourceToRuleOutputs);
            });

            outputData.Add(sub, rgToResources);
        });

        progressTask.StopTask();
        return outputData;
    }

    public virtual IEnumerable<IRuleOutput> EvaluateRules(Resource r)
    {
        return RuleEvaluator<TResource>.Evaluate(r);
    }

    private static int GetResourceCount(Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
        var resourceCount = 0;
        data.Values.ToList().ForEach(d =>
        {
            d.Values.ToList().ForEach(l =>
            {
                resourceCount += l.Count;
            });
        });

        return resourceCount;
    }

    protected abstract string GetAzureType();

    protected virtual Task WriteOutput(
        TSettings settings,
        CommandContext commandContext,
        Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput>
                >
            >
        > outputData
    )
    {
        return OutputFormattersCollection.Formatters[settings.Output]
            .WriteRuleOutputs(settings, commandContext, outputData);
    }

    protected static Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> MergeData(
        params Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>[] data
        )
    {
        var output = new Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>();

        foreach (var result in data)
        {
            result.Keys.ToList().ForEach(sub =>
            {
                result[sub].Keys.ToList().ForEach(rg =>
                {
                    var resourceList = result[sub][rg];

                    if (output.TryGetValue(sub, out var rgDictionary))
                    {
                        if (output[sub].TryGetValue(rg, out var rList))
                        {
                            rList.AddRange(resourceList);
                        }
                        else
                        {
                            rgDictionary.Add(rg, resourceList);
                        }
                    }
                    else
                    {
                        output.Add(sub, new Dictionary<ResourceGroup, List<Resource>>()
                        {
                            {rg, resourceList}
                        });
                    }
                });
            });
        }

        return output;
    }
}