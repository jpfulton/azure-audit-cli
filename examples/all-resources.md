# Audit Rule Outputs

> Rendered on: Monday, September 11, 2023 10:17 PM <br/>
> Using command: azure-audit all <br/>
> Version: 0.0.36.0 <br/>
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

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
</tr>
<tr>
<td>*Microsoft.Storage/storageAccounts*</td>
<td>**cs2100300008c8d894b**</td>
</tr>
<tr><td colspan="2"><ul><li>[Note] Storage account Blob service is enabled.</li>
<li>[Note] Storage account Dfs service is enabled.</li>
<li>[Note] Storage account File service is enabled.</li>
<li>[Note] Storage account Queue service is enabled.</li>
<li>[Note] Storage account Table service is enabled.</li>
<li>[Note] Storage account Web service is enabled.</li>
</ul></td></tr>
</table>

### personal-network

- Location: northcentralus
- Total evaluated resources: 14
- Total resources with rule findings: 8
- Total rule findings: 20

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
</tr>
<tr>
<td>*Microsoft.Compute/disks*</td>
<td>**backup-data-disk**</td>
</tr>
<tr><td colspan="2"><ul><li>[Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Compute/disks*</td>
<td>**linux-dev_OsDisk**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.</li>
<li>[Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.</li>
<li>[Note] Managed disk is reserved. Its managing virtual machine is currently deallocated.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Compute/disks*</td>
<td>**ubuntu-backup-server-spot_OsDisk_1_82ad58ea1b864609a678571faedee9b3**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Managed disk is configured for public network access and an allow all network access policy with no data access authorization policy. It may be enabled for export at anytime.</li>
<li>[Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Compute/disks*</td>
<td>**ubuntu-vpn-server-spot_OsDisk**</td>
</tr>
<tr><td colspan="2"><ul><li>[Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Compute/disks*</td>
<td>**win-dev_OsDisk**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Managed disk is configured for public network access using AAD authorization credentials. It may be enabled for authorized export at any time.</li>
<li>[Note] Managed disk is encrypted at rest using EncryptionAtRestWithPlatformKey.</li>
<li>[Note] Managed disk is reserved. Its managing virtual machine is currently deallocated.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Network/networkInterfaces*</td>
<td>**ubuntu-vpn-server-spot105**</td>
</tr>
<tr><td colspan="2"><ul><li>[Note] Accelerated networking is not available or not enabled.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Network/networkSecurityGroups*</td>
<td>**ubuntu-vpn-server-spot-nsg**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Open and unfiltered UDP port(s): 1194 found.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Storage/storageAccounts*</td>
<td>**jpfteststorageacct**</td>
</tr>
<tr><td colspan="2"><ul><li>[Info] Storage account is accessible via 1 IP rule(s).</li>
<li>[Info] Storage account uses infrastructure encryption for double encryption.</li>
<li>[Note] Storage account Blob service is enabled.</li>
<li>[Note] Storage account Dfs service is enabled.</li>
<li>[Note] Storage account File service is enabled.</li>
<li>[Note] Storage account Queue service is enabled.</li>
<li>[Note] Storage account Table service is enabled.</li>
<li>[Note] Storage account Web service is enabled.</li>
</ul></td></tr>
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
<td>*Microsoft.Storage/storageAccounts*</td>
<td>**personalsiteapipreview**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Storage account allows public access.</li>
<li>[Info] Storage account defaults to OAuth authentication.</li>
<li>[Note] Storage account Blob service is enabled.</li>
<li>[Note] Storage account File service is enabled.</li>
<li>[Note] Storage account Queue service is enabled.</li>
<li>[Note] Storage account Table service is enabled.</li>
</ul></td></tr>
<tr>
<td>*Microsoft.Storage/storageAccounts*</td>
<td>**personalsiteapistorage**</td>
</tr>
<tr><td colspan="2"><ul><li>[Warn] Storage account allows public access.</li>
<li>[Info] Storage account defaults to OAuth authentication.</li>
<li>[Note] Storage account Blob service is enabled.</li>
<li>[Note] Storage account File service is enabled.</li>
<li>[Note] Storage account Queue service is enabled.</li>
<li>[Note] Storage account Table service is enabled.</li>
</ul></td></tr>
</table>



