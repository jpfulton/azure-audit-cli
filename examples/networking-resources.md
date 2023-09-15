# Audit Rule Outputs

> Rendered on: Friday, September 15, 2023 7:04 PM <br/>
> Using command: azure-audit networking <br/>
> Version: 0.0.45.0 <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 11
- Total evaluated resources: 8
- Total rule findings: 1

### personal-network

- Location: northcentralus
- Total evaluated resources: 8
- Total resources with rule findings: 1
- Total rule findings: 1

<table>
<tr>
<th>Resource Type</th>
<th>Name</th>
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
