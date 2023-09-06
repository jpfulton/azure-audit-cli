using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public abstract class BaseOutputFormatter
{
    public abstract Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions);
}