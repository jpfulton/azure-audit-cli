using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;

public class NetworkSecurityGroupsCommand
    : BaseRuleOutputCommand<ResourceSettings, NetworkSecurityGroup>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkSecurityGroup;
    }
}