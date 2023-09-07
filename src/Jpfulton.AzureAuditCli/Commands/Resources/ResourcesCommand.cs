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

        var subscriptions = new List<Subscription>();

        if (settings.Subscription != Guid.Empty)
        {
            subscriptions.Add(await AzCommand.GetAzureSubscriptionAsync(settings.Subscription));
        }
        else
        {
            subscriptions.AddRange(await AzCommand.GetAzureSubscriptionsAsync());
        }

        var subscriptionToResources = new Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>();

        foreach (var sub in subscriptions)
        {
            var groupToResourcesForSubscription = new Dictionary<ResourceGroup, List<Resource>>();
            var groups = await AzCommand.GetAzureResourceGroupsAsync(Guid.Parse(sub.Id));

            foreach (var group in groups)
            {
                var resources = await AzCommand.GetAzureResourcesAsync(Guid.Parse(sub.Id), group.Name);
                groupToResourcesForSubscription.Add(group, resources.ToList());
            }

            subscriptionToResources.Add(sub, groupToResourcesForSubscription);
        }

        await OutputFormattersCollection.Formatters[settings.Output]
            .WriteResources(settings, subscriptionToResources);

        return 0;
    }
}