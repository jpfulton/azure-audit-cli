using Jpfulton.AzureAuditCli.Commands.Networking;
using Jpfulton.AzureAuditCli.Commands.Storage;
using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Commands;

public class AllRulesCommand : BaseAggregateRuleOutputCommand
{
    protected override List<IRuleOutputCommand> GetSubCommands()
    {
        return new List<IRuleOutputCommand>
        {
            new NetworkingCommand(),
            new StorageCommand()
        };
    }
}