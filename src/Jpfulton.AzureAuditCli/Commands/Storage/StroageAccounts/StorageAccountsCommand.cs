using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Storage;

namespace Jpfulton.AzureAuditCli.Commands.Storage.ManagedDisks;

public class StorageAccountsCommand : BaseRuleOutputCommand<ResourceSettings, StorageAccount>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.StorageAccount;
    }
}