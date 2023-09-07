using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.NetworkSecurityGroups;

public class NetworkSecurityGroupsCommand : AsyncCommand<NetworkSecurityGroupsSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, NetworkSecurityGroupsSettings settings)
    {
        if (settings.Debug)
            AnsiConsole.Write(
                new Markup($"[bold]Version:[/] {typeof(NetworkSecurityGroupsCommand).Assembly.GetName().Version}\n")
                );

        var data = new Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>();

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
                await SubscriptionHelpers.GetResourceGroupsAsync(data, rgTask, subscriptions);
            }
        );

        return 0;
    }
}