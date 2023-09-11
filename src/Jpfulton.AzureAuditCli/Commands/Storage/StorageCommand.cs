using Jpfulton.AzureAuditCli.Commands.Networking;
using Jpfulton.AzureAuditCli.Commands.Storage.ManagedDisks;
using Jpfulton.AzureAuditCli.Models;
using Jpfulton.AzureAuditCli.Models.Storage;
using Jpfulton.AzureAuditCli.Rules;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Commands.Storage;

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