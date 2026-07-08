//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WeCantSpell.Hunspell;

namespace BookGen.Commands;

[CommandName("spellcheck")]
[Description("Perform spell check on a given markdown file or text file. The command will print the misspelled words to the console.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "Dictionary was not found or file contained spelling mistakes.")]
internal sealed class SpellCheckCommand : AsyncCommand<SpellCheckCommand.Arguments>
{
    internal sealed class Arguments : ArgumentsBase, IVerbosablityToggle
    {
        [Switch("i", "input", Required = true)]
        [Description("Specifies the input file path. The file must be a markdown file or a text file.")]
        public string InputFile { get; set; } = string.Empty;

        [Switch("v", "verbose", Required = false)]
        [Description("Turns on detailed logging. Usefull for locating issues")]
        public bool Verbose { get; set; }

        [Switch("l", "language", Required = false)]
        [Description("Specifies the language to use for spell checking. The value must be a valid language code, like en_US or hu_HU. If not specified, then en_US will be used as the default language.")]
        public string Language { get; set; } = "en_US";

        [Switch("-ld", "--list-dictionaries", Required = false)]
        [Description("When specified, the command will list all available dictionaries and exit.")]
        public bool DictionariesDisplay { get; set; } = false;

        public override ValidationResult Validate(IValidationContext context)
        {
            if (!context.FileSystem.FileExists(InputFile))
                return ValidationResult.Error($"Input file '{InputFile}' does not exist.");

            var extension = Path.GetExtension(InputFile);

            if (!string.Equals(extension, ".md", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(extension, ".txt", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Error($"Input file '{InputFile}' is not a markdown file.");
            }

            return ValidationResult.Ok();
        }
    }

    private readonly IAssetSource _dictionaries;
    private readonly ILogger _logger;
    private readonly IReadOnlyFileSystem _fileSystem;

    public SpellCheckCommand([FromKeyedServices("dictionaries")] IAssetSource dictionaries,
                             ILogger logger,
                             IReadOnlyFileSystem fileSystem)
    {
        _dictionaries = dictionaries;
        _logger = logger;
        _fileSystem = fileSystem;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context)
    {
        if (arguments.DictionariesDisplay)
        {
            _logger.LogInformation("Available dictionaries:");
            foreach (string assetName in _dictionaries.AssetNames)
            {
                if (assetName.EndsWith(".dic", StringComparison.OrdinalIgnoreCase))
                {
                    string language = Path.GetFileNameWithoutExtension(assetName);
                    _logger.LogInformation("- {Language}", language);
                }
            }
            return ExitCodes.Success;
        }

        WordList? dictionaries = await LoadDictionaries(arguments.Language);
        if (dictionaries == null)
        {
            _logger.LogError("Dictionaries for language '{Language}' not found.", arguments.Language);
            return ExitCodes.GeneralError;
        }

        var extension = Path.GetExtension(arguments.InputFile);

        int count = 0;

        if (string.Equals(extension, ".txt", StringComparison.OrdinalIgnoreCase))
        {
            string text = await _fileSystem.ReadAllTextAsync(arguments.InputFile);
            foreach (var word in SplitIntoWords(text))
            {
                if (!string.IsNullOrWhiteSpace(word) && !dictionaries.Check(word))
                {
                    IEnumerable<string> suggestions = dictionaries.Suggest(word);
                    _logger.LogWarning("Misspelled word: '{Word}'. Suggestions: {Suggestions}", word, string.Join(", ", suggestions));
                    ++count;
                }
            }
        }

        if (string.Equals(extension, ".md", StringComparison.OrdinalIgnoreCase))
        {
            string markdown = await _fileSystem.ReadAllTextAsync(arguments.InputFile);
            var document = Markdig.Markdown.Parse(markdown);
            CheckBlock(document, dictionaries, ref count);
        }

        return count == 0 ? ExitCodes.Success : ExitCodes.GeneralError;

    }

    private async Task<WordList?> LoadDictionaries(string language)
    {
        string dicFileName = $"{language}.dic";
        string affFileName = $"{language}.aff";

        if (!_dictionaries.AssetNames.Contains(dicFileName)
            && !_dictionaries.AssetNames.Contains(affFileName))
        {
            return null;
        }

        await using Stream dicStream = _dictionaries.GetBinaryAssetStream(dicFileName);
        await using Stream affStream = _dictionaries.GetBinaryAssetStream(affFileName);
        return await WordList.CreateFromStreamsAsync(dicStream, affStream);
    }

    private void CheckBlock(Block block, WordList spellChecker, ref int count)
    {
        if (block is LeafBlock leafBlock)
        {
            if (leafBlock.Inline != null)
            {
                foreach (Inline inline in leafBlock.Inline)
                {
                    CheckInline(inline, spellChecker, ref count);
                }
            }
        }
        else if (block is ContainerBlock containerBlock)
        {
            foreach (Block child in containerBlock)
            {
                CheckBlock(child, spellChecker, ref count);
            }
        }
    }

    private void CheckInline(Inline inline, WordList spellChecker, ref int count)
    {
        if (inline is LiteralInline literal)
        {
            IEnumerable<string> words = SplitIntoWords(literal.Content.ToString());
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word) && !spellChecker.Check(word))
                {
                    IEnumerable<string> suggestions = spellChecker.Suggest(word);
                    _logger.LogWarning("Misspelled word: '{Word}'. Suggestions: {Suggestions}", word, string.Join(", ", suggestions));
                }
            }
        }
        else if (inline is ContainerInline container)
        {
            foreach (Inline child in container)
            {
                CheckInline(child, spellChecker, ref count);
            }
        }
    }

    private static IEnumerable<string> SplitIntoWords(string text)
    {
        var currentWord = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c) || c == '\'')
            {
                currentWord.Append(c);
            }
            else if (currentWord.Length > 0)
            {
                string result = currentWord.ToString();
                currentWord.Clear();
                yield return result;
            }
        }

        if (currentWord.Length > 0)
        {
            yield return currentWord.ToString();
        }
    }
}
