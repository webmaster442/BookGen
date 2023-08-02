//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Framework.Server;
using BookGen.GeneratorStepRunners;
using BookGen.GeneratorSteps;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

using Webmaster442.HttpServerFramework;

namespace BookGen;

internal class GeneratorRunner
{
    private readonly CsharpScriptHandler _scriptHandler;
    private readonly ProjectLoader _projectLoader;
    private readonly bool _projectLoadSuccess;

    public const string ExitString = "Press a key to exit...";
    private readonly ToC? _toc;
    private readonly TagUtils? _tags;
    private readonly IModuleApi _moduleApi;
    private readonly IAppSetting _appSettings;
    private readonly ProgramInfo _programInfo;

    public FsPath ConfigFile { get; private set; }

    public string WorkDirectory
    {
        get;
    }

    public ILog Log { get; }
    public IServerLog ServerLog { get; }
    public bool NoWait { get; internal set; }

    public bool IsBookGenFolder => _projectLoader.IsBookGenFolder;

    public GeneratorRunner(ILog log,
                           IServerLog serverLog,
                           IModuleApi moduleApi,
                           IAppSetting appSettings,
                           ProgramInfo programInfo,
                           string workDir)
    {
        ServerLog = serverLog;
        _moduleApi = moduleApi;
        Log = log;
        _appSettings = appSettings;
        _programInfo = programInfo;
        _projectLoader = new ProjectLoader(workDir, log, programInfo);
        _projectLoadSuccess = _projectLoader.LoadProject();
        _scriptHandler = new CsharpScriptHandler(Log);
        WorkDirectory = workDir;
        ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
        _toc = new ToC();
        _tags = new TagUtils();
    }

    #region Helpers

    [MemberNotNull(nameof(_toc), nameof(_tags))]
    private void ThrowIfInvalidState()
    {
        if (!_projectLoadSuccess)
            throw new InvalidOperationException("Project load failed");

        if (_toc == null)
            throw new InvalidOperationException("Table of contents is null");

        if (_tags == null)
            throw new InvalidOperationException("Tags is null");

    }

    public bool InitializeAndExecute(Action<GeneratorRunner> actionToExecute)
    {
        if (Initialize())
        {
            actionToExecute.Invoke(this);
            return true;
        }
        else
        {
            Log.Flush();
            Environment.Exit((int)ExitCode.BadConfiguration);
            return false;
        }
    }

    public bool Initialize(bool compileScripts = true)
    {
        Log.Info("---------------------------------------------------------");
        Log.Info("BookGen Build date: {0:yyyy.MM.dd} Starting...", _programInfo.BuildDateUtc.Date);
        Log.Info("Config API version: {0}", _programInfo.ProgramVersion);
        Log.Info("Working directory: {0}", WorkDirectory);
        Log.Info("Os: {0}", Environment.OSVersion.VersionString);
        Log.Info("---------------------------------------------------------");


        bool ret = _projectLoader.LoadProject();

        if (compileScripts)
            LoadAndCompileScripts();

        if (!ret && !NoWait)
            _moduleApi.Wait(ExitString);

        return ret;
    }

    private void LoadAndCompileScripts()
    {
        if (string.IsNullOrEmpty(_projectLoader.Configuration.ScriptsDirectory)) return;

        Log.Info("Trying to load and compile script files...");
        FsPath scripts = new FsPath(WorkDirectory).Combine(_projectLoader.Configuration.ScriptsDirectory);

        int count = _scriptHandler.LoadScripts(scripts);
        Log.Info("Loaded {0} instances from script files", count);
    }

    public void DoClean()
    {
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetWeb.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetPrint.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetEpub.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetWordpress.OutPutDirectory), Log);
    }
    #endregion

    #region Argument handlers

    private void RunSteps<TBuilder>(Func<ShortCodeLoader, TBuilder> builderCreator, RuntimeSettings settings) where TBuilder : BookGen.Framework.GeneratorStepRunner
    {
        using (var loader = new ShortCodeLoader(Log, settings, _appSettings))
        {
            using (TBuilder instance = builderCreator(loader))
            {
                TimeSpan runTime = instance.Run();
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
            }
        }
    }

    public void DoBuild()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWeb);

        Log.Info("Building deploy configuration...");

        RunSteps((loader) => new WebsiteGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
    }

    public void DoPrint()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetPrint);

        Log.Info("Building print configuration...");

        RunSteps((loader) => new PrintGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
    }

    public void DoEpub()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetEpub);

        Log.Info("Building epub configuration...");

        RunSteps((loader) => new EpubGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
    }

    public void DoWordpress()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWordpress);

        Log.Info("Building Wordpress configuration...");

        RunSteps((loader) => new WordpressGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
    }

    public void DoPostProcess()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetPostProcess);

        Log.Info("Building postprocess configuration...");

        RunSteps((loader) => new PostProcessGenreratorStepRunner(settings, Log, loader, _scriptHandler), settings);
    }

    public void DoTest()
    {
        ThrowIfInvalidState();

        Log.Info("Building test configuration...");

        _projectLoader.Configuration.HostName = "http://localhost:8090/";

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWeb);


        using (var loader = new ShortCodeLoader(Log, settings, _appSettings))
        {
            var builder = new WebsiteGeneratorStepRunner(settings, Log, loader, _scriptHandler);
            TimeSpan runTime = builder.Run();

            using (HttpServer? server = HttpServerFactory.CreateServerForTest(ServerLog, Path.Combine(WorkDirectory, _projectLoader.Configuration.TargetWeb.OutPutDirectory)))
            {
                server.Start();
                Log.Info("-------------------------------------------------");
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                Log.Info("Test server running on: http://localhost:8090/");
                Log.Info("To get QR code for another device visit: http://localhost:8090/qrcodelink");
                Log.Info("Serving from: {0}", _projectLoader.Configuration.TargetWeb.OutPutDirectory);

                if (_appSettings.AutoStartWebserver)
                {
                    UrlOpener.OpenUrl(_projectLoader.Configuration.HostName);
                }

                Console.WriteLine(ExitString);
                Console.ReadLine();
                server.Stop();
            }
        }
    }

    #endregion
}
