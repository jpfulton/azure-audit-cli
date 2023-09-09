using Jpfulton.AzureAuditCli.Models;

namespace Jpfulton.AzureAuditCli.Rules;

public static class RuleEvaluator<T> where T : Resource
{
    private static readonly List<IRule<T>> rules = GetRules();

    public static int RuleCount { get { return rules.Count; } }

    public static IEnumerable<IRuleOutput> Evaluate(Resource input)
    {
        T resource = input as T ?? throw new ArgumentException("Input is not of correct type.", "input");

        var outputs = new List<IRuleOutput>();

        rules.ForEach(r => outputs.AddRange(r.Evaluate(resource)));

        return outputs.OrderByDescending(o => o.Level).ThenBy(o => o.Message);
    }

    private static List<IRule<T>> GetRules()
    {
        var ruleInstances = new List<IRule<T>>();

        var ruleType = typeof(IRule<T>);
        var assembly = typeof(RuleEvaluator<T>).Assembly;

        var ruleTypes = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(ruleType) && !type.IsInterface && !type.IsAbstract);

        ruleTypes.ToList().ForEach(rt => ruleInstances.Add((IRule<T>)Activator.CreateInstance(rt)!));
        return ruleInstances;
    }
}