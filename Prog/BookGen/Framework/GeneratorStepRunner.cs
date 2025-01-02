//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Gui;
using BookGen.RenderEngine;

namespace BookGen.Framework;

internal abstract class GeneratorStepRunner : IDisposable
{
    private readonly List<IGeneratorStep> _steps;
    private readonly StaticTemplateContent _staticContent;
    private readonly List<string> _redirectedLogMessages;

    protected readonly ILogger _log;
    private readonly ProgramInfo _programInfo;

    protected RuntimeSettings Settings { get; }

    private readonly FunctionServices _functionServices;

    protected GeneratorStepRunner(RuntimeSettings settings, ILogger log, IAppSetting appSetting, ProgramInfo programInfo)
    {
        Settings = settings;
        _redirectedLogMessages = [];
        _staticContent = new StaticTemplateContent();

        _functionServices = new FunctionServices
        {
            Log = log,
            RuntimeSettings = Settings,
            TimeProvider = TimeProvider.System,
            AppSetting = appSetting,
        };

        _steps = [];
        _log = log;
        _programInfo = programInfo;
    }

    private TemplateProcessor CreateTemplateProcessor()
        => new(_functionServices, _staticContent);

    protected abstract string ConfigureTemplateContent();

    protected abstract FsPath ConfigureOutputDirectory(FsPath workingDirectory);

    public void AddStep(IGeneratorStep step)
    {
        _steps.Add(step);
    }

    public TimeSpan Run()
    {
        Settings.OutputDirectory = ConfigureOutputDirectory(Settings.SourceDirectory);
        var sw = new Stopwatch();
        ConsoleProgressbar progressbar = new(0, _steps.Count, !_programInfo.JsonLogging);
        sw.Start();
        string stepName = string.Empty;
        try
        {
            progressbar.SwitchBuffers();
            int stepCounter = 1;
            foreach (IGeneratorStep? step in _steps)
            {
                stepName = step.GetType().Name;
                switch (step)
                {
                    case ITemplatedStep templated:
                        TemplateProcessor? instance = CreateTemplateProcessor();
                        templated.Content = instance;
                        templated.Template = instance;
                        templated.Template.TemplateContent = ConfigureTemplateContent();
                        break;
                    case IGeneratorContentFillStep contentFill:
                        contentFill.Content = _staticContent;
                        break;
                }

                progressbar.Report(stepCounter, "Step {0} of {1}", stepCounter, _steps.Count);
                step.RunStep(Settings, _log);
                ++stepCounter;
            }
            progressbar.SwitchBuffers();
            progressbar.Report(_redirectedLogMessages);
            _redirectedLogMessages.Clear();
        }
        catch (Exception ex)
        {
            progressbar.SwitchBuffers();
            _log.LogCritical(ex, "Critical exception while running: {stepName}", stepName);
#if DEBUG
            Debugger.Break();
#endif
        }
        finally
        {
            sw.Stop();
        }
        return sw.Elapsed;
    }

    protected virtual void Dispose(bool disposing)
    {
        //empty
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
