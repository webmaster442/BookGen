﻿//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------



namespace BookGen.Domain.ArgumentParsing
{
    public sealed class ProjectConvertArguments : ArgumentsBase
    {
        [Switch("b", "backup")]
        public bool Backup { get; set; }

        [Switch("d", "dir")]
        public string Directory { get; set; }

        public ProjectConvertArguments()
        {
            Directory = Environment.CurrentDirectory;
        }
    }
}
