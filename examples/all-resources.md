# Audit Rule Outputs

> Rendered on: Thursday, November 16, 2023 11:06 AM <br/>
> Using command: azure-audit all <br/>
> Version: 0.0.48.0 <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 11
- Total evaluated resources: 10
- Total rule findings: 24

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
- Total evaluated resources: 7
- Total resources with rule findings: 4
- Total rule findings: 6

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
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
<td><strong>backup-server-data-disk</strong></td>
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
<td><em>Microsoft.Network/networkSecurityGroups</em></td>
<td><strong>vpn-server-nsg</strong></td>
</tr>
<tr>
<td colspan="2">
<ul>
<li>
:yellow_circle:
<strong>[Warn]</strong>
Open and unfiltered Udp port(s): 1194 found.
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
