namespace AzureAuditCli.Models;

public class Subscription
{
    public string State { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not Subscription other) return false;
        else if (other == this) return true;
        else if (other.Id.Equals(Id)) return true;
        else return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}