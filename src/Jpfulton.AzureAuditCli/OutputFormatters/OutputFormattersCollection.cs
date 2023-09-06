using Jpfulton.AzureAuditCli.Commands;

namespace Jpfulton.AzureAuditCli.OutputFormatters;

public static class OutputFormattersCollection
{
    public static Dictionary<OutputFormat, BaseOutputFormatter> Formatters { get; } = new();

    static OutputFormattersCollection()
    {
        Formatters.Add(OutputFormat.Console, new ConsoleOutputFormatter());
    }
}