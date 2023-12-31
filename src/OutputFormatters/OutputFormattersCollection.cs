using AzureAuditCli.Commands;

namespace AzureAuditCli.OutputFormatters;

public static class OutputFormattersCollection
{
    public static Dictionary<OutputFormat, BaseOutputFormatter> Formatters { get; } = new();

    static OutputFormattersCollection()
    {
        Formatters.Add(OutputFormat.Console, new ConsoleOutputFormatter());
        Formatters.Add(OutputFormat.Json, new JsonOutputFormatter());
        Formatters.Add(OutputFormat.Markdown, new MarkdownOutputFormatter());
    }
}