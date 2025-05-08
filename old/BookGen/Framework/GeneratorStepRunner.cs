//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli.Mediator;
using BookGen.Gui;
using BookGen.RenderEngine;

namespace BookGen.Framework;

internal abstract class GeneratorStepRunner : IDisposable
{
    private readonly List<IGeneratorStep> _steps;
    private readonly StaticTemplateContent _staticContent;
    private readonly List<string> _redirectedLogMessages;

    protected readonly ILogger _log;
    private readonly IMediator _mediator;
    private readonly ProgramInfo _programInfo;

    protected RuntimeSettings Settings { get; }

    private readonly FunctionServices _functionServices;

    protected GeneratorStepRunner(RuntimeSettings settings,
                                  ILogger log,
                                  IMediator mediator,
                                  IAppSetting appSetting,
                                  ProgramInfo programInfo)
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
        _mediator = mediator;
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
        ConsoleProgressbar progressbar = new(_mediator);
        sw.Start();
        string stepName = string.Empty;
        try
        {
            progressbar.Show(useAlternateBuffer: true);
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

                double percent = (double)stepCounter / _steps.Count;
                progressbar.Report(percent);
                step.RunStep(Settings, _log);
                ++stepCounter;
            }
            progressbar.Hide();
            _redirectedLogMessages.Clear();
        }
        catch (Exception ex)
        {
            progressbar.Hide();
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
