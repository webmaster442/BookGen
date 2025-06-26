using System.Globalization;

using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands;

internal sealed class ImgConvert : Command<ImgConvert.ImgConvertArgs>
{
    private readonly IWritableFileSystem _fileSystem;

    public enum ImageFormat
    {
        Jpg,
        Png,
        Webp
    }

    public class ImgConvertArgs : ArgumentsBase
    {
        [Switch("i", "input")]
        public string Input { get; set; }

        [Switch("o", "output")]
        public string Output { get; set; }

        [Switch("f", "format")]
        public string Format { get; set; }

        [Switch("q", "quality")]
        public int Quality { get; set; } = 90;

        [Switch("r", "resolution")]
        public string Resolution { get; set; }

        public ImgConvertArgs()
        {
            Input = string.Empty;
            Output = string.Empty;
            Format = "jpg"; // Default format
            Quality = 90; // Default quality
            Resolution = string.Empty; // Default resolution
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult results = new();
            if (string.IsNullOrWhiteSpace(Input))
                results.AddIssue("Input file is required.");

            if (string.IsNullOrWhiteSpace(Output))
                results.AddIssue("Output file is required.");

            if (Quality < 0 || Quality > 100)
                results.AddIssue("Quality must be between 0 and 100.");

            if (!context.FileSystem.DirectoryExists(Input)
                && !context.FileSystem.FileExists(Input))
            {
                results.AddIssue($"Input file or directory '{Input}' does not exist.");
            }

            if (string.IsNullOrWhiteSpace(Format))
            {
                results.AddIssue("Format is required.");
            }
            else if (!Enum.TryParse<ImageFormat>(Format, ignoreCase: true, out _))
            {
                results.AddIssue($"Invalid format '{Format}'. Valid formats are: {string.Join(',', Enum.GetNames<ImageFormat>())}.");
            }

            return results;
        }
    }

    public ImgConvert(IWritableFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public override int Execute(ImgConvertArgs arguments, IReadOnlyList<string> context)
    {
        HashSet<string> supportedExtensions = new(StringComparer.InvariantCultureIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        if (!Resolution.TryParse(arguments.Resolution, CultureInfo.InvariantCulture, out var resolution))
        {
            Console.Error.WriteLine($"Invalid resolution format: '{arguments.Resolution}'. Expected format is 'WidthxHeight'.");
            return ExitCodes.ArgumentsError;
        }

        var format = Enum.Parse<ImageFormat>(arguments.Format, ignoreCase: true);

        if (_fileSystem.DirectoryExists(arguments.Input))
        {
            var files = _fileSystem.GetFiles(arguments.Input, "*.*", false).Where(f => supportedExtensions.Contains(Path.GetExtension(f)));
            Parallel.ForEach(files, file =>
            {
                var outputFile = Path.Combine(arguments.Output, Path.GetFileNameWithoutExtension(file) + "." + arguments.Format);
                ConvertImage(file, outputFile, format, arguments.Quality, resolution);
            });
        }
        else
        {
            ConvertImage(arguments.Input, arguments.Output, format, arguments.Quality, resolution);
        }

        return ExitCodes.Succes;
    }

    private static void ConvertImage(string file, string outputFile, ImageFormat format, int quality, Resolution resolution)
    {
        ImageConverter.Encode(file, outputFile, format switch
        {
            ImageFormat.Jpg => ImageType.Jpeg,
            ImageFormat.Png => ImageType.Png,
            ImageFormat.Webp => ImageType.Webp,
            _ => throw new NotSupportedException($"Image format {format} is not supported.")
        }, resolution.Width, resolution.Height, quality);
    }
}
