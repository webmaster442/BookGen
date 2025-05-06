using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.VFS;
using Bookgen.Lib.Internals;
using System.Security.Cryptography.X509Certificates;

namespace Bookgen.Lib;

internal static class Constants
{
    public const string ConfigFile = "bookgen.json";
}

public sealed class BookGenEnvironment
{
    private readonly IFolder _folder;

    public BookGenEnvironment(IFolder soruceFolder)
    {
        _folder = soruceFolder;
    }

    public async Task<EnvironmentStatus> Initialize()
    {
        EnvironmentStatus status = new EnvironmentStatus();

        if (!_folder.Exists(Constants.ConfigFile))
        {
            status.AddIssue($"No {Constants.ConfigFile} found in folder {_folder.FullPath}");
            return status;
        }

        Config? config = await _folder.DeserializeAsync<Config>(Constants.ConfigFile);

        if ( config== null)
        {
            status.AddIssue("Config file load failed");
            return status;
        }

        Config defaultConfig = Config.GetDefault();

        if (config.VersionTag < defaultConfig.VersionTag)
        {
            await _folder.SerializeAsync(Constants.ConfigFile, config);
            status.AddIssue($"Config from version {config.VersionTag} was updated to {defaultConfig.VersionTag}. Check settings and re-execute");
            return status;
        }



        return status;
    }

}
