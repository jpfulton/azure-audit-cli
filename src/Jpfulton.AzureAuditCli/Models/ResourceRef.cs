namespace Jpfulton.AzureAuditCli.Models;

public class ResourceRef
{
    public string Id { get; set; } = string.Empty;
    public string ResourceGroup { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not ResourceRef other) return false;
        else if (other == this) return true;
        else if (other.Id.Equals(Id)) return true;
        else return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}