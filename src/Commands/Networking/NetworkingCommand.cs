using AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using AzureAuditCli.Models;
using AzureAuditCli.Models.Networking;
using AzureAuditCli.OutputFormatters;
using AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AzureAuditCli.Commands.Networking;

public class NetworkingCommand : BaseAggregateRuleOutputCommand
{
    protected override List<IRuleOutputCommand> GetSubCommands()
    {
        return new List<IRuleOutputCommand>
        {
            new NetworkInterfaceCardsCommand(),
            new NetworkSecurityGroupsCommand()
        };
    }
}