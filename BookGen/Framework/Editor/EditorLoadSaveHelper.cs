//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace BookGen.Framework.Editor
{
    public static class EditorLoadSaveHelper
    {
        private static string GetDiskFile(string folder, string base64encodedurl)
        {
            byte[] urlBytes = Convert.FromBase64String(base64encodedurl);
            string url = Encoding.UTF8.GetString(urlBytes);

            if (url.StartsWith("\\")) url = url.Substring(1);

            return Path.Combine(folder, url);
        }

        public static string LoadFile(string folder, string base64encodedurl, Api.ILog log)
        {
            try
            {
                string diskfile = GetDiskFile(folder, base64encodedurl);

                if (File.Exists(diskfile))
                {
                    return File.ReadAllText(diskfile);
                }
                else
                    return string.Empty;
            }
            catch (FormatException ex)
            {
                log.Warning(ex);
                return string.Empty;
            }

        }

        public static bool SaveFile(string folder, string base64encodedurl, string base64content, Api.ILog log)
        {
            try
            {
                byte[] contentBytes = Convert.FromBase64String(base64content);
                string rawContent = Encoding.UTF8.GetString(contentBytes);

                string diskfile = GetDiskFile(folder, base64encodedurl);

                using (var file = File.CreateText(diskfile))
                {
                    file.Write(rawContent);
                }

                return true;

            }
            catch (IOException ex)
            {
                log.Warning(ex);
                return false;
            }
        }
    }
}
