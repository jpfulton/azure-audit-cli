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

                var subscriptions = await GetSubscriptions(settings, subscriptionsTask);
                await GetResourceGroups(subscriptionToResources, rgTask, subscriptions);
            }
        );

        await OutputFormattersCollection.Formatters[settings.Output]
            .WriteResources(settings, subscriptionToResources);

        return 0;
    }

    private static async Task GetResourceGroups(Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> subscriptionToResources, ProgressTask rgTask, List<Subscription> subscriptions)
    {
        var subscriptionCount = subscriptions.Count;
        var subscriptionCounter = 0;
        var rgProgressIncrement = 100.0 / subscriptionCount;

        rgTask.StartTask();
        foreach (var sub in subscriptions)
        {
            var groups = await AzCommand.GetAzureResourceGroupsAsync(Guid.Parse(sub.SubscriptionId));

            var groupToResourcesForSubscription = GetResources(sub, groups);
            subscriptionToResources.Add(sub, groupToResourcesForSubscription);

            subscriptionCounter += 1;
            rgTask.Increment(rgProgressIncrement * subscriptionCounter);
        }
        rgTask.StopTask();
    }

    private static Dictionary<ResourceGroup, List<Resource>> GetResources(Subscription sub, ResourceGroup[] groups)
    {
        var groupToResourcesForSubscription = new Dictionary<ResourceGroup, List<Resource>>();

        var tasks = new Dictionary<ResourceGroup, Task<Resource[]>>();
        groups.ToList().ForEach(rg =>
        {
            tasks.Add(rg, AzCommand.GetAzureResourcesAsync(Guid.Parse(sub.SubscriptionId), rg.Name));
        });

        Task.WaitAll(tasks.Values.ToArray());

        tasks.Keys.ToList().ForEach(rg => groupToResourcesForSubscription.Add(rg, tasks[rg].Result.ToList()));

        return groupToResourcesForSubscription;
    }

    private static async Task<List<Subscription>> GetSubscriptions(ResourcesSettings settings, ProgressTask subscriptionsTask)
    {
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
        return subscriptions;
    }
}