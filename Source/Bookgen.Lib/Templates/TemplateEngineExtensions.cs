using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bookgen.Lib.AppSettings;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Templates;

public static class TemplateEngineExtensions
{
    public static void RegisterScriptFunction(this TemplateEngine engine,
                                              ILogger logger,
                                              string functionName,
                                              string programNameWithoutExtension,
                                              string programPath,
                                              int timeout = 5000)
    {
        ScriptProcess scriptProcess = new(logger);
        engine.RegisterFunction(functionName, (args) => scriptProcess.ExecuteScriptProcess(programNameWithoutExtension, programPath, args[0], timeout));
    }

    public static void RegisterPythonScript(this TemplateEngine engine, ILogger logger, BookGenAppSettings settings)
    {
        engine.RegisterScriptFunction(logger, "Python", "python", settings.PythonPath);
    }

    public static void RegisterNodeScript(this TemplateEngine engine, ILogger logger, BookGenAppSettings settings)
    {
        engine.RegisterScriptFunction(logger, "Node", "node", settings.NodeJsPath);
    }
}
