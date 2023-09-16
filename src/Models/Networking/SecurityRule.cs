namespace AzureAuditCli.Models.Networking;

public enum Access
{
    Allow,
    Deny
}

public enum Direction
{
    Inbound,
    Outbound
}

public enum Protocol
{
    Any,
    ICMP,
    Tcp,
    Udp
}

public class SecurityRule : Resource
{
    public Access Access { get; set; } = Access.Deny;
    public string DestinationAddressPrefix { get; set; } = string.Empty;
    public string DestinationPortRange { get; set; } = string.Empty;
    public Direction Direction { get; set; } = Direction.Inbound;
    public int Priority { get; set; }
    public Protocol Protocol { get; set; } = Protocol.Any;
    public string SourceAddressPrefix { get; set; } = string.Empty;
    public string SourcePortRange { get; set; } = string.Empty;
}