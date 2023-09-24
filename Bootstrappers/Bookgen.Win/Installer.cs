using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bookgen.Win
{
    public static class Installer
    {
        public static async Task<string> Install(string sourceDirectory, string targetDir, IProgress<double> progress, CancellationToken cancellationToken)
        {
            try
            {
                if (!Directory.Exists(sourceDirectory))
                    throw new DirectoryNotFoundException($"Source directory '{sourceDirectory}' not found.");

                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                string[] allFiles = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);
                int done = 0;

                foreach (string sourceFile in allFiles)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string relativePath = GetRelativePath(sourceDirectory, sourceFile);
                    string destFile = Path.Combine(targetDir, relativePath);

                    using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                    {
                        using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                        {
                            await sourceStream.CopyToAsync(destStream, 4096, cancellationToken);
                            ++done;
                            progress.Report((double)done / allFiles.Length);
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Rollback(string targetDir)
        {
            try
            {
                Directory.Delete(targetDir, true);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string GetRelativePath(string rootPath, string fullPath)
        {
            Uri rootUri = new Uri(rootPath);
            Uri fullUri = new Uri(fullPath);
            Uri relativeUri = rootUri.MakeRelativeUri(fullUri);
            return Uri.UnescapeDataString(relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
