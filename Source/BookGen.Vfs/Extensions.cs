//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Xml;

namespace BookGen.Vfs;

public static class Extensions
{
    extension(IReadOnlyFileSystem fs)
    {
        public async Task<T?> DeserializeAsync<T>(string path)
        {
            await using var stream = fs.OpenReadStream(path);
            T? result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions.SerializerOptions);
            return result;
        }

        public async Task<JsonObject> ReadJsonAsync(string path)
        {
            string content = await fs.ReadAllTextAsync(path);
            JsonNode? parsed = JsonNode.Parse(content);
            if (parsed is not JsonObject jsonObject)
            {
                throw new InvalidOperationException($"Failed to parse JSON from {path}");
            }
            return jsonObject;
        }

        public string GetFileNameInTargetFolder(IReadOnlyFileSystem targetFolder, string file, string newExtension)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fs.Scope);
            ArgumentException.ThrowIfNullOrWhiteSpace(targetFolder.Scope);

            var fullPath = Path.GetFullPath(file, fs.Scope);

            var relativePart = Path.GetRelativePath(fs.Scope, fullPath);

            return Path.ChangeExtension(Path.GetFullPath(relativePart, targetFolder.Scope), newExtension);
        }

        public (string content, DateTime lastmodified) ReadInputFiles(string[] inputFiles)
        {
            StringBuilder md = new(inputFiles.Length * 1024);
            DateTime lastmodified = DateTime.MinValue;
            foreach (var inputFile in inputFiles)
            {
                string content = fs.ReadAllText(inputFile);
                DateTime date = fs.GetLastModifiedUtc(inputFile);

                if (date > lastmodified)
                    lastmodified = date;

                md.Append(content);

                if (!content.EndsWith('\n'))
                    md.Append(System.Environment.NewLine);
            }
            return (md.ToString(), lastmodified);
        }
    }

    extension(IWritableFileSystem fs)
    {
        public XmlWriter CreateXmlWriter(string path)
        {
            return XmlWriter.Create(fs.CreateTextWriter(path));
        }

        public async Task WriteJsonAsync(string path, JsonObject json)
        {
            await fs.WriteAllTextAsync(path, json.ToJsonString(JsonOptions.SerializerOptions));
        }

        public async Task WriteSchema<T>(string path)
        {
            var node = JsonOptions.SerializerOptions.GetJsonSchemaAsNode(typeof(T), JsonOptions.ExporterOptions);
            await fs.WriteAllTextAsync(path, node.ToJsonString(JsonOptions.SerializerOptions));
        }

        public async Task SerializeAsync<T>(string path, T value, bool writeSchema)
        {
            await using var stream = fs.CreateWriteStream(path);
            await JsonSerializer.SerializeAsync(stream, value, JsonOptions.SerializerOptions);
            if (writeSchema)
            {
                var newName = Path.ChangeExtension(path, ".schema.json");
                await fs.WriteSchema<T>(newName);
            }
        }
    }
}
