using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Bookgen.Win
{
    public static class IntegrityCheck
    {
        public static async Task<List<IntegrityItem>> Create(string folder,
                                                             ICountProgress countProgress,
                                                             CancellationToken cancellationToken)
        {
            var result = new List<IntegrityItem>();

            string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            countProgress.SetMaximum(files.Length);

            int count = 0;

            foreach (string file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();

                result.Add(new IntegrityItem
                {
                    FileName = GetRelativePath(folder, file),
                    Hash = await GetFileHash(file, cancellationToken),
                    DateHash = GetFileDate(file)
                });

                ++count;

                countProgress.Report(count);
            }

            return result;
        }

        private static string GetFileDate(string file)
        {
            var d =  BitConverter.GetBytes(File.GetLastWriteTime(file).ToBinary());
            return Convert.ToBase64String(d);
        }

        public static async Task<bool> Verify(string folder,
                                              IReadOnlyList<IntegrityItem> items,
                                              IResultProgrss resultProgrss,
                                              CancellationToken cancellationToken)
        {
            int count = 0;

            resultProgrss.SetMaximum(items.Count);

            foreach (IntegrityItem item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fileName = Path.Combine(folder, item.FileName);
                if (!File.Exists(fileName))
                {
                    resultProgrss.ReportFailed(fileName);
                    continue;
                }

                var hash = await GetFileHash(fileName, cancellationToken);
                var date = GetFileDate(fileName);
                if (hash != item.Hash
                    || date != item.DateHash)
                {
                    resultProgrss.ReportFailed(fileName);
                }
                ++count;
                resultProgrss.Report(count);
            }

            return true;
        }

        private const int BufferSize = 16 * 1024;

        private static async Task<string> GetFileHash(string fileName, CancellationToken cancellationToken)
        {
            using (var algo = SHA256.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {

                    byte[] buffer;
                    int bytesRead;
                    long position = 0;
                    long size = stream.Length;
                    byte[] readAheadBuffer = new byte[BufferSize];

                    int readAheadBytesRead = await stream.ReadAsync(readAheadBuffer, 0, readAheadBuffer.Length, cancellationToken);
                    position += readAheadBytesRead;

                    do
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        bytesRead = readAheadBytesRead;
                        buffer = readAheadBuffer;
                        readAheadBuffer = new byte[BufferSize];
                        readAheadBytesRead = await stream.ReadAsync(readAheadBuffer, 0, readAheadBuffer.Length, cancellationToken);
                        position += readAheadBytesRead;

                        if (readAheadBytesRead == 0)
                            algo.TransformFinalBlock(buffer, 0, bytesRead);
                        else
                            algo.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                    }
                    while (readAheadBytesRead != 0);
                }
                return Convert.ToBase64String(algo.Hash);
            }
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path += Path.DirectorySeparatorChar;
            }
            return path;
        }

        private static string GetRelativePath(string basePath, string targetPath)
        {
            Uri baseUri = new Uri(AppendDirectorySeparatorChar(basePath));
            Uri targetUri = new Uri(targetPath);

            Uri relativeUri = baseUri.MakeRelativeUri(targetUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
