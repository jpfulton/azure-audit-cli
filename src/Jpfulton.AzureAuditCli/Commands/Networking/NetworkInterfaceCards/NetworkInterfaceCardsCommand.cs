using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;

public class NetworkInterfaceCardsCommand
    : BaseRuleOutputCommand<ResourceSettings, NetworkInterfaceCard>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkInterfaceCard;
    }
}