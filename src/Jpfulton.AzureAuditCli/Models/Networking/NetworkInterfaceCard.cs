namespace Jpfulton.AzureAuditCli.Models.Networking;

public class NetworkInterfaceCard : Resource
{
    public bool EnableAcceleratedNetworking { get; set; }
    public bool Primary { get; set; }
    public ResourceRef? VirtualMachine { get; set; }
    public bool VnetEncryptionSupported { get; set; }
}