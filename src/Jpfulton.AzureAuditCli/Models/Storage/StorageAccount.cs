namespace Jpfulton.AzureAuditCli.Models.Storage;

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
    public TlsVersion MinimumTlsVersion { get; set; } = TlsVersion.TLS1_0;
    public bool SupportsHttpsTrafficOnly { get; set; }
}