//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using System;
using System.Text.Json.Serialization;

namespace BookGen.Domain
{
    public class AppSetting : IAppSetting
    {
        public int NodeJsTimeout { get; set; }
        public string NodeJsPath { get; set; }
        public bool AutoStartWebserver { get; set; }
        public string PhpPath { get; set; }
        public int PhpTimeout { get; set; }
        public string PythonPath { get; set; }
        public int PythonTimeout { get; set; }
        public string SpellLanguage { get; set; }
        
        [JsonIgnore]
        public string AppDataPath { get; private set; }

        public AppSetting()
        {
            NodeJsTimeout = 60;
            PhpTimeout = 60;
            PythonTimeout = 60;
            NodeJsPath = string.Empty;
            PythonPath = string.Empty;
            PhpPath = string.Empty;
            SpellLanguage = string.Empty;
            AutoStartWebserver = true;
            AppDataPath = new FsPath(Environment.SpecialFolder.ApplicationData).Combine("\\BookGen").ToString();
        }
    }
}
