using Jpfulton.AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using Jpfulton.AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands.Networking;

public class NetworkingCommand : BaseRuleOutputCommand<ResourceSettings, Resource>
{
    public override async Task<Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>>
        GetResourceDataAsync(
            ProgressTask rgTask,
            List<Subscription> subscriptions
        )
    {
        var nicDataTask = new NetworkInterfaceCardsCommand().GetResourceDataAsync(rgTask, subscriptions);
        var nsgDataTask = new NetworkSecurityGroupsCommand().GetResourceDataAsync(rgTask, subscriptions);

        await Task.WhenAll(nicDataTask, nsgDataTask);

        return MergeData(nicDataTask.Result, nsgDataTask.Result);
    }

    protected override string GetAzureType()
    {
        throw new NotImplementedException();
    }

    protected override Task WriteOutput(ResourceSettings settings, Dictionary<Subscription, Dictionary<ResourceGroup, Dictionary<Resource, List<IRuleOutput>>>> outputData)
    {
        throw new NotImplementedException();
    }

    private static Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>> MergeData(
        params Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>[] data
        )
    {
        var output = new Dictionary<Subscription, Dictionary<ResourceGroup, List<Resource>>>();

        foreach (var result in data)
        {
            result.Keys.ToList().ForEach(sub =>
            {
                result[sub].Keys.ToList().ForEach(rg =>
                {
                    var resourceList = result[sub][rg];

                    if (output.ContainsKey(sub))
                    {
                        if (output[sub].ContainsKey(rg))
                        {
                            output[sub][rg].AddRange(resourceList);
                        }
                        else
                        {
                            output[sub].Add(rg, resourceList);
                        }
                    }
                    else
                    {
                        output.Add(sub, new Dictionary<ResourceGroup, List<Resource>>()
                        {
                            {rg, resourceList}
                        });
                    }
                });
            });
        }

        return output;
    }
}