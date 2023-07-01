namespace Webmaster442.HttpServerFramework.Domain;

[Flags]
internal enum EncodingType
{
    None = 0,
    Deflate = 1,
    Gzip = 2,
    Brotli = 4,
}
