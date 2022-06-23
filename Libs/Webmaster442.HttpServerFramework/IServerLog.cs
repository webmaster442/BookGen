// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace Webmaster442.HttpServerFramework;

/// <summary>
/// Logging interface for the server
/// </summary>
public interface IServerLog
{
    /// <summary>
    /// Log a critical exception.
    /// </summary>
    /// <param name="ex">Exception to log</param>
    void Critical(Exception ex)
    {
        Debug.WriteLine("Critical:");
        Debug.WriteLine(ex);
        Debug.WriteLine(ex.StackTrace);
    }

    /// <summary>
    /// Log an iformational message.
    /// Informations give the user feedback about what is happening.
    /// </summary>
    /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
    /// <param name="args">Arguments for formatting</param>
    void Info(string format, params object[] args)
    {
        Debug.WriteLine("Info:");
        Debug.WriteLine(format, args);
    }

    /// <summary>
    /// Log a warning message
    /// </summary>
    /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
    /// <param name="args">Arguments for formatting</param>
    void Warning(string format, params object[] args)
    {
        Debug.WriteLine("Warning:");
        Debug.WriteLine(format, args);
    }
}
