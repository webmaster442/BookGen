﻿//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Framework;
using System;
using System.Reflection;
using Webmaster442.HttpServerFramework;

namespace BookGen
{
    internal class ProgramState
    {
        public bool Gui { get; set; }
        public bool NoWaitForExit { get; set; }
        public Version ProgramVersion { get; }
        public DateTime BuildDate { get; }
        public string ProgramDirectory { get; }
        public int ConfigVersion { get; }
        public IMoudleApi Api { get; }

#if TESTBUILD
        public ILog Log { get; set; }
        public IServerLog ServerLog { get; set; }
#else

        public ILog Log { get; }
        public IServerLog ServerLog { get; }
#endif

        private static DateTime GetProgramDate()
        {
            Assembly? current = Assembly.GetAssembly(typeof(ProgramState));
            if (current != null)
            {
                var attribute = current.GetCustomAttribute<AssemblyBuildDateAttribute>();
                if (attribute != null)
                {
                    return attribute.BuildDate;
                }
            }
            return new DateTime();
        }

        public ProgramState(IMoudleApi apiImplementation, ILog log, IServerLog serverLog)
        {
            Log = log;
            ServerLog = serverLog;
            Api = apiImplementation;
            var asm = Assembly.GetAssembly(typeof(ProgramState));
            ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
            ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;
            BuildDate = GetProgramDate();
            ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
        }

    }
}
