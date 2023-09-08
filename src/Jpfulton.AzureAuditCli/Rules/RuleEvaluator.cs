using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Rules;

public static class RuleEvaluator
{
    public static IEnumerable<IRuleOutput<T>> Evaluate<T>(T resource) where T : Resource
    {
        var outputs = new List<IRuleOutput<T>>();

        var rules = GetRules<T>();
        rules.ForEach(r => outputs.AddRange(r.Evaluate(resource)));

        return outputs;
    }

    private static List<IRule<T>> GetRules<T>() where T : Resource
    {
        var rules = new List<IRule<T>>();

        var ruleType = typeof(IRule<T>);
        var assembly = typeof(RuleEvaluator).Assembly;

        var ruleTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(ruleType) && !type.IsInterface && !type.IsAbstract);

        ruleTypes.ToList().ForEach(rt => rules.Add((IRule<T>)Activator.CreateInstance(rt)!));
        return rules;
    }
}