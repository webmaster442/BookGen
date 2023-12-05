//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public static class Constants
    {
        /// <summary>
        /// Succcesfull exit code = 0
        /// </summary>
        public const int Succes = 0;
        /// <summary>
        /// Aguments error exit code = 1
        /// </summary>
        public const int ArgumentsError = 1;
        /// <summary>
        /// Config error exit code = 2
        /// </summary>
        public const int ConfigError = 2;
        /// <summary>
        /// Folder lock exit code = 3
        /// </summary>
        public const int FolderLocked = 3;
        /// <summary>
        /// General error
        /// </summary>
        public const int GeneralError = int.MaxValue;

        public const int FiveSeconds = 5000;

        public const string WikiUrl = "https://github.com/webmaster442/BookGen/wiki";

        public const string ConfigJson = "config.json";
        public const string ConfigYml = "config.yml";

        public const string VsCodePath = @"%localappdata%\Programs\Microsoft VS Code\Code.exe";
        public const string NotepadPath = @"%windir%\notepad.exe";

        public const string ProgramName = "BookGen";

        //A4 page 12 font size, default margins
        public const long LinesPerPage = 52;
    }
}
