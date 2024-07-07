# Audit Rule Outputs

> Rendered on: Sunday, July 7, 2024 11:06 AM <br/>
> Using command: azure-audit all <br/>
> Version: 0.0.48.0 <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 10
- Total evaluated resources: 3
- Total rule findings: 18

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
