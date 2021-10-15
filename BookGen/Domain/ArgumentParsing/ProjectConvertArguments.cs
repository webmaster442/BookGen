﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;
using System;

namespace BookGen.Domain.ArgumentParsing
{
    internal class ProjectConvertArguments : ArgumentsBase
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