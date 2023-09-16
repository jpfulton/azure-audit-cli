using AzureAuditCli.Models;
using AzureAuditCli.Models.Storage;

namespace AzureAuditCli.Commands.Storage.ManagedDisks;

public class StorageAccountsCommand : BaseRuleOutputCommand<ResourceSettings, StorageAccount>
{
    protected override string GetAzureType()
    {
        return AzureResourceType.StorageAccount;
    }
}