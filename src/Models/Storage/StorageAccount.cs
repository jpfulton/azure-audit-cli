namespace AzureAuditCli.Models.Storage;

public enum Kind
{
    BlobStorage,
    BlockBlobStorage,
    FileStorage,
    Storage,
    StorageV2
}

public enum TlsVersion
{
    TLS1_0,
    TLS1_1,
    TLS1_2
}

public enum KeyType
{
    Account,
    Service
}

public enum KeySource
{
    MicrosoftKeyvault,
    MicrosoftStorage
}

public class StorageAccount : Resource
{
    public bool AllowBlobPublicAccess { get; set; }
    public bool DefaultToOAuthAuthentication { get; set; }
    public KeySource EncryptionKeySource { get; set; } = KeySource.MicrosoftStorage;
    public bool? EncryptionServicesBlobEnabled { get; set; }
    public KeyType? EncryptionServicesBlobKeyType { get; set; }
    public bool? EncryptionServicesFileEnabled { get; set; }
    public KeyType? EncryptionServicesFileKeyType { get; set; }
    public bool? EncryptionServicesQueueEnabled { get; set; }
    public KeyType? EncryptionServicesQueueKeyType { get; set; }
    public bool? EncryptionServicesTableEnabled { get; set; }
    public KeyType? EncryptionServicesTableKeyType { get; set; }
    public Kind Kind { get; set; }
    public NetworkAcls? NetworkAcls { get; set; }
    public TlsVersion MinimumTlsVersion { get; set; } = TlsVersion.TLS1_0;
    public int PrivateEndpointConnectionsCount { get; set; }
    public bool RequireInfrastructureEncryption { get; set; }
    public bool ServicesBlobEnabled { get; set; }
    public bool ServicesDfsEnabled { get; set; }
    public bool ServicesFileEnabled { get; set; }
    public bool ServicesQueueEnabled { get; set; }
    public bool ServicesTableEnabled { get; set; }
    public bool ServicesWebEnabled { get; set; }
    public bool SupportsHttpsTrafficOnly { get; set; }
}

public enum NetworkAclAction
{
    Allow,
    Deny
}

public class NetworkAcls
{
    public string Bypass { get; set; } = string.Empty;
    public NetworkAclAction DefaultAction { get; set; } = NetworkAclAction.Allow;
    public int IpRulesCount { get; set; }
    public int IpV6RulesCount { get; set; }
    public int VirtualNetworkRulesCount { get; set; }
}