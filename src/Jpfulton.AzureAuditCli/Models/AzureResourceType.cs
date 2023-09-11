namespace Jpfulton.AzureAuditCli.Models;

public static class AzureResourceType
{
    public const string ManagedDisk = "Microsoft.Compute/disks";
    public const string IpConfiguration = "Microsoft.Network/networkInterfaces/ipConfigurations";
    public const string NetworkInterfaceCard = "Microsoft.Network/networkInterfaces";
    public const string NetworkSecurityGroup = "Microsoft.Network/networkSecurityGroups";
    public const string SecurityRule = "Microsoft.Network/networkSecurityGroups/securityRules";
    public const string StorageAccount = "Microsoft.Storage/storageAccounts";
}