using AzureAuditCli.Commands.Networking;
using AzureAuditCli.Commands.Storage;
using AzureAuditCli.Models;

namespace AzureAuditCli.Commands;

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