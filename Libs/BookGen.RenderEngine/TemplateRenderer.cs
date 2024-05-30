//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;

using BookGen.Api;
using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

namespace BookGen.RenderEngine;

public partial class TemplateRenderer : ITemplateRenderer
{
    private readonly ILog _log;
    private readonly StringBuilder _buffer;
    private readonly Dictionary<string, Function> _functions;

    [GeneratedRegex(@"\{\{([\w]+)\}\}")]
    private static partial Regex Identifier();

    [GeneratedRegex(@"\{\{([\w]+\([\w\,\=\ ]*\))}\}")]
    private static partial Regex Function();

    private readonly FunctionServices _functionServices;
    private static readonly char[] _functionSplitters = ['(', ')', ','];

    public TemplateRenderer(ILog log, TimeProvider timeProvider, IAppSetting appSetting, int bufferSize = 4096)
    {
        _log = log;
        _functionServices = new FunctionServices
        {
            Log = log,
            AppSetting = appSetting,
            TimeProvider = timeProvider
        };
        _buffer = new StringBuilder(bufferSize);
        _functions = FunctionLoader.LoadFunctions(_functionServices).ToDictionary(f => f.Information.Name, f => f);
    }

    public string Render(string template, TemplateParameters templateParameters)
    {
        _buffer.Clear();
        using (var reader = new StringReader(template))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var identifiers = Identifier().Matches(line);
                var functions = Function().Matches(line);
                if (identifiers.Count > 0)
                {
                    line = ReplaceIdentifiers(line, identifiers, templateParameters);
                }
                if (functions.Count > 0)
                {
                    line = ReplaceFunctions(line, functions);
                }
                _buffer.AppendLine(line);
            }
        }
        return _buffer.ToString();
    }

    private string ReplaceFunctions(string line, MatchCollection functions)
    {
        foreach (Match match in functions)
        {
            var (functionName, arguments) = ParseFunction(match.Groups[1].Value);
            if (_functions.TryGetValue(functionName, out Function? function))
            {
                line = line.Replace(match.Value, function.Execute(arguments));
            }
            else
            {
                _log.Warning("Unknown function: {0}", functionName);
            }
        }
        return line;
    }

    private string ReplaceIdentifiers(string line,
                                             IReadOnlyList<Match> identifiers,
                                             TemplateParameters templateParameters)
    {
        foreach (Match match in identifiers)
        {
            string parameterName = match.Groups[1].Value;
            if (templateParameters.TryGetValue(parameterName, out string? value))
            {
                line = line.Replace(match.Value, value);
            }
            else
            {
                _log.Warning("Unknown parameter: {0}", parameterName);
            }
        }
        return line;
    }

    private (string functionName, FunctionArguments arguments) ParseFunction(string functionString)
    {
        static FunctionArguments CreateArguments(string[] keyValues)
        {
            FunctionArguments result = new();
            foreach (var keyValue in keyValues)
            {
                var parts = keyValue.Split('=');
                result.Add(parts[0], parts[1]);
            }
            return result;
        }

        var parts = functionString.Split(_functionSplitters, StringSplitOptions.RemoveEmptyEntries);
        return (parts[0], CreateArguments(parts[1..]));
    }
}
