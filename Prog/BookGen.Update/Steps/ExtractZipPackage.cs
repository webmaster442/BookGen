//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using System.IO.Compression;

namespace BookGen.Update.Steps
{
    internal sealed class ExtractZipPackage : IUpdateStepSync
    {
        private const string UpdaterName = "BookGen.Update";

        public string StatusMessage => "Extracting update...";

        public bool Execute(GlobalState state)
        {
            if (!File.Exists(state.TempFile))
            {
                state.Issues.Add("Downloaded update file not found");
                return false;
            }

            using (ZipArchive zip = ZipFile.OpenRead(state.TempFile))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    ExtractRelativeToDirectory(entry, state.TargetDir, state.PostProcessFiles, true);
                }
            }

            return true;
        }

        internal static string SanitizeEntryFilePath(string entryPath) => entryPath.Replace('\0', '_');

        internal static void ExtractRelativeToDirectory(ZipArchiveEntry source,
                                                        string destinationDirectoryName,
                                                        List<(string source, string target)> postProcessFiles,
                                                        bool overwrite)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destinationDirectoryName);

            // Note that this will give us a good DirectoryInfo even if destinationDirectoryName exists:
            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;
            if (!destinationDirectoryFullPath.EndsWith(Path.DirectorySeparatorChar))
                destinationDirectoryFullPath += Path.DirectorySeparatorChar;

            string fileName = SanitizeEntryFilePath(source.FullName);

            string fileDestinationPath = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, fileName));

            if (!fileDestinationPath.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                throw new IOException("Extracting Zip entry would have resulted in a file outside the specified destination directory.");

            if (Path.GetFileName(fileDestinationPath).Length == 0)
            {
                // If it is a directory:

                if (source.Length != 0)
                    throw new IOException("Zip entry name ends in directory separator character but contains data.");

                Directory.CreateDirectory(fileDestinationPath);
            }
            else
            {
                // If it is a file:
                // Create containing directory:
                Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath)!);
                if (CanWrite(fileDestinationPath))
                {
                    source.ExtractToFile(fileDestinationPath, overwrite: overwrite);
                }
                else
                {
                    string extension = Path.GetExtension(fileDestinationPath);
                    string finalFileName = Path.ChangeExtension(fileDestinationPath, extension + "_new");
                    postProcessFiles.Add((finalFileName, fileDestinationPath));
                    source.ExtractToFile(finalFileName, overwrite: overwrite);
                }
            }
        }

        private static bool CanWrite(string fileDestinationPath)
        {
            bool returnValue = true;
            if (File.Exists(fileDestinationPath))
            {
                using (var fs = new FileStream(fileDestinationPath, FileMode.Open))
                {
                    returnValue = fs.CanWrite;
                }
            }
            return returnValue;
        }
    }
}
