using AzureAuditCli.Models;
using AzureAuditCli.Models.Networking;
using AzureAuditCli.OutputFormatters;
using AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace AzureAuditCli.Commands.Networking.NetworkSecurityGroups;

public class NetworkSecurityGroupsCommand
    : BaseRuleOutputCommand<ResourceSettings, NetworkSecurityGroup>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkSecurityGroup;
    }
}