using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public class ConsoleOutputFormatter : BaseOutputFormatter
{
    public override Task WriteResources(ResourcesSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
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

        foreach (var sub in data.Keys)
        {
            var resourceGroupResource = data[sub];

            var rgTable = new Table();
            rgTable.AddColumn("").AddColumn("");

            foreach (var rg in resourceGroupResource.Keys)
            {
                var rTable = new Table();
                rTable.AddColumns(
                    new TableColumn("Name"),
                    new TableColumn("Type")
                );

                foreach (var resource in resourceGroupResource[rg])
                {
                    rTable.AddRow(
                        resource.Name,
                        resource.ResourceType
                    );
                }

                rgTable.AddRow(new Markup(rg.Name), rTable);
            }

            table.AddRow(new Markup(sub.DisplayName), rgTable);
        }

        AnsiConsole.Write(table);

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