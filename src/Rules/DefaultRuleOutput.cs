using AzureAuditCli.Models;
using Spectre.Console;

namespace AzureAuditCli.Rules;

public class DefaultRuleOutput : IRuleOutput
{
    public Level Level { get; private set; }

    public string Message { get; private set; }

    public Resource Resource { get; private set; }

    public DefaultRuleOutput(
        Level level,
        string message,
        Resource resource
    )
    {
        Level = level;
        Message = message;
        Resource = resource;
    }

    public Markup GetMarkup()
    {
        string format;
        switch (Level)
        {
            case Level.Info:
                format = "[bold blue]";
                break;
            case Level.Warn:
                format = "[bold yellow]";
                break;
            case Level.Critical:
                format = "[bold red]";
                break;
            default:
                format = "[bold]";
                break;
        }

        var output = $"{format}[[{Enum.GetName(Level)}]][/] {Message}";
        return new Markup(output);
    }
}