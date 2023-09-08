using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkSecurityGroups;

public class OpenInboundPortsRule : IRule<NetworkSecurityGroup>
{
    private static readonly string DISALLOWED_UDP_PORTS_RANGE = "53,67-69,123,135,137-139,161-162,445,500,514,520,631,1434,1900,4500,49152";
    private static readonly string DISALLOWED_TCP_PORTS_RANGE = "20,21-23,25,53,80,110-111,135,139,143,443,445,993,995,1723,3306,3389,5900,8080";

    private static readonly List<int> DISALLOWED_UDP_PORTS = ParseRange(DISALLOWED_UDP_PORTS_RANGE);
    private static readonly List<int> DISALLOWED_TCP_PORTS = ParseRange(DISALLOWED_TCP_PORTS_RANGE);

    public IEnumerable<IRuleOutput<NetworkSecurityGroup>> Evaluate(NetworkSecurityGroup resource)
    {
        var outputs = new List<IRuleOutput<NetworkSecurityGroup>>();

        outputs.AddRange(EvaluateSecurityRules(resource));

        return outputs.OrderByDescending(o => o.Level).ThenBy(o => o.Message);
    }

    private IEnumerable<IRuleOutput<NetworkSecurityGroup>> EvaluateSecurityRules(
        NetworkSecurityGroup resource
    )
    {
        var outputs = new List<IRuleOutput<NetworkSecurityGroup>>();

        resource.SecurityRules
            .Where(r =>
                r.Access == Access.Allow &&
                r.Direction == Direction.Inbound &&
                r.SourceAddressPrefix.Equals("*")
            )
            .ToList()
            .ForEach(rule =>
            {
                var destinationPorts = ParseRange(rule.DestinationPortRange);

                Level level;
                if (
                    (rule.Protocol == Protocol.TCP && destinationPorts.Intersect(DISALLOWED_TCP_PORTS).Count() > 0) ||
                    (rule.Protocol == Protocol.UDP && destinationPorts.Intersect(DISALLOWED_UDP_PORTS).Count() > 0)
                )
                {
                    level = Level.Critical;
                }
                else
                {
                    level = Level.Warn;
                }

                var protocol = Enum.GetName(rule.Protocol);
                var message = $"Open and unfiltered {protocol} port(s): {rule.DestinationPortRange} found.";

                outputs.Add(new DefaultRuleOutput<NetworkSecurityGroup>(
                    level,
                    message,
                    resource
                ));
            });

        return outputs;
    }

    private static List<int> ParseRange(string input)
    {
        var results = (from x in input.Split(',')
                       let y = x.Split('-')
                       select y.Length == 1
                         ? new[] { int.Parse(y[0]) }
                         : Enumerable.Range(int.Parse(y[0]), int.Parse(y[1]) - int.Parse(y[0]) + 1)
               ).SelectMany(x => x).ToList();

        return results;
    }
}