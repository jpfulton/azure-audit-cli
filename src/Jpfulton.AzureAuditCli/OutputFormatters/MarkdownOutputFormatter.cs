using System.Globalization;
using System.Text;
using Jpfulton.AzureAuditCli.Commands;
using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public class MarkdownOutputFormatter : BaseOutputFormatter
{
    public override Task WriteResources(ResourcesSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
        throw new NotImplementedException();
    }

    public override Task WriteRuleOutputs(
        ResourceSettings settings,
        CommandContext commandContext,
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> data
        )
    {
        WriteRuleOutputsMarkdown(settings, commandContext, data);
        return Task.CompletedTask;
    }

    public override Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions)
    {
        throw new NotImplementedException();
    }

    private static void WriteRuleOutputsMarkdown(
        ResourceSettings settings,
        CommandContext commandContext,
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> data
        )
    {
        var output = new StringBuilder();

        output.AppendLine("# Audit Rule Outputs");
        output.AppendLine();
        output.AppendLine($"> Rendered on: {DateTime.Now.ToString("f", CultureInfo.CreateSpecificCulture("en-US"))}");
        output.AppendLine($"> Using command: azure-audit {commandContext.Name}");
        output.AppendLine("> Resource groups and resources without rule findings will be omitted.");
        output.AppendLine();

        data.Keys
            .OrderBy(s => s.DisplayName)
            .ToList()
            .ForEach(subscription =>
            {
                var subscriptionData = data[subscription];
                var subscriptionResourceCount = GetSubscriptionResourceCount(subscriptionData);
                var subscriptionTotalFindings = GetSubscriptionTotalFindingsCount(subscriptionData);

                output.AppendLine($"## {subscription.DisplayName} ({subscription.SubscriptionId})");
                output.AppendLine();

                output.AppendLine($"- Total resource groups: {subscriptionData.Keys.Count}");
                output.AppendLine($"- Total evaluated resources: {subscriptionResourceCount}");
                output.AppendLine($"- Total rule findings: {subscriptionTotalFindings}");
                output.AppendLine();

                subscriptionData.Keys
                    .Where(rg => subscriptionData[rg].Values.Any(l => l.Count > 0))
                    .OrderBy(rg => rg.Name)
                    .ToList()
                    .ForEach(resourceGroup =>
                    {
                        var resourceGroupData = subscriptionData[resourceGroup];
                        var resourceCount = resourceGroupData.Keys.Count;

                        var resourcesWithFindingsCount = 0;
                        var totalFindings = 0;
                        resourceGroupData.Keys.ToList().ForEach(r =>
                        {
                            var resourceGroupFindings = resourceGroupData[r].Count;

                            resourcesWithFindingsCount += resourceGroupFindings > 0 ? 1 : 0;
                            totalFindings += resourceGroupFindings;
                        });

                        output.AppendLine($"### {resourceGroup.Name}");
                        output.AppendLine();

                        output.AppendLine($"- Location: {resourceGroup.Location}");
                        output.AppendLine($"- Total evaluated resources: {resourceCount}");
                        output.AppendLine($"- Total resources with rule findings: {resourcesWithFindingsCount}");
                        output.AppendLine($"- Total rule findings: {totalFindings}");
                        output.AppendLine();

                        output.AppendLine("|---|---|---|---|");
                        output.Append("| Resource Type ");
                        output.Append("| Name ");
                        output.Append("| Level ");
                        output.Append("| Message ");
                        output.Append("|\n");

                        resourceGroupData.Keys
                        .Where(r => resourceGroupData[r].Count > 0)
                        .OrderBy(r => r.ResourceType)
                        .ThenBy(r => r.Name)
                        .ToList()
                        .ForEach(resource =>
                        {
                            resourceGroupData[resource]
                            .OrderByDescending(o => o.Level)
                            .ThenBy(o => o.Message)
                            .ToList()
                            .ForEach(ruleOutput =>
                            {
                                output.Append($"| *{resource.ResourceType}* ");
                                output.Append($"| **{resource.Name}** ");
                                output.Append($"| [{Enum.GetName(ruleOutput.Level)}] ");
                                output.Append($"| {ruleOutput.Message} ");
                                output.Append("|\n");
                            });
                        });

                        output.AppendLine();
                    });
            });
        output.AppendLine();

        Console.WriteLine(output);
    }

    private static int GetSubscriptionResourceCount(
        Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>> subscriptionData
        )
    {
        var subscriptionResourceCount = 0;
        subscriptionData.Keys.ToList().ForEach(rg =>
        {
            subscriptionResourceCount += subscriptionData[rg].Keys.Count;
        });
        return subscriptionResourceCount;
    }

    private static int GetSubscriptionTotalFindingsCount(
        Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>> subscriptionData
        )
    {
        var totalFindingsCount = 0;
        subscriptionData.Keys.ToList().ForEach(rg =>
        {
            subscriptionData[rg].Keys
                .ToList()
                .ForEach(r =>
                {
                    totalFindingsCount += subscriptionData[rg][r].Count;
                });
        });
        return totalFindingsCount;
    }
}