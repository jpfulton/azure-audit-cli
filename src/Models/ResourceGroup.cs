using System.Reflection.Metadata.Ecma335;

namespace AzureAuditCli.Models;

public class ResourceGroup
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not ResourceGroup other) return false;
        else if (other == this) return true;
        else if (other.Id.Equals(Id)) return true;
        else return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}