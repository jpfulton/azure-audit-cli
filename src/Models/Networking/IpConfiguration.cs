namespace AzureAuditCli.Models.Networking;

public class IpConfiguration : Resource
{
    public bool Primary { get; set; }
    public string PrivateIpAddress { get; set; } = string.Empty;
    public ResourceRef? PublicIpAddress { get; set; }
    public ResourceRef? Subnet { get; set; }
}