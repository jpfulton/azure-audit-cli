# Audit Rule Outputs

> Rendered on: Monday, September 11, 2023 9:16 PM <br/>
> Using command: azure-audit all <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 11
- Total evaluated resources: 17
- Total rule findings: 38

### cloud-shell-storage-eastus

- Location: eastus
- Total evaluated resources: 1
- Total resources with rule findings: 1
- Total rule findings: 6

| Resource Type | Name | Level | Message |
|---|---|---|---|
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account Blob service is enabled. |
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account Dfs service is enabled. |
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account File service is enabled. |
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account Queue service is enabled. |
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account Table service is enabled. |
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** | [Note] | Storage account Web service is enabled. |

### personal-network

- Location: northcentralus
- Total evaluated resources: 14
- Total resources with rule findings: 8
- Total rule findings: 20

| Resource Type | Name | Level | Message |
|---|---|---|---|
| *Microsoft.Compute/disks* | **backup-data-disk** | [Note] | Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. |
| *Microsoft.Compute/disks* | **linux-dev_OsDisk** | [Warn] | Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime. |
| *Microsoft.Compute/disks* | **linux-dev_OsDisk** | [Note] | Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. |
| *Microsoft.Compute/disks* | **linux-dev_OsDisk** | [Note] | Managed disk is reserved. Its managing virtual machine is currently deallocated. |
| *Microsoft.Compute/disks* | **ubuntu-backup-server-spot_OsDisk_1_82ad58ea1b864609a678571faedee9b3** | [Warn] | Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime. |
| *Microsoft.Compute/disks* | **ubuntu-backup-server-spot_OsDisk_1_82ad58ea1b864609a678571faedee9b3** | [Note] | Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. |
| *Microsoft.Compute/disks* | **ubuntu-vpn-server-spot_OsDisk** | [Note] | Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. |
| *Microsoft.Compute/disks* | **win-dev_OsDisk** | [Warn] | Managed disk is configured for public network access using AAD authorization credentials. It may be enabled for authorized export at any time. |
| *Microsoft.Compute/disks* | **win-dev_OsDisk** | [Note] | Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. |
| *Microsoft.Compute/disks* | **win-dev_OsDisk** | [Note] | Managed disk is reserved. Its managing virtual machine is currently deallocated. |
| *Microsoft.Network/networkInterfaces* | **ubuntu-vpn-server-spot105** | [Note] | Accelerated networking is not available or not enabled. |
| *Microsoft.Network/networkSecurityGroups* | **ubuntu-vpn-server-spot-nsg** | [Warn] | Open and unfiltered UDP port(s): 1194 found. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Info] | Storage account is accessible via 1 IP rule(s). |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Info] | Storage account uses infrastructure encryption for double encryption. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account Blob service is enabled. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account Dfs service is enabled. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account File service is enabled. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account Queue service is enabled. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account Table service is enabled. |
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** | [Note] | Storage account Web service is enabled. |

### personal-site

- Location: centralus
- Total evaluated resources: 2
- Total resources with rule findings: 2
- Total rule findings: 12

| Resource Type | Name | Level | Message |
|---|---|---|---|
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Warn] | Storage account allows public access. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Info] | Storage account defaults to OAuth authentication. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Note] | Storage account Blob service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Note] | Storage account File service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Note] | Storage account Queue service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** | [Note] | Storage account Table service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Warn] | Storage account allows public access. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Info] | Storage account defaults to OAuth authentication. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Note] | Storage account Blob service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Note] | Storage account File service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Note] | Storage account Queue service is enabled. |
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** | [Note] | Storage account Table service is enabled. |



