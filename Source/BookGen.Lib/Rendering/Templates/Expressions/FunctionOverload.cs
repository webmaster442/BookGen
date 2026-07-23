//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Lib.Rendering.Templates.Expressions;

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
