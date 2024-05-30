using BookGen.RenderEngine.Internals;

namespace BookGen.RenderEngine;

internal static class FunctionLoader
{
    public static IEnumerable<Function> LoadFunctions(FunctionServices functionServices)
    {
        return typeof(Function).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Function)))
            .Select(t => CreateFunctionFrom(t, functionServices));
    }

    private static Function CreateFunctionFrom(Type t, FunctionServices functionServices)
    {
        var instance = Activator.CreateInstance(t);
        if (instance is Function function)
        {
            if (function is IInjectable injectable)
            {
                injectable.Inject(functionServices);
            }
            return function;
        }
        throw new InvalidOperationException($"Function creation failed: {t.Name}");
    }
}
