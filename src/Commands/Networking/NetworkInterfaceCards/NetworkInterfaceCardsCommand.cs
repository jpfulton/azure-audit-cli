using AzureAuditCli.Models;
using AzureAuditCli.Models.Networking;
using AzureAuditCli.OutputFormatters;
using AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace AzureAuditCli.Commands.Networking.NetworkInterfaceCards;

public class NetworkInterfaceCardsCommand
    : BaseRuleOutputCommand<ResourceSettings, NetworkInterfaceCard>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkInterfaceCard;
    }
}