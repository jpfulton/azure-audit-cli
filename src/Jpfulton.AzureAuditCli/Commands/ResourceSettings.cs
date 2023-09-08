using System.ComponentModel;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands;

public class ResourceSettings : BaseSettings
{
    [CommandOption("-s|--subscription")]
    [Description("The subscription id to use. Will all accessible subscriptions if not specified.")]
    public Guid Subscription { get; set; }

    [CommandOption("-r|--resource-group")]
    [Description("Resource group to use. All resource groups will be used if not specified.")]
    public string ResourceGroup { get; set; } = string.Empty;
}