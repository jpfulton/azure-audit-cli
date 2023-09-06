using Jpfulton.AzureAuditCli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("azure-price");

#if DEBUG
    config.PropagateExceptions();
#endif

    config.ValidateExamples();
});

return await app.RunAsync(args);
