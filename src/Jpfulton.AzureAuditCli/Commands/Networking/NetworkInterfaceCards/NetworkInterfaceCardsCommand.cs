using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;

public class NetworkInterfaceCardsCommand : AsyncCommand<NetworkInterfaceCardsSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        NetworkInterfaceCardsSettings settings
        )
    {
        if (settings.Debug)
            AnsiConsole.Write(
                new Markup($"[bold]Version:[/] {typeof(NetworkInterfaceCardsCommand).Assembly.GetName().Version}\n")
                );

        var jmesQuery = "[?type == `Microsoft.Network/networkSecurityGroups`]";
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
                    Resource, List<IRuleOutput<NetworkInterfaceCard>>
                >
            >
        >();

        data.Keys.ToList().ForEach(sub =>
        {
            var rgToResources = new Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput<NetworkInterfaceCard>>
                >
            >();

            data[sub].Keys.ToList().ForEach(rg =>
            {
                var resourceToRuleOutputs = new Dictionary<
                    Resource, List<IRuleOutput<NetworkInterfaceCard>>
                    >();

                data[sub][rg].ForEach(r =>
                {
                    var ruleOutputs = RuleEvaluator<NetworkInterfaceCard>.Evaluate(r);
                    resourceToRuleOutputs.Add(r, ruleOutputs.ToList());
                });

                rgToResources.Add(rg, resourceToRuleOutputs);
            });

            outputData.Add(sub, rgToResources);
        });

        await OutputFormattersCollection.Formatters[settings.Output]
            .WriteNetworkInterfaceCards(settings, outputData);

        return 0;
    }
}