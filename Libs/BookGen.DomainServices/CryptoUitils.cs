//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BookGen.DomainServices
{
    public static class CryptoUitils
    {
        public static string GetSRI(string content)
        {
            using (var hashAlgorithm = SHA384.Create())
            {

                byte[] textData = Encoding.UTF8.GetBytes(content);
                byte[] hash = hashAlgorithm.ComputeHash(textData);

                return "sha384-" + Convert.ToBase64String(hash);
            }
        }

        public static string GetSRI(FsPath path)
        {
            using (FileStream? fs = File.OpenRead(path.ToString()))
            {
                using (var hashAlgorithm = SHA384.Create())
                {
                    byte[] hash = hashAlgorithm.ComputeHash(fs);
                    return "sha384-" + Convert.ToBase64String(hash);
                }
            }
        }
    }
}
