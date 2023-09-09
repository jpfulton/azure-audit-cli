using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;

public class NetworkSecurityGroupsCommand
    : BaseRuleOutputCommand<ResourceSettings, NetworkSecurityGroup>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkSecurityGroup;
    }

    protected override Task WriteOutput(ResourceSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> outputData)
    {
        return OutputFormattersCollection.Formatters[settings.Output]
            .WriteNetworkSecurityGroups(settings, outputData);
    }
}