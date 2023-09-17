using AzureAuditCli.Models;
using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Commands.Storage.ManagedDisks;

public class ManagedDisksCommand : BaseRuleOutputCommand<ResourceSettings, ManagedDisk>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.ManagedDisk;
    }
}