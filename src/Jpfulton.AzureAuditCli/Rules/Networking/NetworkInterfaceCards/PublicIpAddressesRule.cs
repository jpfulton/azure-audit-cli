using Jpfulton.AzureAuditCli.Models.Networking;

namespace Jpfulton.AzureAuditCli.Rules.Networking.NetworkInterfaceCards;

public class PublicIpAddressesRule : IRule<NetworkInterfaceCard>
{
    public IEnumerable<IRuleOutput<NetworkInterfaceCard>> Evaluate(NetworkInterfaceCard resource)
    {
        var outputs = new List<IRuleOutput<NetworkInterfaceCard>>();

        var publicIpCount = 0;
        resource.IpConfigurations.ForEach(config =>
        {
            if (config.PublicIpAddress != null) publicIpCount++;
        });

        if (publicIpCount == 0)
        {
            outputs.Add(new DefaultRuleOutput<NetworkInterfaceCard>(
                Level.Info,
                "No public IP addresses found. A NAT Gateway must be in place on the network for internet access.",
                resource
            ));
        }
        else if (publicIpCount > 1)
        {
            outputs.Add(new DefaultRuleOutput<NetworkInterfaceCard>(
                Level.Info,
                "Multiple public IP address found.",
                resource
            ));
        }

        return outputs;
    }
}