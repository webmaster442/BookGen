//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;

using Bookgen.Win.Properties;

namespace Bookgen.Win
{
    public static class InstallVerify
    {
        public static void ThrowIfNotExist()
        {
            var files = new[]
            {
                Path.Combine(AppContext.BaseDirectory, Constants.DataFolder, Constants.BookGenLauncher),
                Path.Combine(AppContext.BaseDirectory, Constants.DataFolder, Constants.BookGen)
            };
            foreach (var file in files) 
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException(Resources.ErrorProgramNotFound);
                }
            }
        }

    }
}