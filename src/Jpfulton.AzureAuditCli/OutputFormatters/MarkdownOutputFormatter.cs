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
            output.AppendLine($"## {subscription.DisplayName}");
            output.AppendLine();

            data[subscription].Keys
                .Where(rg => data[subscription][rg].Values.Any(l => l.Count > 0))
                .OrderBy(rg => rg.Name)
                .ToList()
                .ForEach(resourceGroup =>
            {
                output.AppendLine($"### {resourceGroup.Name}");
                output.AppendLine();

                output.AppendLine("|---|---|---|---|");
                output.Append("| Resource Type ");
                output.Append("| Name ");
                output.Append("| Level ");
                output.Append("| Message ");
                output.Append("|\n");

                data[subscription][resourceGroup].Keys
                    .Where(r => data[subscription][resourceGroup][r].Count > 0)
                    .OrderBy(r => r.ResourceType)
                    .ThenBy(r => r.Name)
                    .ToList()
                    .ForEach(resource =>
                {
                    data[subscription][resourceGroup][resource]
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

        Console.WriteLine(output.ToString());
    }
}