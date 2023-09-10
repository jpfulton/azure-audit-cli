using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Commands.Storage.ManagedDisks;

public class ManagedDisksCommand : BaseRuleOutputCommand<ResourceSettings, ManagedDisk>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.ManagedDisk;
    }
}