using Jpfulton.AzureAuditCli.Infrastructure;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Subscriptions;

public class SubscriptionsCommand : AsyncCommand<SubscriptionsSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, SubscriptionsSettings settings)
    {
        if (settings.Debug)
            AnsiConsole.WriteLine($"Version: {typeof(SubscriptionsCommand).Assembly.GetName().Version}");

        var subscriptions = await AzCommand.GetAzureSubscriptionsAsync();

        await OutputFormattersCollection.Formatters[settings.Output]
            .WriteSubscriptions(settings, subscriptions);

        return 0;
    }
}