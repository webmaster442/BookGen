//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Cli.Mediator;
using BookGen.GeneratorStepRunners;
using BookGen.GeneratorSteps;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;
using BookGen.Web;


namespace BookGen;

internal class GeneratorRunner
{
    private readonly ProjectLoader _projectLoader;
    private readonly bool _projectLoadSuccess;

    public const string ExitString = "Press a key to exit...";
    private readonly ToC? _toc;
    private readonly TagUtils? _tags;
    private readonly IModuleApi _moduleApi;
    private readonly IAppSetting _appSettings;
    private readonly ProgramInfo _programInfo;
    private readonly TimeProvider _timeProvider;
    private readonly IMediator _mediator;

    public FsPath ConfigFile { get; }

    public string WorkDirectory
    {
        get;
    }

    public ILogger Log { get; }
    public bool NoWait { get; internal set; }

    public GeneratorRunner(ILogger log,
                           IModuleApi moduleApi,
                           IAppSetting appSettings,
                           ProgramInfo programInfo,
                           TimeProvider timeProvider,
                           IMediator mediator,
                           string workDir)
    {
        _moduleApi = moduleApi;
        Log = log;
        _appSettings = appSettings;
        _programInfo = programInfo;
        _timeProvider = timeProvider;
        _mediator = mediator;
        _projectLoader = new ProjectLoader(workDir, log, programInfo);
        _projectLoadSuccess = _projectLoader.LoadProject();
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

    public async Task<bool> InitializeAndExecute(AsyncAction<GeneratorRunner> actionToExecute)
    {
        if (Initialize())
        {
            await actionToExecute.Invoke(this);
            return true;
        }
        else
        {
            Environment.Exit(Constants.ConfigError);
            return false;
        }
    }

    public bool Initialize()
    {
        Log.LogInformation("---------------------------------------------------------");
        Log.LogInformation("BookGen Build date: {build:yyyy.MM.dd} Starting...", _programInfo.BuildDateUtc.Date);
        Log.LogInformation("Config API version: {apiVersion}", _programInfo.ProgramVersion);
        Log.LogInformation("Working directory: {WorkDirectory}", WorkDirectory);
        Log.LogInformation("Os: {os}", Environment.OSVersion.VersionString);
        Log.LogInformation("---------------------------------------------------------");

        bool ret = _projectLoader.LoadProject();

        if (!ret && !NoWait)
            _moduleApi.Wait(ExitString);

        return ret;
    }

    public Task DoClean()
    {
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetWeb.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetPrint.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetEpub.OutPutDirectory), Log);
        CreateOutputDirectory.CleanDirectory(new FsPath(_projectLoader.Configuration.TargetWordpress.OutPutDirectory), Log);
        return Task.CompletedTask;
    }
    #endregion

    #region Argument handlers

    private void RunSteps<TBuilder>(Func<TBuilder> builderCreator, RuntimeSettings settings) where TBuilder : BookGen.Framework.GeneratorStepRunner
    {
        using (TBuilder instance = builderCreator())
        {
            TimeSpan runTime = instance.Run();
            Log.LogInformation("Runtime: {runtime} ms", runTime.TotalMilliseconds);
        }
    }

    public Task DoBuild()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWeb);

        Log.LogInformation("Building deploy configuration...");

        RunSteps(() => new WebsiteGeneratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo), settings);

        return Task.CompletedTask;
    }

    public Task DoPrint()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetPrint);

        Log.LogInformation("Building print configuration...");

        RunSteps(() => new PrintGeneratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo), settings);

        return Task.CompletedTask;
    }

    public Task DoEpub()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetEpub);

        Log.LogInformation("Building epub configuration...");

        RunSteps(() => new EpubGeneratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo), settings);

        return Task.CompletedTask;
    }

    public Task DoWordpress()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWordpress);

        Log.LogInformation("Building Wordpress configuration...");

        RunSteps(() => new WordpressGeneratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo), settings);

        return Task.CompletedTask;
    }

    public Task DoPostProcess()
    {
        ThrowIfInvalidState();

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetPostProcess);

        Log.LogInformation("Building postprocess configuration...");

        RunSteps(() => new PostProcessGenreratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo), settings);

        return Task.CompletedTask;
    }

    public async Task DoTest()
    {
        ThrowIfInvalidState();

        Log.LogInformation("Building test configuration...");

        _projectLoader.Configuration.HostName = "http://localhost:8090/";

        RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_projectLoader.Configuration.TargetWeb);

        var builder = new WebsiteGeneratorStepRunner(settings, Log, _mediator, _appSettings, _programInfo);
        TimeSpan runTime = builder.Run();

        var server = ServerFactory.CreateServerForTesting(Path.Combine(WorkDirectory, _projectLoader.Configuration.TargetWeb.OutPutDirectory));
        using (var runner = new ConsoleHttpServerRunner(server))
        {
            var serverurls = string.Join(' ', server.GetListenUrls());
            var qrcodes = string.Join(' ', server.GetListenUrls().Select(x => $"{x}/qrcodelink"));

            Log.LogInformation("-------------------------------------------------");
            Log.LogInformation("Runtime: {runtime} ms", runTime.TotalMilliseconds);
            Log.LogInformation("Server running on {urls}", serverurls);
            Log.LogInformation("To get QR code for another device visit: {qrcodes}", qrcodes);
            Log.LogInformation("Serving from: {directory}", _projectLoader.Configuration.TargetWeb.OutPutDirectory);

            if (_appSettings.AutoStartWebserver)
            {
                UrlOpener.OpenUrl(_projectLoader.Configuration.HostName);
            }

            await runner.RunServer();
        }
    }

    #endregion
}
