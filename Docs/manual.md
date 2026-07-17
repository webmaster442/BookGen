---
title: Bookgen manual
tags: ''
---

# Bookgen

BookGen is a command-line Markdown processor designed to streamline the creation of books and technical documentation. 
It consists of two binaries: `BookGen` is the main command-line program and `BookGen.Shellprog.exe` is the shell helper
for PowerShell and works on Windows. 

For existing books, the most common tasks are available through the `Gui` sub-command. Running `BookGen Gui` starts a
terminal user interface (TUI) that lets you perform various book maintenance and workflow tasks.

To get the latest version of BookGen, please visit the releases page at: https://github.com/webmaster442/BookGen/releases

## Listing commands and getting help

BookGen is a command driven application. To get documentation on all the available commands use the `Bookgen doc commands`
command. This will display all the available commands. To get help on a specific command use the `help` sub-command with
the commands name. For example: `bookgen help doc commands`.

## Installation

Installation is straight-forward. Extract or copy BookGen into your folder of choice and you can then use it from that folder
with your shell. However I recommend running the `bookgen install` command that can install a Windows Terminal profile and add
BookGen into your `PATH` environment variable, so that you can use it from any folder.

## Writing your book workflow

The first step in writing any book is to know roughly what it's going to be about, but I suppose if you are looking for tools,
then you have your rough idea. The first step in using bookgen is to create a folder that you are going to be working in. 
After this, navigate to the folder and follow these steps.

Recommended tools to have installed:

* [GIT](https://git-scm.com/)
* [Visual Studio code](https://code.visualstudio.com/)

0. **Make sure that your folder is a GIT repository or some other version controlled folder**

    This is a somewhat optional step, nobody will stop you if it's not a GIT repository, but I highly recommend it, since if 
    you mess up something, than at least, you can revert to any revision at anytime. If you don't know what is GIT you can 
    find a detailed tutorial about it at https://www.w3schools.com/git/

1. **Create the Bookgen configuration files**

    This can be performed via executing the `bookgen newbook` command. This will create four files. A table of contents file
    and a configuration file. For both files a JSON schema document is also created and for editing the configurations an 
    editor that supports JSON schema based autocomplete and validation is recommended, like Visual studio code.

2. **Create a page**

	You can create a page by using your favorite text editor or by using the `bookgen newpage` command. The later adds the
    necessary front matter configuration to the file as well. If you have existing files without front matter content you 
    can use the `bookgen addfrontmatter` command to add them to existing files.

3. **Edit your page**

	Create your content. This is the hardest part and if you are like me, you will struggle a lot with finding proper words
    and inspiration. What I can recommend is don't stress. There are some days, when writing just one line is exhausting and
    there are others, where inspiration finds you and you just write and write.

4. **Add page to table of contents**

	Your created page is not automatically part of your book. You need to place it your table of contents file into 
    your specific chapter.

5. **Review and edit your configuration**

	Before building review your configuration and make necessary edits to it, if needed.

6. **Build your book**

    This can be done by utilizing one of the build commands:
	
	* `buildepub` - Build an epub3 file
	* `buildepxort` - Build a JSON export file, that can be post processed with your favorite JSON processor
	* `buildfeed` - Build RSS and ATOM feed files from your book
	* `buildprint` - Build a HTML and XHTML document that can be imported into word processors
	* `buildweb` - Build a static website
	* `buildwp` - Build a WordPress export XML that can be imported into a WordPress site

### Convinience commands

The philosophy behind BookGen that it should support the workflow as much as possible, that is why it has a few convenience
commands, that should make writing much more enjoyable. These commands are:

  * `bookgen vstasks` - Creates a `.vscode` folder with Visual Studio code and configures BookGen as available tasks. Optionally this command also creates a recommended extension list file to easily install the best tools for working with markdown files.

* `bookgen shortcut` - This command creates a shortcut file in the current directory that can be used to start the bookgen Shell in the current directory.
	
## Updating bookgen

The BookGen configuration format is versioned and in some releases it's updated. Some settings are removed and some are added as the program evolves. Bookgen offers a migration mechanism to update your book configuration file to the latest one used by Bookgen.

To make sure after updating your existing book is compatible with the tool run the `bookgen validate` command to validate your configuration file. If it needs upgrading then you will see an error message similar to this:

```
Config file is too old. Run bookgen upgrade to update it
```

To update to the latest configuration format run the `bookgen upgrade` command. This will upgrade your configuration to the latest version. **Warning**: 
It's highly unlikely that this will fail and mess up your configuration, but as a fail-safe please back up your existing configuration before migrating.

Migration also updates the configuration and the table of contents document schema files as well. 

After the migration, review the configuration file in your favorite editor and make necessary changes, if needed and validate your changes with `bookgen validate` 

## Converting a single markdown file to html

Converting a single markdown file can be achieved with the `md2html` command:

Renders a single markdown file to an HTML file.

```sh
BookGen md2html \
  [-ne, --no-embed] \
  [-ns, --no-syntax] \
  [-r, --raw] \
  [-s, --svg] \
  [-t, --title <value>] \
  [-tf, --template <value>] \
  -i, --input <value> \
  -o, --output <value>
```

### Options

* -`ne`, `--no-embed`

  **Optional option**

  Disables embedding of assets in the output HTML.

* -`ns`, `--no-syntax`

  **Optional option**

  Disables syntax highlighting in the output HTML.

* -`r`, `--raw`

  **Optional option**

  Disables full html generation, only outputs the html produced by the markdown formatting.

* -`s`, `--svg`

  **Optional option**

  When enabled SVG files will be embedded in resulting html, instead of being rendered to webp.

* -`t`, `--title`

  **Optional option**

  Specifies the rendered HTML page title. Only has affect, when `-r` or `--raw` is not specified.

* -`tf`, `--template`

  **Optional option**

  If not specified, default template is used. If custom file provided, then the file must contain the folloing tags: `<!--{title}-->`, `<!--{content}-->`

* -`i`, `--input`

  **Required option**

  Input markdown file path. Multiple files can be set with multiple `-i` arguments

* -`o`, `--output`

  **Required option**

  Output html file path. If file name is `-`, outputs to console.


## Markdown files

Markdown files are the main input format for BookGen. BookGen supports the following Markdown extensions on top of the [commonmark](https://commonmark.org/) specification:

* [Citations](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/FigureFooterAndCiteSpecs.md)
* [Custom containers](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/CustomContainerSpecs.md)
* [Definition lists](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/DefinitionListSpecs.md)
* [Extra emphasis](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/EmphasisExtraSpecs.md)
* [Figures](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/FigureFooterAndCiteSpecs.md)
* [Footers](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/FigureFooterAndCiteSpecs.md)
* [Grid tables](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/GridTableSpecs.md)
* [Mathematics](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/MathSpecs.md)
* [Media links](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/MediaSpecs.md)
* [Pipe tables](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/PipeTableSpecs.md)
* [List extras](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/ListExtraSpecs.md)
* [Task lists](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/TaskListSpecs.md)
* [Diagrams](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/DiagramsSpecs.md)
* [Auto links](https://github.com/xoofx/markdig/blob/main/src/Markdig.Tests/Specs/AutoLinks.md)
* [Alert blocks](https://xoofx.github.io/markdig/docs/extensions/alert-blocks/)
* YAML front matter for metadata
* Table of contents via the `[toc]` or `[toc maxlevel=n]` tag, where n is the level of headdings to include
* Keyboard input via the `<<` and `>>` tags, which will render the text between them as keyboard input.

### YAML Front matter

BookGen stores metadata information about each markdown file in a yaml front matter block, that will look something like this at the beginning of the markdown file:

```yaml
title: "Getting Started with BookGen"
tags: "bookgen, markdown, publishing, tutorial"
template: "tiny-light.html"
data:
  author: "Jane Doe"
  audience: "Developers"
  version: "1.0"
  category: "Documentation"
```

The front matter used in the beginning of the files follows the following schema:

```yaml
$schema: "https://json-schema.org/draft/2020-12/schema"
$id: "https://example.com/schemas/frontmatter.schema.yaml"
title: "FrontMatter"
type: object
additionalProperties: false
properties:
  title:
    type: string
    minLength: 1
    description: "Document title"
  tags:
    type: string
    minLength: 1
    description: "A comma separated list of tags"
  template:
    type:
      - string
      - "null"
    description: "Template file to use. If empty, default template is used"
  data:
    type:
      - object
      - "null"
    description: "Additional data that can be used during rendering"
    additionalProperties:
      type: string
required:
  - title
  - tags
```

## Templates

Template tags use a mustache like syntax starting with the `{{` symbols and ending with `}}`. Tags can include simple properties or functions. **Function and property names are case sensitive**

There are a few special properties that have special meaning. These are:

* `{{Title}}` - Title of the acual page
* `{{Content}}` - Content of the actual page
* `{{Host}}` - Host url, set in the configuration file

The `{{Content}}` mustache tag is a special placeholder used to represent the markdown content within a file. **Important:** A markdown document must not include the `{{Content}}` tag within its own content. Including this tag will cause infinite recursion during rendering, resulting in a failure to render the document.

The `Title` attribute comes from the YAML front matter of the document. The YAML front matter must include a `Title` and a `Tags` property. The Title is the document title and the tags are a comma separated string of keywords that can be set for the metadata.

You can also add additional data to the YAML front matter. The YAML front matter via the `Data` property, which is a Dictionary.

```yaml
Data:
  foo: bar
```

This will allow you to access this value in templates or markdown content using the mustache syntax `{{foo}}`. Property names are case sensitive.

### Template functions

| Function                | Category     | Description                          |
| ----------------------- | ------------ | ------------------------------------ |
| `ToUpper`               | Case         | Converts to uppercase                |
| `ToLower`               | Case         | Converts to lowercase                |
| `Substring`             | Manipulation | Extracts a portion of a string       |
| `Trim`                  | Manipulation | Removes surrounding whitespace       |
| `TrimStart`             | Manipulation | Removes leading whitespace           |
| `TrimEnd`               | Manipulation | Removes trailing whitespace          |
| `Replace`               | Manipulation | Replaces a substring                 |
| `Concat`                | Manipulation | Joins multiple values                |
| `RegexReplace`          | Manipulation | Replaces via regex (5s timeout)      |
| `HtmlEncode`            | Encoding     | HTML-encodes a value                 |
| `UrlEncode`             | Encoding     | URL-encodes a value                  |
| `UrlDecode`             | Encoding     | URL-decodes a value                  |
| `CurrentDate`           | Date/Time    | Current date (`yyyy-MM-dd`)          |
| `CurrentDateFormat`     | Date/Time    | Current date, custom format          |
| `CurrentTime`           | Date/Time    | Current time (`HH:mm:ss`)            |
| `CurrentTimeFormat`     | Date/Time    | Current time, custom format          |
| `CurrentDateTime`       | Date/Time    | Current date and time                |
| `CurrentDateTimeFormat` | Date/Time    | Current date and time, custom format |

> **Note:** Numeric arguments (such as `startIndex` and `length`) are converted using the invariant culture. Any value that is `null` is treated as an empty string.

#### String Case Functions

*  `ToUpper(obj)`
    
    Converts the given value to an uppercase string.

   - **Parameters:** `obj` — the value to convert.
   - **Returns:** The uppercase representation of the value.

* `ToLower(obj)`
    
    Converts the given value to a lowercase string.

  - **Parameters:** `obj` — the value to convert.
  - **Returns:** The lowercase representation of the value.

#### String Manipulation Functions

* `Substring(obj, startIndex, length)`

    Extracts a sub-string from the value.

    - **Parameters:**
      - `obj` — the source value.
      - `startIndex` — the zero-based starting position.
      - `length` — the number of characters to extract.
    - **Returns:** The extracted sub-string.

* `Trim(obj)`
    
    Removes all leading and trailing white-space from the value.

    - **Parameters:** `obj` — the value to trim.
    - **Returns:** The trimmed string.

* `TrimStart(obj)`
    
    Removes all leading white-space from the value.

    - **Parameters:** `obj` — the value to trim.
    - **Returns:** The string without leading white-space.

*  `TrimEnd(obj)`

    Removes all trailing white-space from the value.

   - **Parameters:** `obj` — the value to trim.
   - **Returns:** The string without trailing white-space.

* `Replace(obj, oldValue, newValue)`

    Replaces all occurrences of a sub-string with another sub-string.

    - **Parameters:**
      - `obj` — the source value.
      - `oldValue` — the sub-string to find.
      - `newValue` — the sub-string to substitute.
    - **Returns:** The resulting string after replacement.

* `Concat(args)`

    Concatenates multiple values into a single string.

    - **Parameters:** `args` — an array of values to concatenate.
    - **Returns:** The combined string.

*  `RegexReplace(obj, pattern, replacement)`

    Replaces text matching a regular expression pattern.

    - **Parameters:**
      - `obj` — the source value.
      - `pattern` — the regular expression pattern.
      - `replacement` — the replacement text.
    - **Returns:** The resulting string after replacement.
    - **Remarks:** Uses culture-invariant matching and enforces a **5 second** timeout to guard against catastrophic backtracking.

#### Encoding Functions

* `HtmlEncode(obj)`

    Encodes a value for safe inclusion in HTML.

    - **Parameters:** `obj` — the value to encode.
    - **Returns:** The HTML-encoded string.

*  `UrlEncode(obj)`

    Encodes a value for safe inclusion in a URL.

    - **Parameters:** `obj` — the value to encode.
    - **Returns:** The URL-encoded string.

* `UrlDecode(obj)`

    Decodes a URL-encoded value.
    
    **Parameters:** `obj` — the value to decode.
    **Returns:** The decoded string

#### Date and Time Functions

* `CurrentDate()`

    Returns the current date.

    - **Returns:** The date formatted as `yyyy-MM-dd`.

* `CurrentDateFormat(format)`

    Returns the current date using a custom format.

    - **Parameters:** `format` — a .NET date/time format string.
    - **Returns:** The date formatted with the specified format.

* `CurrentTime()`

    Returns the current time.

    - **Returns:** The time formatted as `HH:mm:ss`.

* `CurrentTimeFormat(format)`

    Returns the current time using a custom format.

    - **Parameters:** `format` — a .NET date/time format string.
    - **Returns:** The time formatted with the specified format.

* `CurrentDateTime()`

    Returns the current date and time.

    - **Returns:** The date and time formatted as `yyyy-MM-dd HH:mm:ss`.

* `CurrentDateTimeFormat(format)`

    Returns the current date and time using a custom format.

    - **Parameters:** `format` — a .NET date/time format string.
    - **Returns:** The date and time formatted with the specified format.