using Jpfulton.AzureAuditCli.Commands.NetworkSecurityGroups;
using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public abstract class BaseOutputFormatter
{
    public abstract Task WriteNetworkSecurityGroups(
        NetworkSecurityGroupsSettings settings,
        Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data
        );
    public abstract Task WriteResources(
        ResourcesSettings settings,
        Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data
        );
    public abstract Task WriteSubscriptions(
        SubscriptionsSettings settings,
        Subscription[] subscriptions
        );
}