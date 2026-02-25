# Help

BookGen - Markdown to Book tool.

For the tool to work in the work folder there must be a bookgen.json config file. This config file can be created with the following command:

`BookGen Newbook`

To Get more help about a subcommand type:

`BookGen Help [module]`

To list available subcommands type:

`BookGen SubCommands`

General arguments:

* `-wd` or `--wait-debugger`

  Waits for a debugger to be attached. Usefull for error reporting & error finding.

* `-ad` or `--attach-debugger`
  
  Attaches a debugger. Usefull for error reporting & error finding.

* `-js` or `--json-log`

  Outputs log in JSON format. Usefull for interop purposes.

# Addfrontmatter

Add a basic YAML frontmatter information to all markdown files located in the current folder 
and it's subfolders.

```
BookGen Addfrontmatter [-v] [-d [directory]]
BookGen Addfrontmatter [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues


# Assembly-document

Generates a markdown file(s) from a given .NET assembly and it's XML documentation file.

```
BookGen Assembly-document -i <input> -o <output> [-d] [-n]
BookGen Assembly-document --input <input> --output <output> [--dry] [--namespace-pages]
```

Arguments:

* `-i`, `--input`:

  Required argument. Specifies the input assembly file path. The file must be a .NET assembly.

* `-o`, `--output`:

  Required argument. Specifies the output files path.

* `-d`, `--dry`:

  Optional argument. If specified, the command will not write any files, but will only print the output to console.

* `-n`, `--namespace-pages`:

  Optional argument. If specified, the command will create a separate markdown file for each namespace in the assembly.

# BuildEpub

Build an epub3 file from the book.

```
BookGen BuildEpub -o <output> [-v] [-d [directory]]
BookGen BuildEpub --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# BuildExport

Build a JSON file with schema for post processing of the book.

```
BookGen BuildExport -o <output> [-v] [-d [directory]]
BookGen BuildExport --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-h`, `--host`:

  Optional argument. If specified, the host name set in the config file will be ignored and the host name will be set to the specified value.

# BuildFeed

Build an RSS 2.0 and an Atom 1.0 feed from the book.

```
BookGen BuildFeed -o <output> [-v] [-d [directory]]
BookGen BuildFeed --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# BuildPrint

Build a printable html & xhtml file from the book

```
BookGen BuildPrint -o <output> [-v] [-d [directory]]
BookGen BuildPrint --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-h`, `--host`:

  Optional argument. If specified, the host name set in the config file will be ignored and the host name will be set to the specified value.

# BuildWeb

Build a static website from the book

```
BookGen BuildWeb -o <output> [-v] [-d [directory]]
BookGen BuildWeb --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-h`, `--host`:

  Optional argument. If specified, the host name set in the config file will be ignored and the host name will be set to the specified value.

# BuildWp

Build a wordpress export file from the book.

```
BookGen BuildWp -o <output> [-v] [-d [directory]]
BookGen BuildWp --output <output> [--verbose] [--dir [directory]]
```

Arguments:

* `-o`, `--output`:

  Required argument. Specifies the output directory name.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will
    be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-h`, `--host`:


  Optional argument. If specified, the host name set in the config file will be ignored and the host name will be set to the specified value.

# Config

Get or set bookgen application specific settings

* `BookGen Config`

  List all currently supported application wide settings

* `BookGen Config <key>`

  Gets a setting value, prints it to output and exits.

* `BookGen Config <key> <value>`

  Sets a setting value and exits

# Edit

Open a file for editing with configured editor.

`BookGen edit [filename]`

# Gui

Starts the program with a command line gui interface

```
BookGen Gui [-v] [-d [directory]]
BookGen Gui [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues.

# Html2Pdf

Converts a HTML file to a png using edges or chromes headless mode. The tool will use chrome, 
if it's installed, otherwise it will use edge. This command is only supported on Windows OS.

```
BookGen Html2Pdf -i <input> -o <output>
BookGen Html2Pdf --input <input> --output <output>
```

* `-i`, `--input`:

  Input html file with extension of .htm or .html

* `-o`, `--output`:

  Output PDF file.

# Html2Png

Converts a HTML file to a png using edges or chromes headless mode. The tool will use chrome, 
if it's installed, otherwise it will use edge. This command is only supported on Windows OS.

```
BookGen Html2Png -i <input> -o <output> [-w [width]] [-h [height]]
BookGen Html2Png --input <input> --output <output> [--width [width]] [--height [height]]
```

* `-i`, `--input`:

  Input html file with extension of .htm or .html

* `-o`, `--output`:

  Output PNG file.

* `-w`, `--width`:

  Optional argument. Specifies the width of the output image in pixels.

* `-h`, `--height`:

  Optional argument. Specifies the height of the output image in pixels.

# ImgConvert

Converts an image file to a different format. The tool supports png, jpeg, webp and svg formats.

```
BookGen ImgConvert -i <input> -o <output> -f <format> [-q [quality]] [-r [resolution]]
```

Arguments:

* `-i`, `--input`:

  Required argument. Specifies the input image file path. The file must be a valid image file or a directory containing image files.

* `-o`, `--output`:

  Required argument. Specifies the output file path. The file must have a valid image file  extension, like .png, .jpg, .jpeg, .webp. Can also be a directory, in which case the output files will be saved with the same name as the input files

* `-f`, `--format`:

    Required argument. Specifies the output image format. Supported formats are png, jpeg, webp.

* `-q`, `--quality`:

  Optional argument. Specifies the quality of the output image. The value must be between 0 and 100. Default is 90. If not specified, then the default value will be used.

* `-r`, `--resolution`:

  Optional argument. Specifies the resolution of the output image. The value must be a valid resolution string, like 1920x1080 or 1280x720. If not specified, then the resolution will be the same as the input image.

# Install

Windows only command that installs BookGen to the system PATH & optionally to the windows terminal.

```
BookGen Install
```

# JsonArgs

Creates an empty json arguments template file for a given bookgen command.

```
BookGen JsonArgs -c <command> [-d [directory]]
BookGen JsonArgs --command <command> [--dir [directory]]
```

A Json arguments template can be used to store command line arguments, so the bookgen command can be invoked with the same arguments without having to type them in again.

Arguments:

* `-c`, `--command`:

  Required argument. Specifies the command for which the json template will be created.

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

# Links

Scans all markdown files in the current book and writes the lins to a markdown file, named links.md

```
BookGen Links [-vf] [-v] [-d [directory]]
BookGen Links [--verify] [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 
  
  Optional argument, turns on detailed logging. Usefull for locating issues

* `-vf`, `--verify`:

  Optional argument. When specified, the command will verify if the links are accessible and will print the result to the console. If not specified, the command will only write the links to the output file.

# Math2Svg

Renders a single markdown file containing Tex formulas to svg files

```
BookGen Math2Svg -f <formula> -o <output.svg> [-s [scale]]
BookGen Math2Svg --formula <formula> --output <output.svg> [--scale [scale]]
```

Arguments:

* `-f`, `--formula`: 

  Formula to render to svg. The formula must be a valid Tex formula.

* `-o`, `--output`: 

  Output svg file.

* `-s`, `--scale`: 

  Optional argument. Specifies the scale of the output svg file. Default is 1.0. If not specified, then the default value will be used.

# Md2HTML

Renders a single markdown file to an HTML file

```
BookGen Md2HTML -i <input.md> -o <output.html>
BookGen Md2HTML --input <input.md> --output <output.html>
```

Arguments:

* `-i`, `--input`: 

  Input markdown file path. Multiple files can be set with multiple `-i` arguments

* `-o`, `--output`: 

  Output html file path. If file name is "-", outputs to console.

* `-tf`, `--template`

  Optional argument. If not specified, default template is used. If custom file provided, then the file must contain the folloing tags:

  * `<!--{title}-->` - For document title
  * `<!--{content}-->` - For document content

* `-ns`, `--no-syntax`

  Optional argument. Disables syntax highlighting.

* `-ne`, `--no-embed`

  Optional argument. Disables embedding of media site links, like youtube.

* `-r`, `--raw`

  Optional argument. Disables full html generation, only outputs the html produced by the markdown formatting.

* `-s`, `--svg`

  Enables SVG Passthrough. When enabled SVG files will be embedded in resulting html, instead of being rendered to webp.

* `-t`, `--title`

  Optional argument. Specifies the rendered HTML page title. Only has affect, when `-r` or `--raw` is not specified.

# Md2terminal

Converts a markdown file to terminal formatted text.

```
BookGen Md2terminal -i <input.md> -o <output.html>
BookGen Md2terminal --input <input.md> --output <output.html>
```

* `-i`, `--input`: 

  Input markdown file path. Multiple files can be set with multiple `-i` arguments

* `-o`, `--output`: 

  Output html file path. If file name is "-", outputs to console.

# Migrate

Migrate an old Bookgen book to the new format.

```
BookGen Migrate [-v] [-d [directory]]
BookGen Migrate [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# NewBook

Creates a new book structure in the given folder

```
BookGen NewBook [-v] [-d [directory]]
BookGen NewBook [--verbose] [-dir [directory]]
```

Arguments:

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

# NewPage

Creates a new markdown page.

```
BookGen NewPage -n <file> [-v] [-d [directory]]
BookGen NewPage --name <file> [--verbose] [-dir [directory]]
```

Arguments:

* `-n`, `--name`:

  File name. Specifies new file name

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

# QrCode

Renders an url into a QRCode image

```
BookGen QrCode -d <url> -o <output> [-c [color]]
BookGen QrCode --data <url> --output <output> [--color [color]]
```

Arguments:

* `-d`, `--data`: 

  Url data to encode. Minimum 1 byte, Maximum 900 bytes

* `-c`, `--color`: 

  Optional argument. Specifies the color of the QRCode.  The color must be a valid hex color code, like #FF0000 or #F00.

* `-o`, `--output`

  Output file. Must have .png or .svg extension

# Schemas

Creates a schemas.md documentation file, describing the various config schemas used by bookgen.

```
BookGen Schemas [-v] [-d [directory]]
BookGen Schemas [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# Serve

Starts a local only http server that serves file from the given directory

```
BookGen Serve [-d [directory]]
BookGen Serve [--dir [directory]]
```
    
Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

# Shell

Autocompleter command, that is used by Powershell

```
BookGen Shell
```

# Shortcut

Create a cmd file in the current directory that can be used to start the bookgen Shell in the current directory.

```
BookGen Shortcut
```

This command is only supported on Windows OS.

# Spellcheck

Perform spell check on a given markdown file or text file. The command will print the misspelled words to the console.

```
BookGen Spellcheck -i <input> [-l <language>] [-v]
BookGen Spellcheck --input <input> [--language <language>] [--verbose]
```

Arguments:

* `-i`, `--input`:
  
  Required argument. Specifies the input file path. The file must be a markdown file or a text file.

* `-l`, `--language`:

  Optional argument. Specifies the language to use for spell checking. The value must be a valid language code, like en_US or hu_HU. If not specified, then en_US will be used as the default language.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

* `-ld`, `--list-dictionaires`

  Optional argument. When specified, the command will list all available dictionaries and exit.

# Stats

Displays various statistics about the bookgen project.

```
BookGen Stats [-v] [-d [directory]]
BookGen Stats [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# Subcommands

Listst all available subcommands

```
BookGen SubCommands
```

# Templates

Lists all available templates, or extracts a single template to the current directory.

```
Bookgen Templates [-n [template name]]
Bookgen Templates [--name [template name]]
```

Arguments:

* `-n`, `--name`:

  Optional argument. If specified, only the template with the given name will be extracted. If not specified, all available templates will be printed.

# Terminalinstall

Installs a bookgen profile to the Windows Termninal.

This command is only supported on Windows OS. Without arguments, performs terminal profile install.

```
BookGen Terminalinstall [-c] [-t]
BookGen Terminalinstall [--checkinstall] [--checkterminalinstall]
```

Arguments:

* `-c`, `--checkinstall`:

  Optional argument. When specified checks, if terminal profile installed or not. If exit code is 0, profile is installed.

* `-t`, `--checkterminalinstall`:

  Optional argument. When specified checks, if windows terminal is installed or not. If exit code is 0, terminal is installed.

# Tools

Display a list of downloadable tools that can be installed and used with BookGen shell. 

This command is only supported on Windows OS.

```
BookGen Tools
```

# Upgrade

Upgrades the bookgen project to the latest version. This command will upgrade the bookgen.json 
config file to the latest version, and will also upgrade the bookgen.toc.json file to the
latest version.

```
BookGen Upgrade [-v] [-d [directory]]
BookGen Upgrade [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues


# Validate

Validate the configuration files used by bookgen in the specified folder.

```
BookGen Validate [-v] [-d [directory]]
BookGen Validate [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues

# Version

Print the current program and config API version

`BookGen Version`

# Vstasks

Generates a Visual Studio Code tasks.json file for the bookgen project.

```
BookGen Vstasks [-v] [-d [directory]]
BookGen Vstasks [--verbose] [--dir [directory]]
```

Arguments:

* `-d`, `--dir`:

  Optional argument. Specifies work directory. If not specified, then the current directory will be used as working directory.

* `-v`, `--verbose`: 

  Optional argument, turns on detailed logging. Usefull for locating issues
