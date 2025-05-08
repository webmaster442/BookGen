//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace BookGen.DomainServices.Markdown.Scripting;
internal sealed class CsharpScriptExecutor
{
    private readonly ScriptOptions _options;

    public CsharpScriptExecutor()
    {
        _options = ScriptOptions.Default
            .WithCheckOverflow(true)
            .WithFileEncoding(Encoding.UTF8)
            .WithLanguageVersion(Microsoft.CodeAnalysis.CSharp.LanguageVersion.Latest)
            .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Release)
            .WithImports(GetImports())
            .WithReferences(GetReferences());
    }

    private static HashSet<Assembly> GetReferences()
    {
        HashSet<Assembly> references =
        [
            typeof(Console).Assembly,
            typeof(object).Assembly,
            typeof(List<>).Assembly,
            typeof(Enumerable).Assembly,
            typeof(Task).Assembly,
            typeof(File).Assembly,
            typeof(Regex).Assembly
        ];
        return references;
    }

    private static IEnumerable<string> GetImports()
    {
        yield return "System";
        yield return "System.Collections.Generic";
        yield return "System.Linq";
        yield return "System.Text";
        yield return "System.Threading.Tasks";
        yield return "System.IO";
        yield return "System.Text.RegularExpressions";
    }

    public async Task<string> Execute(string code)
    {
        var originalConsoleOut = Console.Out;
        var originalConsoleIn = Console.In;
        using (var writer = new StringWriter())
        {
            Console.SetOut(writer);
            Console.SetIn(new NotSupportedReader());
            try
            {
                await CSharpScript.RunAsync(code, _options);
            }
            catch (Exception ex)
            {
                writer.WriteLine(ex.Message);
            }
            finally
            {
                Console.SetIn(originalConsoleIn);
                Console.SetOut(originalConsoleOut);
            }
            return writer.ToString();
        }
    }
}
