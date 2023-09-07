using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public class ConsoleOutputFormatter : BaseOutputFormatter
{
    public override Task WriteResources(ResourcesSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
        /*
        var tableTitle = "[bold blue]Accessible Azure Subscriptions[/]";

        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle(tableTitle)
        };
        table.Expand();

        table
            .AddColumn("")
            .AddColumn("");
        */

        var tree = new Tree("Subscriptions");

        foreach (var sub in data.Keys)
        {
            var subTree = new Tree($"{sub.DisplayName} ({sub.Id})");
            var resourceGroupResource = data[sub];


            foreach (var rg in resourceGroupResource.Keys)
            {
                var rgTree = new Tree(rg.Name);

                foreach (var resource in resourceGroupResource[rg])
                {
                    rgTree.AddNode($"{resource.Name} ({resource.ResourceType})");
                }

                subTree.AddNode(rgTree);
            }

            tree.AddNode(subTree);
        }

        AnsiConsole.Write(tree);

        return Task.CompletedTask;
    }

    public override Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions)
    {
        var tableTitle = "[bold blue]Accessible Azure Subscriptions[/]";

        var table = new Table
        {
            Border = TableBorder.Rounded,
            ShowHeaders = true,
            Title = new TableTitle(tableTitle)
        };
        table.Expand();

        table
            .AddColumn("Id")
            .AddColumn("Display Name")
            .AddColumn("State");

        foreach (var subscription in subscriptions)
        {
            table.AddRow(
                new Markup(subscription.Id),
                new Markup(subscription.DisplayName),
                new Markup(subscription.State)
            );
        }

        AnsiConsole.Write(table);

        return Task.CompletedTask;
    }
}