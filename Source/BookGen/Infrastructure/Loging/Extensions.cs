//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Loging;

internal static class Extensions
{
    public static void EnvironmentStatus(this ILogger logger, EnvironmentStatus status)
    {
        foreach (var issue in status)
        {
            logger.LogError("Init error: {issue}", issue);
        }
    }
}
