using Jpfulton.AzureAuditCli.Commands.Resources;
using Jpfulton.AzureAuditCli.Commands.Subscriptions;
using Jpfulton.AzureAuditCli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("azure-audit");

#if DEBUG
    config.PropagateExceptions();
#endif

    config.AddCommand<ResourcesCommand>("resources")
        .WithDescription("List resources in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<SubscriptionsCommand>("subscriptions")
        .WithDescription("List subscriptions accessible with the current Azure CLI login.");

    config.ValidateExamples();
});

return await app.RunAsync(args);
