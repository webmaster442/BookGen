//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BookGen.Core
{
    public static class SriGenerator
    {
        public static string GetSRI(string content)
        {
            using (var hashAlgorithm = new SHA384Managed())
            {

                byte[] textData = Encoding.UTF8.GetBytes(content);
                byte[] hash = hashAlgorithm.ComputeHash(textData);

                return "sha384-" + Convert.ToBase64String(hash);
            }
        }

        public static string GetSRI(FsPath path)
        {
            using (var fs = File.OpenRead(path.ToString()))
            {
                using (var hashAlgorithm = new SHA384Managed())
                {
                    byte[] hash = hashAlgorithm.ComputeHash(fs);
                    return "sha384-" + Convert.ToBase64String(hash);
                }
            }
        }
    }
}
