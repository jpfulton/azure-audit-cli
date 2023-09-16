using AzureAuditCli.Commands;
using AzureAuditCli.Commands.Networking;
using AzureAuditCli.Commands.Networking.NetworkInterfaceCards;
using AzureAuditCli.Commands.Networking.NetworkSecurityGroups;
using AzureAuditCli.Commands.Resources;
using AzureAuditCli.Commands.Storage;
using AzureAuditCli.Commands.Storage.ManagedDisks;
using AzureAuditCli.Commands.Subscriptions;
using AzureAuditCli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("azure-audit");

    //#if DEBUG
    config.PropagateExceptions();
    //#endif

    config.AddCommand<AllRulesCommand>("all")
        .WithDescription("Audit all resources in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<ManagedDisksCommand>("disks")
        .WithDescription("Audit managed disks in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<NetworkingCommand>("networking")
        .WithDescription("Audit networking resources in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<NetworkInterfaceCardsCommand>("nic")
        .WithDescription("Audit NICs in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<NetworkSecurityGroupsCommand>("nsg")
        .WithDescription("Audit NSGs in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<ResourcesCommand>("resources")
        .WithDescription("List resources in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<StorageCommand>("storage")
        .WithDescription("Audit storage resources in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<StorageAccountsCommand>("storageAccounts")
        .WithDescription("Audit storage accounts in subscriptions accessible with the current Azure CLI login.");

    config.AddCommand<SubscriptionsCommand>("subscriptions")
        .WithDescription("List subscriptions accessible with the current Azure CLI login.");

    config.AddExample(
        "networking",
        "--output",
        "Json",
        "--query",
        "[].{resourceName: Resource.Name, level: Level, message: Message}"
        );

    config.ValidateExamples();
});

return await app.RunAsync(args);
