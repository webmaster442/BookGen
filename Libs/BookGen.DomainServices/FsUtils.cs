﻿//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Serialization;

using BookGen.Api;
using BookGen.Interfaces;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BookGen.DomainServices
{
    public static class FsUtils
    {
        public static bool CreateDir(this FsPath path, ILog log)
        {
            try
            {
                log.Detail("Creating directory: {0}", path);
                if (!FsPath.IsEmptyPath(path))
                {
                    Directory.CreateDirectory(path.ToString());
                    return true;
                }
                else
                {
                    log.Warning("CreateDir called with empty input path");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Warning("CreateDir failed: {0}", path);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static bool CopyDirectory(this FsPath sourceDirectory, FsPath targetDir, ILog log)
        {
            try
            {
                if (!Directory.Exists(targetDir.ToString()))
                {
                    log.Detail("Creating directory: {0}", targetDir);
                    Directory.CreateDirectory(targetDir.ToString());
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(sourceDirectory.ToString(), "*.*",
                    SearchOption.AllDirectories))
                {
                    string? targetfile = newPath.Replace(sourceDirectory.ToString(), targetDir.ToString());
                    log.Detail("Copy file: {0} to {1}", newPath, targetfile);

                    string? targetDirString = Path.GetDirectoryName(targetfile);
                    if (targetDirString != null && !Directory.Exists(targetDirString))
                    {
                        log.Detail("Creating directory: {0}", targetDirString);
                        Directory.CreateDirectory(targetDirString);
                    }

                    File.Copy(newPath, targetfile, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Warning("CopyDirectory failed: {0} to {1}", sourceDirectory, targetDir);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static bool Copy(this FsPath source, FsPath target, ILog log)
        {
            try
            {
                if (!source.IsExisting)
                {
                    log.Warning("Source doesn't exist, skipping: {0}");
                    return false;
                }

                string? dir = Path.GetDirectoryName(target.ToString()) ?? string.Empty;
                if (!Directory.Exists(dir))
                {
                    log.Detail("Creating directory: {0}", dir);
                    Directory.CreateDirectory(dir);
                }

                File.Copy(source.ToString(), target.ToString());

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("Copy failed: {0} to {1}", source, target);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static bool Move(this FsPath source, FsPath target, ILog log)
        {
            try
            {
                if (!source.IsExisting)
                {
                    log.Warning("Source doesn't exist, skipping: {0}");
                    return false;
                }

                string? dir = Path.GetDirectoryName(target.ToString()) ?? string.Empty;
                if (!Directory.Exists(dir))
                {
                    log.Detail("Creating directory: {0}", dir);
                    Directory.CreateDirectory(dir);
                }

                File.Move(source.ToString(), target.ToString());

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("Move failed: {0} to {1}", source, target);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static FileStream OpenStreamRead(this FsPath target)
        {
            return File.OpenRead(target.ToString());
        }

        public static FileStream CreateStream(this FsPath target, ILog log)
        {
            string? dir = Path.GetDirectoryName(target.ToString()) ?? string.Empty;
            if (!Directory.Exists(dir))
            {
                log.Detail("Creating directory: {0}", dir);
                Directory.CreateDirectory(dir);
            }

            return File.Create(target.ToString());
        }

        public static StreamWriter CreateStreamWriter(this FsPath target, ILog log)
        {
            string? dir = Path.GetDirectoryName(target.ToString()) ?? string.Empty;
            if (!Directory.Exists(dir))
            {
                log.Detail("Creating directory: {0}", dir);
                Directory.CreateDirectory(dir);
            }

            return File.CreateText(target.ToString());
        }

        public static bool CreateBackup(this FsPath source, ILog log)
        {
            try
            {
                if (!source.IsExisting)
                {
                    log.Detail("Source doesn't exist, skipping: {0}");
                    return false;
                }
                string targetname = $"{source}_backup";
                if (File.Exists(targetname))
                {
                    bool exists = true;
                    int counter = 1;
                    do
                    {
                        targetname = $"{source}_backup{counter}";
                        ++counter;
                        exists = File.Exists(targetname);
                    }
                    while (exists);
                }
                File.Copy(source.ToString(), targetname);
                return true;
            }
            catch (Exception ex)
            {
                log.Warning("CreateBackup failed: {0}", source);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static bool WriteFile(this FsPath target, ILog log, params string[] contents)
        {
            try
            {
                var fileInfo = new FileInfo(target.ToString());

                if (!fileInfo.Exists && fileInfo.Directory != null)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                using (StreamWriter? writer = File.CreateText(target.ToString()))
                {
                    foreach (string? content in contents)
                    {
                        writer.Write(content);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("WriteFile failed: {0}", target);
                log.Detail(ex.Message);
                return false;
            }
        }

        public static string ReadFile(this FsPath path, ILog log, bool appendExtraLine = false)
        {
            try
            {
                using (StreamReader? reader = File.OpenText(path.ToString()))
                {
                    if (appendExtraLine)
                    {
                        var buffer = new StringBuilder(reader.ReadToEnd());
                        buffer.AppendLine();
                        return buffer.ToString();
                    }

                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Warning("ReadFile failed: {0}", path);
                log.Detail(ex.Message);
                return string.Empty;
            }
        }

        public static IList<string> ReadFileLines(this FsPath path, ILog log)
        {
            try
            {
                var lines = new List<string>(100);
                using (StreamReader? reader = File.OpenText(path.ToString()))
                {
                    string? line;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            lines.Add(line);
                        }
                    }
                    while (line != null);
                }
                return lines;
            }
            catch (Exception ex)
            {
                log.Warning("ReadFile failed: {0}", path);
                log.Detail(ex.Message);
                return Array.Empty<string>();
            }
        }

        public static void ProtectDirectory(this FsPath directory, ILog log)
        {
            FsPath? outp = directory.Combine("index.html");
            var sb = new StringBuilder(4096);
            for (int i = 0; i < 256; i++)
            {
                sb.Append("                ");
            }
            outp.WriteFile(log, sb.ToString());
        }

        public static IEnumerable<FsPath> GetAllFiles(this FsPath directory, bool recursive = true, string mask = "*.*")
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string? file in Directory.GetFiles(directory.ToString(), mask, searchOption))
            {
                yield return new FsPath(file);
            }
        }

        public static bool SerializeXml<T>(this FsPath path, T obj, ILog log, IReadOnlyList<(string prefix, string namespac)>? nslist = null) where T : class
        {
            try
            {
                var fileInfo = new FileInfo(path.ToString());

                if (!fileInfo.Exists && fileInfo.Directory != null)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                XmlSerializerNamespaces? xnames = null;
                if (nslist != null)
                {
                    xnames = new XmlSerializerNamespaces();
                    foreach ((string prefix, string namespac) in nslist)
                    {
                        xnames.Add(prefix, namespac);
                    }
                }

                var xs = new XmlSerializer(typeof(T));
                using (FileStream? writer = File.Create(path.ToString()))
                {
                    if (xnames == null)
                        xs.Serialize(writer, obj);
                    else
                        xs.Serialize(writer, obj, xnames);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("SerializeXml failed: {0} type: {1}", path, typeof(T));
                log.Detail(ex.Message);
                return false;
            }
        }

        public static bool SerializeJson<T>(this FsPath path, T obj, ILog log, bool indent = true) where T : class, new()
        {
            try
            {
                var fileInfo = new FileInfo(path.ToString());

                if (!fileInfo.Exists && fileInfo.Directory != null)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                byte[] serialized = JsonSerializer.SerializeToUtf8Bytes(obj, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = indent,
                    Converters =
                    {
                        new CultureInfoConverter()
                    }
                });

                File.WriteAllBytes(path.ToString(), serialized);

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("SerializeJson failed: {0} type: {1}", path, typeof(T));
                log.Detail(ex.Message);
                return false;
            }
        }

        public static T? DeserializeJson<T>(this FsPath path, ILog log) where T : class, new()
        {
            try
            {
                using (StreamReader? reader = File.OpenText(path.ToString()))
                {
                    string text = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<T>(text, new JsonSerializerOptions
                    {
                        Converters =
                        {
                            new CultureInfoConverter()
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Warning("DeserializeJson failed: {0} type: {1}", path, typeof(T));
                log.Critical(ex);
                return default;
            }
        }

        public static bool SerializeYaml<T>(this FsPath path, T obj, ILog log) where T : class, new()
        {
            try
            {
                var fileInfo = new FileInfo(path.ToString());

                if (!fileInfo.Exists && fileInfo.Directory != null)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                ISerializer? serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .WithTypeConverter(new CultureInfoConverter())
                    .Build();

                string? yaml = serializer.Serialize(obj);

                File.WriteAllText(path.ToString(), yaml);

                return true;
            }
            catch (Exception ex)
            {
                log.Warning("SerializeYaml failed: {0} type: {1}", path, typeof(T));
                log.Detail(ex.Message);
                return false;
            }
        }

        public static T? DeserializeYaml<T>(this FsPath path, ILog log) where T : class, new()
        {
            try
            {
                using (StreamReader? reader = File.OpenText(path.ToString()))
                {
                    string text = reader.ReadToEnd();

                    IDeserializer? deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .WithTypeConverter(new CultureInfoConverter())
                        .Build();

                    return deserializer.Deserialize<T>(text);
                }
            }
            catch (Exception ex)
            {
                log.Warning("DeserializeYaml failed: {0} type: {1}", path, typeof(T));
                log.Critical(ex);
                return default;
            }
        }

        public static FsPath GetAbsolutePathRelativeTo(this FsPath path, FsPath file)
        {
            try
            {
                if (path.ToString().StartsWith("../"))
                {
                    path = new FsPath(path.ToString()[3..]);
                }

                string filespec = path.ToString();
                string folder = file.ToString();
                if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    folder += Path.DirectorySeparatorChar;
                }

                var pathUri = new Uri(new Uri(folder), filespec);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return new FsPath(pathUri.ToString().Replace("file://", ""));
                }
                else
                {
                    return new FsPath(pathUri.ToString().Replace("file:///", ""));
                }
            }
            catch (UriFormatException)
            {
                return FsPath.Empty;
            }
        }

        public static FsPath GetRelativePathRelativeTo(this FsPath path, FsPath file)
        {
            try
            {
                string filespec = path.ToString();
                string folder = file.ToString();

                if (file.Extension == null)
                    folder = Path.GetDirectoryName(file.ToString()) ?? string.Empty;

                var pathUri = new Uri(filespec);

                if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    folder += Path.DirectorySeparatorChar;
                }

                var folderUri = new Uri(folder);

                string? relatvie = folderUri.MakeRelativeUri(pathUri).ToString();

                string? ret = Uri.UnescapeDataString(relatvie.Replace('/', Path.DirectorySeparatorChar));
                return new FsPath(ret);
            }
            catch (UriFormatException)
            {
                return FsPath.Empty;
            }
        }

        public static FsPath GetDirectory(this FsPath path)
        {
            string? fullpath = Path.GetFullPath(path.ToString());
            return new FsPath(Path.GetDirectoryName(fullpath) ?? string.Empty);
        }

        public static bool IsWildCard(this FsPath path)
        {
            return path.ToString().Contains('*');
        }

        public static FsPath ChangeExtension(this FsPath path, string extension)
        {
            string? x = Path.ChangeExtension(path.ToString(), extension);
            return new FsPath(x);
        }

        public static bool Delete(this FsPath path, ILog log)
        {
            try
            {
                File.Delete(path.ToString());
                return true;
            }
            catch (Exception ex)
            {
                log.Warning("File detlete failed: {0}", path);
                log.Critical(ex);
                return false;
            }
        }
    }
}