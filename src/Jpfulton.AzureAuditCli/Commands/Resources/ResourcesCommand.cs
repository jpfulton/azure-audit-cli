using Jpfulton.AzureAuditCli.Infrastructure;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Resources;

public class ResourcesCommand : AsyncCommand<ResourcesSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ResourcesSettings settings)
    {
        if (settings.Debug)
            AnsiConsole.Write(
                new Markup($"[bold]Version:[/] {typeof(ResourcesCommand).Assembly.GetName().Version}\n")
                );

        var subscriptionToResources = new Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>();

        await AnsiConsole.Progress()
            .AutoRefresh(true) // Turn off auto refresh
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
                var rgTask = ctx.AddTask("[green]Getting resource groups[/]", new ProgressTaskSettings { AutoStart = false });

                var subscriptions = new List<Subscription>();

                subscriptionsTask.StartTask();
                if (settings.Subscription != Guid.Empty)
                {
                    subscriptions.Add(await AzCommand.GetAzureSubscriptionAsync(settings.Subscription));
                }
                else
                {
                    subscriptions.AddRange(await AzCommand.GetAzureSubscriptionsAsync());
                }
                subscriptionsTask.Increment(100.0);
                subscriptionsTask.StopTask();

                var subscriptionCount = subscriptions.Count;
                var subscriptionCounter = 0;
                var rgProgressIncrement = 100.0 / subscriptionCount;

                rgTask.StartTask();
                foreach (var sub in subscriptions)
                {
                    var groupToResourcesForSubscription = new Dictionary<ResourceGroup, List<Resource>>();
                    var groups = await AzCommand.GetAzureResourceGroupsAsync(Guid.Parse(sub.Id));

                    subscriptionCounter += 1;
                    rgTask.Increment(rgProgressIncrement * subscriptionCounter);

                    foreach (var group in groups)
                    {
                        var rTask = ctx.AddTask($"[green]Getting resources for {group.Name}[/]", new ProgressTaskSettings { AutoStart = false });
                        rTask.StartTask();

                        var resources = await AzCommand.GetAzureResourcesAsync(Guid.Parse(sub.Id), group.Name);
                        groupToResourcesForSubscription.Add(group, resources.ToList());

                        rTask.Increment(100.0);
                        rTask.StopTask();
                    }

                    subscriptionToResources.Add(sub, groupToResourcesForSubscription);
                }
                rgTask.StopTask();
            }
        );

        await OutputFormattersCollection.Formatters[settings.Output]
            .WriteResources(settings, subscriptionToResources);

        return 0;
    }
}