using System.Text.Json;
using System.Text.Json.Serialization;
using Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Networking;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;
using Spectre.Console.Json;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public class JsonOutputFormatter : BaseOutputFormatter
{
    public override Task WriteNetworkInterfaceCards(NetworkInterfaceCardsSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput<NetworkInterfaceCard>>>>> data)
    {
        WriteJson(FlattenRuleOutputs(data));
        return Task.CompletedTask;
    }

    public override Task WriteNetworkSecurityGroups(NetworkSecurityGroupsSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput<NetworkSecurityGroup>>>>> data)
    {
        WriteJson(FlattenRuleOutputs(data));
        return Task.CompletedTask;
    }

    public override Task WriteResources(ResourcesSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> data)
    {
        throw new NotImplementedException();
    }

    public override Task WriteSubscriptions(SubscriptionsSettings settings, Subscription[] subscriptions)
    {
        WriteJson(subscriptions);
        return Task.CompletedTask;
    }

    private static List<FlattenedRuleOutput> FlattenRuleOutputs<TResource>(
        Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput<TResource>>>>> data
    ) where TResource : Resource
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

    private void WriteJson(object data)
    {
        var options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        options.Converters.Add(new LevelJsonConverter());

        var json = JsonSerializer.Serialize(data, options);

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