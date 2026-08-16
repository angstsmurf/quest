using QuestViva.Engine.Scripts;

namespace QuestViva.Engine.Functions;

// qvh patch: a dynamictemplate expression that failed to parse at load.
// Legacy Quest deferred that parse to first use; do the same, so the error
// (if ever reached) surfaces as a runtime script error, not a load failure.
public class QvhLazyExpression(string expression, ScriptContext scriptContext) : IFunction<string>
{
    private IFunction<string>? _inner;

    public Task<string> ExecuteAsync(Context c)
    {
        _inner ??= new Expression<string>(expression, scriptContext);
        return _inner.ExecuteAsync(c);
    }

    public string Save() => expression;

    public IFunction<string> Clone() => new QvhLazyExpression(expression, scriptContext);
}
