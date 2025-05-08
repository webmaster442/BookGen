//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RenderEngine.Internals;

internal static class FunctionArgumentsFactory
{
    public static FunctionArguments Create(string arguments)
    {
        var result = new FunctionArguments();
        var parts = arguments.Split(',');

        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2)
            {
                result.Add(keyValue[0], keyValue[1]);
            }
        }
        return result;
    }
}
