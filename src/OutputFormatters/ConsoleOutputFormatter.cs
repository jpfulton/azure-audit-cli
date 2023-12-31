using AzureAuditCli.Commands;
using AzureAuditCli.Commands.Resources;
using AzureAuditCli.Commands.Subscriptions;
using AzureAuditCli.Models;
using AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AzureAuditCli.OutputFormatters;

public class ConsoleOutputFormatter : BaseOutputFormatter
{
    public override Task WriteRuleOutputs(
        ResourceSettings settings,
        CommandContext commandContext,
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> data
        )
    {
        return WriteRuleOutputTree(data);
    }

    public override Task WriteResources(
        ResourcesSettings settings,
        Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data
        )
    {
        var tree = new Tree("[bold]Subscriptions[/]");

        foreach (var sub in data.Keys)
        {
            var subTree = new Tree($"[bold blue]{sub.DisplayName} ({sub.SubscriptionId})[/]");
            var resourceGroupResource = data[sub];

            foreach (var rg in resourceGroupResource.Keys)
            {
                var rgTree = new Tree(
                    $"[bold green]{rg.Name} ({rg.Location}) -> [[{resourceGroupResource[rg].Count} resource(s)]][/]"
                );

                foreach (var resource in resourceGroupResource[rg])
                {
                    rgTree.AddNode($"([italic]{resource.ResourceType}[/]) [bold]{resource.Name}[/]");
                }

                subTree.AddNode(rgTree);
            }

            tree.AddNode(subTree);
        }

        AnsiConsole.Write(tree);

        return Task.CompletedTask;
    }

    public override Task WriteSubscriptions(
        SubscriptionsSettings settings,
        Subscription[] subscriptions
        )
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
                new Markup(subscription.SubscriptionId),
                new Markup(subscription.DisplayName),
                new Markup(subscription.State)
            );
        }

        AnsiConsole.Write(table);

        return Task.CompletedTask;
    }

    private static Task WriteRuleOutputTree(
        Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput>
                >
            >
        > data
    )
    {
        var tree = new Tree("[bold]Subscriptions[/]");

        foreach (var sub in data.Keys)
        {
            var subTree = new Tree($"[bold blue]{sub.DisplayName} ({sub.SubscriptionId})[/]");
            var resourceGroupResource = data[sub];

            foreach (var pair in resourceGroupResource.Where(p => p.Value.Count > 0))
            {
                var rgTree = new Tree(
                    $"[bold green]{pair.Key.Name} ({pair.Key.Location}) -> [[{pair.Value.Count} resource(s)]][/]"
                );

                foreach (var resource in pair.Value.Keys)
                {
                    var rTree = new Tree($"([dim italic]{resource.ResourceType}[/]) [bold]{resource.Name}[/]");
                    pair.Value[resource].ToList().ForEach(o => rTree.AddNode(o.GetMarkup()));

                    rgTree.AddNode(rTree);
                }

                subTree.AddNode(rgTree);
            }

            tree.AddNode(subTree);
        }

        AnsiConsole.Write(new Markup("[bold]Audit Rule Outputs[/]").Centered());
        AnsiConsole.WriteLine();
        AnsiConsole.Write(tree);
        AnsiConsole.WriteLine();

        return Task.CompletedTask;
    }
}