using Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Networking;

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