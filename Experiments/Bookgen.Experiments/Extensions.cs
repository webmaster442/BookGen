namespace Bookgen.Experiments;

public static class Extensions
{
    public static void RegisterBuiltinFunctions<T>(this TemplateEngine<T> engine, TimeProvider? timeProvider = null)
    {
        TimeProvider provider= timeProvider ?? TimeProvider.System;

        var builtinFunctions = new BuiltinFunctions(provider);

        engine.RegisterFunction(nameof(builtinFunctions.ToUpper), builtinFunctions.ToUpper);
        engine.RegisterFunction(nameof(builtinFunctions.ToLower), builtinFunctions.ToLower);
        engine.RegisterFunction(nameof(builtinFunctions.Substring), builtinFunctions.Substring);
        engine.RegisterFunction(nameof(builtinFunctions.Trim), builtinFunctions.Trim);
        engine.RegisterFunction(nameof(builtinFunctions.TrimStart), builtinFunctions.TrimStart);
        engine.RegisterFunction(nameof(builtinFunctions.TrimEnd), builtinFunctions.TrimEnd);
        engine.RegisterFunction(nameof(builtinFunctions.Replace), builtinFunctions.Replace);
        engine.RegisterFunction(nameof(builtinFunctions.Concat), builtinFunctions.Concat);
        engine.RegisterFunction(nameof(builtinFunctions.RegexReplace), builtinFunctions.RegexReplace);
        engine.RegisterFunction(nameof(builtinFunctions.HtmlEncode), builtinFunctions.HtmlEncode);
        engine.RegisterFunction(nameof(builtinFunctions.UrlEncode), builtinFunctions.UrlEncode);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentDate), builtinFunctions.CurrentDate);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentDateFormat), builtinFunctions.CurrentDateFormat);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentDateTime), builtinFunctions.CurrentDateTime);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentDateTimeFormat), builtinFunctions.CurrentDateTimeFormat);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentTime), builtinFunctions.CurrentTime);
        engine.RegisterFunction(nameof(builtinFunctions.CurrentTimeFormat), builtinFunctions.CurrentTimeFormat);
        engine.RegisterFunction(nameof(builtinFunctions.UrlDecode), builtinFunctions.UrlDecode);
    }
}
