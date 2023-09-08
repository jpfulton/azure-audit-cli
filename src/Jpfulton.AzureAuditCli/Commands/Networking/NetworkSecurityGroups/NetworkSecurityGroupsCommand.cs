using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;

public class NetworkSecurityGroupsCommand
    : BaseRuleOutputCommand<NetworkSecurityGroupsSettings, NetworkSecurityGroup>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkSecurityGroup;
    }

    protected override Task WriteOutput(NetworkSecurityGroupsSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput<NetworkSecurityGroup>>>>> outputData)
    {
        return OutputFormattersCollection.Formatters[settings.Output]
            .WriteNetworkSecurityGroups(settings, outputData);
    }
}