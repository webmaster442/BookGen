//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;
using System.IO;

namespace BookGen.Modules
{
    internal class StatModule : ModuleWithState
    {
        protected StatModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Stat";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand, "-d", "--dir", "-i", "--input");

        public override bool Execute(string[] arguments)
        {
            var args = new StatParameters();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            var stat = new StatisticsData();

            if (!string.IsNullOrEmpty(args.Input))
            {
                if (TryComputeStat(args.Input, ref stat))
                {
                    stat.Pages = (double)stat.Words / StatisticsData.CharsPerA4Page;
                    CurrentState.Log.PrintLine(stat);
                    return true;
                }
                return false;
            }
            else
            {
                FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

                using (var l = new FolderLock(args.Directory))
                {
                    ProjectLoader loader = new ProjectLoader(CurrentState.Log, args.Directory);
                    bool result = loader.TryLoadProjectAndExecuteOperation((config, toc) =>
                    {
                        var settings = loader.CreateRuntimeSettings(config, toc, new BuildConfig());

                        foreach (var link in settings.TocContents.Files)
                        {
                            if (!TryComputeStat(link, ref stat))
                            {
                                return false;
                            }
                        }
                        return true;
                    });

                    if (result)
                    {
                        CurrentState.Log.PrintLine(stat);
                    }

                    return result;
                }
            }

        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(StatModule));
        }

        private bool TryComputeStat(string input, ref StatisticsData stat)
        {
            try
            {
                string? line = null;
                using (var reader = File.OpenText(input))
                {
                    stat.Bytes = reader.BaseStream.Length;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            stat.Chars += line.Length;
                            ++stat.Lines;
                            stat.Words += line.GetWordCount();
                        }
                    }
                    while (line != null);

                    return true;
                }
            }
            catch (Exception ex)
            {
                CurrentState.Log.Warning("ReadFile failed: {0}", input);
                CurrentState.Log.Detail(ex.Message);
                return false;
            }
        }
    }
}
