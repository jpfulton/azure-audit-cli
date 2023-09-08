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

public interface IRuleOutput<T> where T : Resource
{
    public Level Level { get; }
    public string Message { get; }
    public T Resource { get; }

    public Markup GetMarkup();
}