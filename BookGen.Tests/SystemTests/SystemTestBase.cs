//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace BookGen.Tests.SystemTests
{
    public abstract class SystemTestBase
    {
        public string Workdir { get; private set; }
        public string BuildDir { get; private set; }
        public Config Configuration { get; private set; }

        [SetUp]
        public void Setup()
        {
            Workdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemTests");
            BuildDir = Path.Combine(Workdir, "Builds");

            CleanDirectory(Workdir);

            CreateDirectory(Workdir);
            CreateDirectory(BuildDir);
            Configuration = Config.CreateDefault(Program.CurrentState.ConfigVersion);
            Configuration.TOCFile = "Summary.md";
            Configuration.ImageDir = "Img";
            Configuration.Index = "Index.md";
            Configuration.LinksOutSideOfHostOpenNewTab = true;
            Configuration.HostName = "localhost/";
            Configuration.TargetWeb.OutPutDirectory = BuildDir;
            Configuration.TargetPrint.OutPutDirectory = BuildDir;
            Configuration.TargetEpub.OutPutDirectory = BuildDir;
            Configuration.TargetWordpress.OutPutDirectory = BuildDir;
            Configuration.TargetWordpress.TemplateOptions["WordpressTargetHost"] = "localhost/";
        }

        [TearDown]
        public void Teardown()
        {
            CleanDirectory(BuildDir);
            CleanDirectory(Workdir);
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CleanDirectory(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);

            if (!di.Exists) return;

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public void CreateConfigFile()
        {
            var text = JsonSerializer.Serialize(Configuration, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(Path.Combine(Workdir, "bookgen.json"), text);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();      
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }

        public void CopyDemoProject()
        {
            DirectoryCopy(Environment.TestEnvironment.GetSystemTestContentFolder(), Workdir);
        }

        public void RunProgramAndAssertSuccess(params string[] arguments)
        {
            var output = RunProgram(arguments);

            if (Program.ErrorHappened)
            {
                string error = string.Join('\n', Program.ErrorText, "Log:", output);
                Assert.Fail(error);
            }
        }

        public string RunProgram(params string[] arguments)
        {
            var log = new SystemTestLog();

            Program.IsTesting = true;
            Program.CurrentState.Log = log;
            Program.ErrorHappened = false;
            Program.Main(arguments);

            return log.ToString();
        }
    }
}
