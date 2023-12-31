using AzureAuditCli.Models.Networking;

namespace AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class NetworkSecurityGroupRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput>();

        var publicIpCount = 0;
        resource.IpConfigurations.ForEach(config =>
        {
            if (config.PublicIpAddress != null) publicIpCount++;

            if (config.PublicIpAddress != null && resource.NetworkSecurityGroup == null)
            {
                var message = $"Contains a public IP address on configuration: '{config.Name}' and has no attached NSG.";
                outputs.Add(new DefaultRuleOutput(
                    Level.Warn,
                    message,
                    resource
                ));
            }
        });

        if (publicIpCount == 0 && resource.NetworkSecurityGroup == null)
        {
            var message = $"Contains only private IP addresses and has no attached.";
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                message,
                resource
            ));
        }

        return outputs;
    }
}