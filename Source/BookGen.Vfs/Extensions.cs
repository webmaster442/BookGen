//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Xml;

namespace BookGen.Vfs;

public static class Extensions
{
    extension(Stream stream)
    {
        public string Base64Encode()
        {
            const int readBufferSize = 4096;

            int size = (int)Math.Ceiling(4 * (stream.Length / 3.0));

            var stringBuilder = new StringBuilder(size);

            byte[] remainder = new byte[2];
            int remainderCount = 0;

            byte[] readBuffer = ArrayPool<byte>.Shared.Rent(readBufferSize);
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    int totalBytesToProcess = remainderCount + bytesRead;

                    // Determine how many bytes can form complete 3-byte triplets.
                    var bytesToTakeForEncoding = (totalBytesToProcess / 3) * 3;

                    if (bytesToTakeForEncoding > 0)
                    {
                        var chunk = ArrayPool<byte>.Shared.Rent(bytesToTakeForEncoding);
                        remainder.AsSpan(0, remainderCount).CopyTo(chunk);
                        readBuffer.AsSpan(0, bytesToTakeForEncoding - remainderCount).CopyTo(chunk.AsSpan(remainderCount));
                        stringBuilder.Append(Convert.ToBase64String(chunk, 0, bytesToTakeForEncoding));
                        ArrayPool<byte>.Shared.Return(chunk);

                        remainderCount = bytesRead - (bytesToTakeForEncoding - remainderCount);
                        readBuffer.AsSpan(bytesRead - remainderCount, remainderCount).CopyTo(remainder);
                    }
                    else
                    {
                        readBuffer.AsSpan(0, bytesRead).CopyTo(remainder.AsSpan(remainderCount));
                        remainderCount += bytesRead;
                    }
                }

                if (remainderCount > 0)
                {
                    stringBuilder.Append(Convert.ToBase64String(remainder.AsSpan(0, remainderCount).ToArray()));
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(readBuffer);
            }

            return stringBuilder.ToString();
        }
    }

    extension(IReadOnlyFileSystem fs)
    {
        public async Task<T?> DeserializeAsync<T>(string path)
        {
            await using Stream stream = fs.OpenReadStream(path);
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
            JsonNode node = JsonOptions.SerializerOptions.GetJsonSchemaAsNode(typeof(T), JsonOptions.ExporterOptions);
            await fs.WriteAllTextAsync(path, node.ToJsonString(JsonOptions.SerializerOptions));
        }

        public async Task SerializeAsync<T>(string path, T value, bool writeSchema)
        {
            await using Stream stream = fs.CreateWriteStream(path);
            await JsonSerializer.SerializeAsync(stream, value, JsonOptions.SerializerOptions);
            if (writeSchema)
            {
                var newName = Path.ChangeExtension(path, ".schema.json");
                await fs.WriteSchema<T>(newName);
            }
        }
    }
}
