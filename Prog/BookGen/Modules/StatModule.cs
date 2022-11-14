﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Configuration;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.ProjectHandling;
using System.IO;

namespace BookGen.Modules
{
    internal class StatModule : ModuleWithState
    {
        public StatModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Stat";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand, "-d", "--dir", "-i", "--input");

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new StatArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            var stat = new StatisticsData();

            if (!string.IsNullOrEmpty(args.Input))
            {
                if (TryComputeStat(args.Input, ref stat))
                {
                    CurrentState.Log.PrintLine("");
                    CurrentState.Log.PrintLine(stat);
                    return ModuleRunResult.Succes;
                }
                return ModuleRunResult.GeneralError;
            }
            else
            {
                CheckLockFileExistsAndExitWhenNeeded(args.Directory);

                using (var l = new FolderLock(args.Directory))
                {
                    var loader = new ProjectLoader(args.Directory, CurrentState.Log);
                    bool result = loader.LoadProject();

                    if (result)
                    {
                        RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());
                        foreach (string? link in settings.TocContents.Files)
                        {
                            if (!TryComputeStat(link, ref stat))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    
                    if (result)
                    {
                        CurrentState.Log.PrintLine("");
                        CurrentState.Log.PrintLine(stat);
                    }

                    return result ? ModuleRunResult.Succes : ModuleRunResult.GeneralError;
                }
            }

        }

        private bool TryComputeStat(string input, ref StatisticsData stat)
        {
            try
            {
                string? line = null;
                using (StreamReader? reader = File.OpenText(input))
                {
                    stat.Bytes += reader.BaseStream.Length;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            stat.Chars += line.Length;
                            ++stat.ParagraphLines;
                            stat.Words += line.GetWordCount();
                            stat.PageCountLines += ComputeParagraphLine(line.Length);
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

        private static long ComputeParagraphLine(int length)
        {
            if (length < 80)
                return 1;
            return length / 80;
        }
    }
}
