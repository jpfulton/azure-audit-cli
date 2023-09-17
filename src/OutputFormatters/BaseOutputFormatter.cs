using AzureAuditCli.Commands;
using AzureAuditCli.Commands.Resources;
using AzureAuditCli.Commands.Subscriptions;
using AzureAuditCli.Models;
using AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace AzureAuditCli.OutputFormatters;

public abstract class BaseOutputFormatter
{
    public abstract Task WriteRuleOutputs(
        ResourceSettings settings,
        CommandContext commandContext,
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