using System.ComponentModel;
using Spectre.Console.Cli;

namespace Jpfulton.AzureAuditCli.Commands;

public abstract class BaseSettings : CommandSettings
{
    [CommandOption("--debug")]
    [Description("Increase logging verbosity to show all debug logs.")]
    [DefaultValue(false)]
    public bool Debug { get; set; }

    [CommandOption("-o|--output")]
    [Description("The output format to use. Defaults to Console (Console)")]
    public OutputFormat Output { get; set; } = OutputFormat.Console;
}