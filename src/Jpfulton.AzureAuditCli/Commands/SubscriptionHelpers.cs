using Jpfulton.AzureAuditCli.Infrastructure;
using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands;

public static class SubscriptionHelpers
{
    public static async Task<List<Subscription>> GetSubscriptionsAsync(
        ResourceSettings settings,
        ProgressTask subscriptionsTask
        )
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

    public static async Task GetResourceGroupsAsync(
        Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> subscriptionToResources,
        ProgressTask rgTask,
        List<Subscription> subscriptions,
        string? jmesQuery = null
        )
    {
        var subscriptionCount = subscriptions.Count;
        var subscriptionCounter = 0;
        var rgProgressIncrement = 100.0 / subscriptionCount;

        rgTask.StartTask();
        foreach (var sub in subscriptions)
        {
            var groups = await AzCommand.GetAzureResourceGroupsAsync(Guid.Parse(sub.SubscriptionId));

            var groupToResourcesForSubscription = await GetResourcesAsync(sub, groups, jmesQuery);
            subscriptionToResources.Add(sub, groupToResourcesForSubscription);

            subscriptionCounter += 1;
            rgTask.Increment(rgProgressIncrement * subscriptionCounter);
        }
        rgTask.StopTask();
    }

    public static async Task<Dictionary<ResourceGroup, List<Resource>>> GetResourcesAsync(
        Subscription sub,
        ResourceGroup[] groups,
        string? jmesQuery = null
        )
    {
        var groupToResourcesForSubscription = new Dictionary<ResourceGroup, List<Resource>>();

        var tasks = new Dictionary<ResourceGroup, Task<Resource[]>>();
        groups.ToList().ForEach(rg =>
        {
            tasks.Add(rg, AzCommand.GetAzureResourcesAsync(Guid.Parse(sub.SubscriptionId), rg.Name, jmesQuery: jmesQuery));
        });

        await Task.WhenAll(tasks.Values.ToArray());

        tasks.Keys.ToList().ForEach(rg => groupToResourcesForSubscription.Add(rg, tasks[rg].Result.ToList()));

        return groupToResourcesForSubscription;
    }
}