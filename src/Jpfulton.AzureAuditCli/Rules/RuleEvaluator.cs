using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Rules;

public static class RuleEvaluator<T> where T : Resource
{
    private static readonly List<IRule<T>> rules = GetRules();

    public static IEnumerable<IRuleOutput<T>> Evaluate(T resource)
    {
        var outputs = new List<IRuleOutput<T>>();

        rules.ForEach(r => outputs.AddRange(r.Evaluate(resource)));

        return outputs.OrderByDescending(o => o.Level).ThenBy(o => o.Message);
    }

    private static List<IRule<T>> GetRules()
    {
        var rules = new List<IRule<T>>();

        var ruleType = typeof(IRule<T>);
        var assembly = typeof(RuleEvaluator<T>).Assembly;

        var ruleTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(ruleType) && !type.IsInterface && !type.IsAbstract);

        ruleTypes.ToList().ForEach(rt => rules.Add((IRule<T>)Activator.CreateInstance(rt)!));
        return rules;
    }
}