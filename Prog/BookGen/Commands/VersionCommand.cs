﻿//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("version")]
internal class VersionCommand : Command<VersionArguments>
{
    private readonly ProgramInfo _programInfo;

    public VersionCommand(ProgramInfo programInfo)
    {
        _programInfo = programInfo;
    }

    public override int Execute(VersionArguments arguments, string[] context)
    {
        if (arguments.IsDefault)
        {
            Console.WriteLine("BookGen Build date (UTC): {0:yyyy.MM.dd}", _programInfo.BuildDateUtc.Date);
            Console.WriteLine("Build timestamp (UTC): {0:HH:mm:ss}", _programInfo.BuildDateUtc);
            Console.WriteLine("Config API version: {0}", _programInfo.ProgramVersion);
        }
        if (arguments.BuildDate)
        {
            Console.WriteLine("{0:yyyy.MM.dd}", _programInfo.BuildDateUtc.Date);
        }
        if (arguments.ApiVersion)
        {
            Console.WriteLine("{0}", _programInfo.ProgramVersion);
        }

        return Constants.Succes;
    }
}
