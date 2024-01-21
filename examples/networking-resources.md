# Audit Rule Outputs

> Rendered on: Sunday, January 21, 2024 11:06 AM <br/>
> Using command: azure-audit networking <br/>
> Version: 0.0.48.0 <br/>
> Resource groups and resources without rule findings will be omitted.

## JPF Pay-As-You-Go (4913be3f-a345-4652-9bba-767418dd25e3)

- Total resource groups: 11
- Total evaluated resources: 4
- Total rule findings: 1

### personal-network

- Location: northcentralus
- Total evaluated resources: 4
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
