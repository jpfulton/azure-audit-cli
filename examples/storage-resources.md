# Audit Rule Outputs

> Rendered on: Friday, September 15, 2023 11:07 AM <br/>
> Using command: azure-audit storage <br/>
> Version: 0.0.44.0 <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 11
- Total evaluated resources: 11
- Total rule findings: 40

### cloud-shell-storage-eastus

- Location: eastus
- Total evaluated resources: 1
- Total resources with rule findings: 1
- Total rule findings: 6

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
</tr>
<tr>
<td><em>Microsoft.Storage/storageAccounts</em></td>
<td><strong>cs2100300008c8d894b</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Blob service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Dfs service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account File service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Queue service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Table service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Web service is enabled.
</li>
</ul>
</td>
</tr>
</table>

### personal-network

- Location: northcentralus
- Total evaluated resources: 8
- Total resources with rule findings: 8
- Total rule findings: 22

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>backup-data-disk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>backup-server_OsDisk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>linux-dev_OsDisk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is reserved. Its managing virtual machine is currently deallocated.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>ubuntu-backup-server-spot_OsDisk_1_82ad58ea1b864609a678571faedee9b3</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>ubuntu-vpn-server-spot_OsDisk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>vpn-server_OsDisk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Compute/disks</em></td>
<td><strong>win-dev_OsDisk</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Managed disk is configured for public network access using AAD authorization credentials. It may be enabled for authorized export at any time.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Managed disk is reserved. Its managing virtual machine is currently deallocated.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Storage/storageAccounts</em></td>
<td><strong>backupstorage32f3</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:large_blue_circle:
<strong>[Info]</strong>
Storage account is accessible via 1 virtual network rule(s).
</li>
<li>
:large_blue_circle:
<strong>[Info]</strong>
Storage account uses infrastructure encryption for double encryption.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Blob service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Dfs service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account File service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Queue service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Table service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Web service is enabled.
</li>
</ul>
</td>
</tr>
</table>

### personal-site

- Location: centralus
- Total evaluated resources: 2
- Total resources with rule findings: 2
- Total rule findings: 12

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
</tr>
<tr>
<td><em>Microsoft.Storage/storageAccounts</em></td>
<td><strong>personalsiteapipreview</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Storage account allows public access.
</li>
<li>
:large_blue_circle:
<strong>[Info]</strong>
Storage account defaults to OAuth authentication.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Blob service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account File service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Queue service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Table service is enabled.
</li>
</ul>
</td>
</tr>
<tr>
<td><em>Microsoft.Storage/storageAccounts</em></td>
<td><strong>personalsiteapistorage</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Storage account allows public access.
</li>
<li>
:large_blue_circle:
<strong>[Info]</strong>
Storage account defaults to OAuth authentication.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Blob service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account File service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Queue service is enabled.
</li>
<li>
:green_circle:
<strong>[Note]</strong>
Storage account Table service is enabled.
</li>
</ul>
</td>
</tr>
</table>
