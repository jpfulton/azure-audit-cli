using AzureAuditCli.Models.Networking;

namespace AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class PublicIpAddressesRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput>();

        var publicIpCount = 0;
        resource.IpConfigurations.ForEach(config =>
        {
            if (config.PublicIpAddress != null) publicIpCount++;
        });

        if (publicIpCount == 0)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                "No public IP addresses found. A NAT Gateway must be in place on the network for internet access.",
                resource
            ));
        }
        else if (publicIpCount > 1)
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                "Multiple public IP address found.",
                resource
            ));
        }

        return outputs;
    }
}