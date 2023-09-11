# Audit Rule Outputs

> Rendered on: Monday, September 11, 2023 9:52 PM <br/>
> Using command: azure-audit all <br/>
> Version: 0.0.34.0 <br/>
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

| Resource Type | Name |
|---|---|
| *Microsoft.Storage/storageAccounts* | **cs2100300008c8d894b** |
<td colspan="2">- [Note] Storage account Blob service is enabled. </td>
<td colspan="2">- [Note] Storage account Dfs service is enabled. </td>
<td colspan="2">- [Note] Storage account File service is enabled. </td>
<td colspan="2">- [Note] Storage account Queue service is enabled. </td>
<td colspan="2">- [Note] Storage account Table service is enabled. </td>
<td colspan="2">- [Note] Storage account Web service is enabled. </td>

### personal-network

- Location: northcentralus
- Total evaluated resources: 14
- Total resources with rule findings: 8
- Total rule findings: 20

| Resource Type | Name |
|---|---|
| *Microsoft.Compute/disks* | **backup-data-disk** |
<td colspan="2">- [Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. </td>
| *Microsoft.Compute/disks* | **linux-dev_OsDisk** |
<td colspan="2">- [Warn] Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime. </td>
<td colspan="2">- [Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. </td>
<td colspan="2">- [Note] Managed disk is reserved. Its managing virtual machine is currently deallocated. </td>
| *Microsoft.Compute/disks* | **ubuntu-backup-server-spot_OsDisk_1_82ad58ea1b864609a678571faedee9b3** |
<td colspan="2">- [Warn] Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime. </td>
<td colspan="2">- [Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. </td>
| *Microsoft.Compute/disks* | **ubuntu-vpn-server-spot_OsDisk** |
<td colspan="2">- [Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. </td>
| *Microsoft.Compute/disks* | **win-dev_OsDisk** |
<td colspan="2">- [Warn] Managed disk is configured for public network access using AAD authorization credentials. It may be enabled for authorized export at any time. </td>
<td colspan="2">- [Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey. </td>
<td colspan="2">- [Note] Managed disk is reserved. Its managing virtual machine is currently deallocated. </td>
| *Microsoft.Network/networkInterfaces* | **ubuntu-vpn-server-spot105** |
<td colspan="2">- [Note] Accelerated networking is not available or not enabled. </td>
| *Microsoft.Network/networkSecurityGroups* | **ubuntu-vpn-server-spot-nsg** |
<td colspan="2">- [Warn] Open and unfiltered UDP port(s): 1194 found. </td>
| *Microsoft.Storage/storageAccounts* | **jpfteststorageacct** |
<td colspan="2">- [Info] Storage account is accessible via 1 IP rule(s). </td>
<td colspan="2">- [Info] Storage account uses infrastructure encryption for double encryption. </td>
<td colspan="2">- [Note] Storage account Blob service is enabled. </td>
<td colspan="2">- [Note] Storage account Dfs service is enabled. </td>
<td colspan="2">- [Note] Storage account File service is enabled. </td>
<td colspan="2">- [Note] Storage account Queue service is enabled. </td>
<td colspan="2">- [Note] Storage account Table service is enabled. </td>
<td colspan="2">- [Note] Storage account Web service is enabled. </td>

### personal-site

- Location: centralus
- Total evaluated resources: 2
- Total resources with rule findings: 2
- Total rule findings: 12

| Resource Type | Name |
|---|---|
| *Microsoft.Storage/storageAccounts* | **personalsiteapipreview** |
<td colspan="2">- [Warn] Storage account allows public access. </td>
<td colspan="2">- [Info] Storage account defaults to OAuth authentication. </td>
<td colspan="2">- [Note] Storage account Blob service is enabled. </td>
<td colspan="2">- [Note] Storage account File service is enabled. </td>
<td colspan="2">- [Note] Storage account Queue service is enabled. </td>
<td colspan="2">- [Note] Storage account Table service is enabled. </td>
| *Microsoft.Storage/storageAccounts* | **personalsiteapistorage** |
<td colspan="2">- [Warn] Storage account allows public access. </td>
<td colspan="2">- [Info] Storage account defaults to OAuth authentication. </td>
<td colspan="2">- [Note] Storage account Blob service is enabled. </td>
<td colspan="2">- [Note] Storage account File service is enabled. </td>
<td colspan="2">- [Note] Storage account Queue service is enabled. </td>
<td colspan="2">- [Note] Storage account Table service is enabled. </td>



