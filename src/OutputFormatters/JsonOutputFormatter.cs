using System.Text.Json;
using System.Text.Json.Serialization;
using DevLab.JmesPath;
using AzureAuditCli.Commands;
using AzureAuditCli.Commands.Resources;
using AzureAuditCli.Commands.Subscriptions;
using AzureAuditCli.Models;
using AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

namespace AzureAuditCli.OutputFormatters;

public class JsonOutputFormatter : BaseOutputFormatter
{
    public override Task WriteRuleOutputs(
        ResourceSettings settings,
        CommandContext commandContext,
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> data
        )
    {
        WriteJson(settings, FlattenRuleOutputs(data));
        return Task.CompletedTask;
    }

    public override Task WriteResources(ResourcesSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
        throw new NotImplementedException();
    }

    public override Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions)
    {
        WriteJson(settings, subscriptions);
        return Task.CompletedTask;
    }

    private static List<FlattenedRuleOutput> FlattenRuleOutputs(
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> data
    )
    {
        var output = new List<FlattenedRuleOutput>();

        data.Keys.ToList().ForEach(sub =>
        {
            data[sub].Keys.ToList().ForEach(rg =>
            {
                data[sub][rg].Keys.ToList().ForEach(r =>
                {
                    data[sub][rg][r].ForEach(o =>
                    {
                        output.Add(new FlattenedRuleOutput(
                            sub,
                            rg,
                            r,
                            o.Level,
                            o.Message
                        ));
                    });
                });
            });
        });

        return output;
    }

    private void WriteJson(BaseSettings settings, object data)
    {
        var options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        options.Converters.Add(new LevelJsonConverter());

        var json = JsonSerializer.Serialize(data, options);

        if (!string.IsNullOrWhiteSpace(settings.Query))
        {
            var jmesPath = new JmesPath();
            json = jmesPath.Transform(json, settings.Query);
        }

        AnsiConsole.Write(new JsonText(json)
            .BracesColor(Color.Red)
            .BracketColor(Color.Green)
            .ColonColor(Color.Blue)
            .CommaColor(Color.Red)
            .StringColor(Color.Green)
            .NumberColor(Color.Blue)
            .BooleanColor(Color.Red)
            .NullColor(Color.Green)
        );
        AnsiConsole.WriteLine();
    }
}

public record FlattenedRuleOutput(
    Subscription Subscription,
    ResourceGroup ResourceGroup,
    Resource Resource,
    Level Level,
    string Message
);

public sealed class LevelJsonConverter : JsonConverter<Level>
{
    public override Level Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Level value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Enum.GetName(value));
    }
}