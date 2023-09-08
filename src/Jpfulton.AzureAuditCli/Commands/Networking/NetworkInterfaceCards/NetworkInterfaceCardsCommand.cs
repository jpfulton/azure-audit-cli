using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.OutputFormatters;
using Jpfulton.AzureAuditCli.Rules;

namespace Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;

public class NetworkInterfaceCardsCommand
    : BaseRuleOutputCommand<NetworkInterfaceCardsSettings, NetworkInterfaceCard>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.NetworkInterfaceCard;
    }

    protected override Task WriteOutput(NetworkInterfaceCardsSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput<NetworkInterfaceCard>>>>> outputData)
    {
        return OutputFormattersCollection.Formatters[settings.Output]
            .WriteNetworkInterfaceCards(settings, outputData);
    }
}