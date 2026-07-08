//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Rendering.Images;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands.Convert;

[CommandName("imgconvert")]
[Description("Converts an image file to a different format. The tool supports png, jpeg, webp and svg formats.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.ArgumentsError, "Invalid arguments provided.")]
internal sealed class ImgConvert : Command<ImgConvert.Arguments>
{
    private readonly IWritableFileSystem _fileSystem;

    internal enum ImageFormat
    {
        Jpg,
        Png,
        Webp
    }

    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("i", "input", Required = true)]
        [Description("Specifies the input image file or directory.")]
        public string Input { get; set; }

        [Switch("o", "output", Required = true)]
        [Description("Specifies the output image file or directory.")]
        public string Output { get; set; }

        [Switch("f", "format", Required = true)]
        [Description("Specifies the output image format (jpg, png, webp).")]
        public string Format { get; set; }

        [Switch("q", "quality", Required = false)]
        [Description("Specifies the quality of the output image (0-100). If not given, the default is 90.")]
        public int Quality { get; set; } = 90;

        [Switch("r", "resolution", Required = false)]
        [Description("Specifies the maximum resolution of the output image (WidthxHeight). If not given, the default is the size of the input image")]
        public string Resolution { get; set; }

        public Arguments()
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

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
        HashSet<string> supportedExtensions = new(StringComparer.InvariantCultureIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        var maxResolution = new Resolution
        {
            Width = int.MaxValue,
            Height = int.MaxValue
        };

        if (!string.IsNullOrEmpty(arguments.Resolution)
            && !Resolution.TryParse(arguments.Resolution, CultureInfo.InvariantCulture, out maxResolution))
        {
            Console.Error.WriteLine($"Invalid resolution format: '{arguments.Resolution}'. Expected format is 'WidthxHeight'.");
            return ExitCodes.ArgumentsError;
        }

        ImageFormat format = Enum.Parse<ImageFormat>(arguments.Format, ignoreCase: true);

        if (_fileSystem.DirectoryExists(arguments.Input))
        {
            IEnumerable<string> files = _fileSystem.GetFiles(arguments.Input, "*.*", false).Where(f => supportedExtensions.Contains(Path.GetExtension(f)));
            Parallel.ForEach(files, file =>
            {
                var outputFile = Path.Combine(arguments.Output, Path.GetFileNameWithoutExtension(file) + "." + arguments.Format);
                ConvertImage(file, outputFile, format, arguments.Quality, maxResolution);
            });
        }
        else
        {
            ConvertImage(arguments.Input, arguments.Output, format, arguments.Quality, maxResolution);
        }

        return ExitCodes.Success;
    }

    private static void ConvertImage(string file, string outputFile, ImageFormat format, int quality, Resolution maxResolution)
    {
        ImageConverter.Encode(file, outputFile, format switch
        {
            ImageFormat.Jpg => ImageType.Jpeg,
            ImageFormat.Png => ImageType.Png,
            ImageFormat.Webp => ImageType.Webp,
            _ => throw new NotSupportedException($"Image format {format} is not supported.")
        }, maxResolution.Width, maxResolution.Height, quality);
    }
}
