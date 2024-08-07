﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Bookgen.Win
{
    public sealed class ProcessBuilder
    {
        private string _workdir;
        private string _program;
        private string _arguments;

        public ProcessBuilder() 
        {
            _workdir = string.Empty;
            _program = string.Empty;
            _arguments = string.Empty;
        }

        public ProcessBuilder SetWorkDir(params string[] pathParts)
        {
            _workdir = Path.Combine(pathParts);
            return this;
        }

        public ProcessBuilder SetProgram(params string[] programPathParts)
        {
            _program = Path.Combine(programPathParts);
            return this;
        }

        public ProcessBuilder SetArguments(string[] args) 
        {
            _arguments = string.Join(" ", args.Select(arg =>
            {
                if (arg.Contains(' '))
                    return $"\"{arg}\"";
                else
                    return arg ;
            }));
            return this;
        }

        public Process Build()
        {
            var p = new Process();
            p.StartInfo.FileName = _program;
            p.StartInfo.Arguments = _arguments;
            p.StartInfo.WorkingDirectory = _workdir;
            p.StartInfo.UseShellExecute = false;
            return p;
        }
    }
}
