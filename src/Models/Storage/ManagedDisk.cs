namespace AzureAuditCli.Models.Storage;

public enum DataAccessAuthMode
{
    AzureActiveDirectory,
    None
}

public enum DiskState
{
    ActiveSAS,
    ActiveSASFrozen,
    ActiveUpload,
    Attached,
    Frozen,
    ReadyToUpload,
    Reserved,
    Unattached
}

public enum EncryptionType
{
    EncryptionAtRestWithCustomerKey,
    EncryptionAtRestWithPlatformAndCustomerKeys,
    EncryptionAtRestWithPlatformKey
}

public enum NetworkAccessPolicy
{
    AllowAll,
    AllowPrivate,
    DenyAll
}

public enum PublicNetworkAccess
{
    Disabled,
    Enabled
}

public enum OsType
{
    Linux,
    Windows
}

public class ManagedDisk : Resource
{
    public string ManagedBy { get; set; } = string.Empty;
    public DataAccessAuthMode? DataAccessAuthMode { get; set; }
    public int DiskSizeGB { get; set; }
    public DiskState DiskState { get; set; }
    public EncryptionType? EncryptionType { get; set; }
    public NetworkAccessPolicy NetworkAccessPolicy { get; set; } = NetworkAccessPolicy.DenyAll;
    public OsType? OsType { get; set; }
    public PublicNetworkAccess PublicNetworkAccess { get; set; } = PublicNetworkAccess.Disabled;
}