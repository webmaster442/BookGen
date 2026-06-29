using System.Reflection;

namespace Bookgen.Experiments.Expressions;

/// <summary>
/// A registered function delegate together with parameter metadata that is
/// computed once at registration time so the expression parser never has to
/// touch reflection on the hot path.
/// </summary>
internal sealed class FunctionOverload
{
    public Delegate Function { get; }

    public Type[] ParameterTypes { get; }

    public bool IsParamsArray { get; }

    public FunctionOverload(Delegate function)
    {
        Function = function;

        ParameterInfo[] parameters = function.Method.GetParameters();
        ParameterTypes = new Type[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterTypes[i] = parameters[i].ParameterType;
        }

        IsParamsArray = ParameterTypes.Length == 1 && ParameterTypes[0] == typeof(object[]);
    }
}
