//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    internal static class Constants
    {
        public const string WikiUrl = "https://github.com/webmaster442/BookGen/wiki";

        public const string ConfigJson = "config.json";
        public const string ConfigYml = "config.yml";

        public const string PexelSearchUrl = "https://www.pexels.com/search/{0}/";
        public const string UnsplashSearchUrl = "https://unsplash.com/s/photos/{0}";
        public const string PixabaySearchUrl = "https://pixabay.com/images/search/{0}/";

        public const string VsCodePath = @"%localappdata%\Programs\Microsoft VS Code\Code.exe";
        public const string NotepadPath = @"%windir%\notepad.exe";

        public const string ProgramName = "BookGen";

        //A4 page 12 font size, default margins
        public const long LinesPerPage = 52;
    }
}
