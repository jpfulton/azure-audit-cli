using Jpfulton.AzureAuditCli.Commands;
using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Rules;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public abstract class BaseOutputFormatter
{
    public abstract Task WriteRuleOutputs(
        ResourceSettings settings,
        Dictionary<
            Subscription, Dictionary<
                ResourceGroup, Dictionary<
                    Resource, List<IRuleOutput>
                >
            >
        > data
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