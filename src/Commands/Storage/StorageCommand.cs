using AzureAuditCli.Commands.Networking;
using AzureAuditCli.Commands.Storage.ManagedDisks;
using AzureAuditCli.Models;
using AzureAuditCli.Models.Storage;
using AzureAuditCli.Rules;
using Spectre.Console;

namespace AzureAuditCli.Commands.Storage;

public class StorageCommand : BaseAggregateRuleOutputCommand
{
    protected override List<IRuleOutputCommand> GetSubCommands()
    {
        return new List<IRuleOutputCommand>
        {
            new ManagedDisksCommand(),
            new StorageAccountsCommand()
        };
    }
}