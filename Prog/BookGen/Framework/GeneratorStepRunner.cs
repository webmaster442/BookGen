//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Gui;

namespace BookGen.Framework;

internal abstract class GeneratorStepRunner : IDisposable
{
    private readonly List<IGeneratorStep> _steps;
    private readonly StaticTemplateContent _staticContent;
    private readonly List<string> _redirectedLogMessages;
    private bool _logListen;

    protected readonly ILog _log;

    protected RuntimeSettings Settings { get; }

    protected readonly ShortCodeLoader _loader;
    protected GeneratorStepRunner(RuntimeSettings settings,
                      ILog log,
                      ShortCodeLoader shortCodeLoader)
    {
        Settings = settings;
        _redirectedLogMessages = new List<string>();
        _staticContent = new StaticTemplateContent();
        _loader = shortCodeLoader;
        _loader.LoadAll();

        _steps = new List<IGeneratorStep>();
        _log = log;
        _log.OnLogWritten += OnLogWritten;
    }

    private void OnLogWritten(object? sender, LogEventArgs e)
    {
        if (!_logListen)
            return;

        switch (e.LogLevel)
        {
            case LogLevel.Warning:
            case LogLevel.Critical:
                _redirectedLogMessages.Add(e.Message);
                break;
        }
    }

    private TemplateProcessor CreateTemplateProcessor()
    {
        return new TemplateProcessor(Settings.Configuration,
                                     new ShortCodeParser(_loader.Imports,
                                                         Settings.Configuration.Translations,
                                                         _log),
                                     _staticContent);
    }

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
        ConsoleProgressbar progressbar = new(0, _steps.Count, _log is not JsonLog);
        sw.Start();
        string stepName = string.Empty;
        try
        {
            progressbar.SwitchBuffers();
            _logListen = _log is not JsonLog;
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
            _logListen = false;
            progressbar.Report(_redirectedLogMessages);
            _redirectedLogMessages.Clear();
        }
        catch (Exception ex)
        {
            _logListen = false;
            progressbar.SwitchBuffers();
            _log.Critical("Critical exception while running: {0}", stepName);
            _log.Critical(ex);
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
        _log.OnLogWritten -= OnLogWritten;
        _logListen = false;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
