using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Rules;

public interface IRule<T> where T : Resource
{
    public IEnumerable<IRuleOutput> Evaluate(T resource);
}