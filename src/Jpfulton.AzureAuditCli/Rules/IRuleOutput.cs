using Jpfulton.AzureAuditCli.Models;
using Spectre.Console;

namespace Jpfulton.AzureAuditCli.Rules;

public enum Level
{
    Note = 0,
    Info = 1,
    Warn = 2,
    Critical = 3
}

public interface IRuleOutput
{
    public Level Level { get; }
    public string Message { get; }
    public Resource Resource { get; }

    public Markup GetMarkup();
}