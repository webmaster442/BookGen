namespace WpLoad.Domain
{
    internal record UploadFile
    {
        public string Path { get; init; }
        public string MimeType { get; init; }
        public long Size { get; init; }

        public UploadFile()
        {
            Path = string.Empty;
            MimeType = string.Empty;
        }
    }
}
