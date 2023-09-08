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
        bool fetchFullResource = false,
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

            var groupToResourcesForSubscription = await GetResourcesAsync(sub, groups, fetchFullResource, jmesQuery);
            subscriptionToResources.Add(sub, groupToResourcesForSubscription);

            subscriptionCounter += 1;
            rgTask.Increment(rgProgressIncrement * subscriptionCounter);
        }
        rgTask.StopTask();
    }

    public static async Task<Dictionary<ResourceGroup, List<Resource>>> GetResourcesAsync(
        Subscription sub,
        ResourceGroup[] groups,
        bool fetchFullResource = false,
        string? jmesQuery = null
        )
    {
        var data = new Dictionary<ResourceGroup, List<Resource>>();

        var tasks = new Dictionary<ResourceGroup, Task<Resource[]>>();
        groups.ToList().ForEach(rg =>
        {
            tasks.Add(rg, AzCommand.GetAzureResourcesAsync(Guid.Parse(sub.SubscriptionId), rg.Name, jmesQuery: jmesQuery));
        });

        await Task.WhenAll(tasks.Values);

        if (fetchFullResource)
        {
            var fullTasks = new Dictionary<ResourceGroup, List<Task<Resource>>>();
            tasks.Keys.ToList().ForEach(rg =>
            {
                var resourceTasks = new List<Task<Resource>>();
                tasks[rg].Result.ToList().ForEach(r => resourceTasks.Add(AzCommand.GetAzureResourceByIdAsync(r.Id)));

                fullTasks.Add(rg, resourceTasks);
            });

            var flatListOfAllTasks = new List<Task<Resource>>();
            fullTasks.Values.ToList().ForEach(l => flatListOfAllTasks.AddRange(l));
            await Task.WhenAll(flatListOfAllTasks);

            fullTasks.Keys.ToList().ForEach(group =>
            {
                var completeResources = new List<Resource>();
                fullTasks[group].ForEach(t => completeResources.Add(t.Result));

                data.Add(group, completeResources);
            });
        }
        else
        {
            tasks.Keys.ToList().ForEach(rg => data.Add(rg, tasks[rg].Result.ToList()));
        }


        return data;
    }
}