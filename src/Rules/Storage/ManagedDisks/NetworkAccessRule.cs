using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Rules.Storage.ManagedDisks;

public class NetworkAccessRule : IRule<ManagedDisk>
{
    public IEnumerable<IRuleOutput> Evaluate(ManagedDisk resource)
    {
        var outputs = new List<IRuleOutput>();

        if (
            (resource.DiskState == DiskState.ActiveSAS || resource.DiskState == DiskState.ActiveSASFrozen) &&
            resource.PublicNetworkAccess == PublicNetworkAccess.Enabled &&
            resource.NetworkAccessPolicy == NetworkAccessPolicy.AllowAll &&
            (resource.DataAccessAuthMode == DataAccessAuthMode.None || resource.DataAccessAuthMode == null)
            )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Critical,
                "Managed disk is configured for public network access without AAD authorization. It currently configured for export for those with the URL.",
                resource
            ));
        }
        else if (
            (resource.DiskState == DiskState.ActiveSAS || resource.DiskState == DiskState.ActiveSASFrozen) &&
            resource.PublicNetworkAccess == PublicNetworkAccess.Enabled &&
            resource.NetworkAccessPolicy == NetworkAccessPolicy.AllowAll &&
            resource.DataAccessAuthMode == DataAccessAuthMode.AzureActiveDirectory
            )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Critical,
                "Managed disk is configured for public network access with AAD authorization. It currently configured for export for those with the URL and AAD access.",
                resource
            ));
        }
        else if (
            resource.PublicNetworkAccess == PublicNetworkAccess.Enabled &&
            resource.NetworkAccessPolicy == NetworkAccessPolicy.AllowAll &&
            (resource.DataAccessAuthMode == DataAccessAuthMode.None || resource.DataAccessAuthMode == null)
            )
        {
            var level = Level.Warn;
            var message = "Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.";

            outputs.Add(new DefaultRuleOutput(level, message, resource));
        }
        else if (
            resource.PublicNetworkAccess == PublicNetworkAccess.Enabled &&
            resource.NetworkAccessPolicy == NetworkAccessPolicy.AllowAll &&
            resource.DataAccessAuthMode == DataAccessAuthMode.AzureActiveDirectory
        )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Warn,
                "Managed disk is configured for public network access using AAD authorization credentials. It may be enabled for authorized export at any time.",
                resource
            ));
        }
        else if (
            resource.PublicNetworkAccess == PublicNetworkAccess.Disabled &&
            resource.NetworkAccessPolicy == NetworkAccessPolicy.AllowPrivate
        )
        {
            outputs.Add(new DefaultRuleOutput(
                Level.Info,
                "Managed disk is configured for private network access.",
                resource
            ));
        }

        return outputs;
    }
}