//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using BookGen.Interfaces;

namespace BookGen.DomainServices
{
    public static class CryptoUitils
    {
        public static string GetSRI(string content)
        {
            using var hashAlgorithm = SHA384.Create();

            byte[] textData = Encoding.UTF8.GetBytes(content);
            byte[] hash = SHA384.HashData(textData);

            return "sha384-" + Convert.ToBase64String(hash);
        }

        public static string GetSRI(FsPath path)
        {
            using FileStream? fs = File.OpenRead(path.ToString());
            using var hashAlgorithm = SHA384.Create();
            byte[] hash = hashAlgorithm.ComputeHash(fs);
            return "sha384-" + Convert.ToBase64String(hash);
        }

        public enum UUIDVersion : int
        {
            Version3 = 3,
            Version5 = 5,
            Version6 = 6,
        }

        public static Guid CreateUUID(Guid @namespace, string name, UUIDVersion version)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            byte[] namespaceBytes = @namespace.ToByteArray();
            SwapByteOrder(namespaceBytes);

            byte[] hash;

            using (var incrementalHash = CreateHash(version))
            {
                incrementalHash.AppendData(namespaceBytes);
                incrementalHash.AppendData(nameBytes);
                hash = incrementalHash.GetHashAndReset();
            }

            byte[] result = new byte[16];
            Array.Copy(hash, 0, result, 0, 16);

            result[6] = (byte)((result[6] & 0x0F) | ( (int)version << 4));
            result[8] = (byte)((result[8] & 0x3F) | 0x80);

            SwapByteOrder(result);

            return new Guid(result);
        }

        private static IncrementalHash CreateHash(UUIDVersion version)
        {
            return version switch
            {
                UUIDVersion.Version3 => IncrementalHash.CreateHash(HashAlgorithmName.MD5),
                UUIDVersion.Version5 => IncrementalHash.CreateHash(HashAlgorithmName.SHA1),
                UUIDVersion.Version6 => IncrementalHash.CreateHash(HashAlgorithmName.SHA256),
                _ => throw new UnreachableException(),
            };
        }

        private static void SwapByteOrder(byte[] guid)
        {
            SwapBytes(guid, 0, 3);
            SwapBytes(guid, 1, 2);
            SwapBytes(guid, 4, 5);
            SwapBytes(guid, 6, 7);
        }

        private static void SwapBytes(byte[] guid, int left, int right)
        {
            byte temp = guid[left];
            guid[left] = guid[right];
            guid[right] = temp;
        }
    }
}
