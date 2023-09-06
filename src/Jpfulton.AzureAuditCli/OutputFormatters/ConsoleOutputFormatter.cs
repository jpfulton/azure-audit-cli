using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public class ConsoleOutputFormatter : BaseOutputFormatter
{
    public override Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions)
    {
        var tableTitle = "[blue]Accessible Azure Subscriptions[/]";

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