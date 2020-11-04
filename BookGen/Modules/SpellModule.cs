//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System.Linq;
using System.Text;

namespace BookGen.Modules
{
    internal class SpellModule : StateModuleBase
    {
        private readonly AppSetting _settings;

        public SpellModule(ProgramState currentState, AppSetting settings) : base(currentState)
        {
            _settings = settings;
        }

        public override string ModuleCommand => "spell";


        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("spell",
                                            "check",
                                            "install",
                                            "uninstall",
                                            "-l",
                                            "--language");
            }
        }

        public override bool Execute(string[] arguments)
        {
            SpellArguments args = new SpellArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            var log = new ConsoleLog(LogLevel.Info);

            switch (args.Action)
            {
                case SpellActions.Install:
                    return HandleInstall(args, log);
                case SpellActions.Uninstall:
                    return HandleUninstall(args, log);
                case SpellActions.Check:
                    return HandleCheck(args, log);
                default:
                    return false;
            }
        }

        private bool HandleCheck(SpellArguments args, ILog log)
        {
            SpellChecker checker = new SpellChecker(log, _settings);
            checker.SpellCheck(new FsPath(args.Files.First()));
            return true;
        }

        private bool HandleUninstall(SpellArguments args, ILog log)
        {
            DictionaryManager manager = new DictionaryManager(log, _settings);
            manager.UninstallDictionary(args.LanguageCode);
            return true;
        }

        private bool HandleInstall(SpellArguments args, ILog log)
        {
            DictionaryManager manager = new DictionaryManager(log, _settings);
            manager.InstallDictionary(args.LanguageCode);
            return true;
        }

        public override string GetHelp()
        {
            StringBuilder result = new StringBuilder(4096);
            result.Append(HelpUtils.GetHelpForModule(nameof(SpellModule)));
            return result.ToString();
        }
    }
}
