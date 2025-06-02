# Help

BookGen - Markdown to Book tool.

For the tool to work in the work folder there must be a bookgen.json config file.
This config file can be created with the following command:

`BookGen Newbook`

To Get more help about a subcommand type:

`BookGen Help [module]`

To list available subcommands type:

`BookGen SubCommands`

General arguments:

`-wd`
`--wait-debugger`
    Waits for a debugger to be attached. Usefull for error reporting & error finding.

`-ad`
`--attach-debugger`
    Attaches a debugger. Usefull for error reporting & error finding.

`-js`
`--json-log`
    Outputs log in JSON format. Usefull for interop purposes.

# Assembly-document

Generates a markdown file(s) from a given .NET assembly and it's XML documentation file.

`BookGen Assembly-document -i <input> -o <output> [-d] [-n]`
`BookGen Assembly-document --input <input> --output <output> [--dry] [--namespace-pages]`

Arguments:

-i, --input:
    Required argument. Specifies the input assembly file path. The file must be a .NET assembly.

-o, --output:
    Required argument. Specifies the output files path.

-d, --dry:
    Optional argument. If specified, the command will not write any files, 
    but will only print the output to console.

-n, --namespace-pages:
    Optional argument. If specified, the command will create a separate markdown file for each namespace
    in the assembly.

# Addfrontmatter

Add a basic YAML frontmatter information to all markdown files located in the
current folder and it's subfolders.

`BookGen Addfrontmatter [-v] [-d [directory]]`
`BookGen Addfrontmatter [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# BuildPrint

Build a printable html & xhtml file from the book

`BookGen BuildPrint -o <output> [-v] [-d [directory]]`
`BookGen BuildPrint --output <output> [--verbose] [--dir [directory]]`

Arguments:

-o, --output:
    Required argument. Specifies the output file name.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues


# BuildWeb

Build a static website from the book

`BookGen BuildWeb -o <output> [-v] [-d [directory]]`
`BookGen BuildWeb --output <output> [--verbose] [--dir [directory]]`

Arguments:

-o, --output:
    Required argument. Specifies the output file name.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# BuildExport

Build a JSON file with schema for post processing of the book.

`BookGen BuildExport -o <output> [-v] [-d [directory]]`
`BookGen BuildExport --output <output> [--verbose] [--dir [directory]]`

Arguments:

-o, --output:
    Required argument. Specifies the output file name.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues


# BuildWp

Build a wordpress export file from the book.

`BookGen BuildWp -o <output> [-v] [-d [directory]]`
`BookGen BuildWp --output <output> [--verbose] [--dir [directory]]`

Arguments:

-o, --output:
    Required argument. Specifies the output file name.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues


# Config

Get or set bookgen application specific settings

`BookGen Config`
    List all currently supported application wide settings

`BookGen Config <key>`
    Gets a setting value, prints it to output and exits.

`BookGen Config <key> <value>`
    Sets a setting value and exits

# Edit

Open a file for editing with configured editor.

`BookGen edit [filename]`

# Externallinks

Collects all external links mentioned in a chapter. Usefull when targeting
printable documentation.

`BookGen ExternalLinks -o <output.md> [-d [directory]`
`BookGen ExternalLinks --output <output.md> [--dir [directory]]`

Arguments:

-o, --output: 
    Output markdown file path.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# Gui

Starts the program with a command line gui interface


`BookGen Gui [-v] [-d [directory]]`
`BookGen Gui [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Html2Pdf

Converts a HTML file to a png using edges or chromes headless mode.
The tool will use chrome, if it's installed, otherwise it will use edge.

`BookGen Html2Pdf -i <input> -o <output>`
`BookGen Html2Pdf --input <input> --output <output>`

-i, --input:
    Input html file with extension of .htm or .html

-o, --output:
    Output PDF file.

# Html2Png

Converts a HTML file to a png using edges or chromes headless mode.
The tool will use chrome, if it's installed, otherwise it will use edge.

`BookGen Html2Png -i <input> -o <output> [-w [width]] [-h [height]]`
`BookGen Html2Png --input <input> --output <output> [--width [width]] [--height [height]]`

-i, --input:
    Input html file with extension of .htm or .html

-o, --output:
    Output PNG file.

-w, --width:
    Optional argument. Specifies the width of the output image in pixels.

-h, --height:
    Optional argument. Specifies the height of the output image in pixels.

# JsonArgs

Creates an empty json arguments template file for a given bookgen command.

`BookGen JsonArgs -c <command> [-d [directory]]`
`BookGen JsonArgs --command <command> [--dir [directory]]`

A Json arguments template can be used to store command line arguments,
so the bookgen command can be invoked with the same arguments without having 
to type them in again.

Arguments:

-c, --command:
    Required argument. Specifies the command for which the json template
    will be created.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# NewPage

Creates a new markdown page.

`BookGen NewPage -n <file> [-v] [-d [directory]]`
`BookGen NewPage --name <file> [--verbose] [-dir [directory]]` 

Arguments:

-n, --name:
    File name. Specifies new file name

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# NewBook

Creates a new book structure in the given folder

`BookGen NewBook [-v] [-d [directory]]` 
`BookGen NewBook [--verbose] [-dir [directory]]` 

Arguments:

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# Math2Svg

Renders a single markdown file containing Tex formulas to svg files

`BookGen Math2Svg -i <input.txt> -o <output dir>`
`BookGen Math2Svg --input <input.txt> --output <output dir>`

Arguments:

-i, --input: 
    Input file containing tex formulas

-o, --output: 
    Output directory path

Note: This module to work requires internet connection.

# Migrate

Migrate an old Bookgen book to the new format.

`BookGen Migrate [-v] [-d [directory]]`
`BookGen Migrate [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Md2HTML

Renders a single markdown file to an HTML file

`BookGen Md2HTML -i <input.md> -o <output.html>`
`BookGen Md2HTML --input <input.md> --output <output.html>`

Arguments:

-i, --input: 
    Input markdown file path. Multiple files can be set with multiple
    -i arguments

-o, --output: 
    Output html file path. If file name is "con", outputs to console.

-tf, --template
    Optional argument. If not specified, default template is used.
    If custom file provided, then the file must contain the folloing
    tags:
    `<!--{title}-->` - For document title
    `<!--{content}-->` - For document content

-ns, --no-syntax
    Optional argument. Disables syntax highlighting.

-r, --raw
    Optional argument. Disables full html generation, only outputs
    the html produced by the markdown formatting.

-s, --svg
    Enables SVG Passthrough. When enabled SVG files will be
    embedded in resulting html, instead of being rendered to
    webp.

-t, --title
    Optional argument. Specifies the rendered HTML page title.
    Only has affect, when -r or --raw is not specified.


# QrCode

Renders an url into a QRCode image

`BookGen QrCode -d <url> -s <size> -o <output>`
`BookGen QrCode --data <url> --size <size> --output <output>`

Arguments:

-d, --data: 
    Url data to encode. Minimum 1 byte, Maximum 900 bytes

-s, --size: 
    Image size in pixels. Output image is square. 
    Minimum 10 , maximum 1000

-o, --output
    Output file. Must have .png or .svg extension

Note: This module to work requires internet connection.

# Serve

Starts a local only http server that serves file from the given directory

`BookGen Serve [-d [directory]]`
`BookGen Serve [--dir [directory]]`
    
Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# Terminalinstall

Installs a bookgen profile to the Windows Termninal

`BookGen Terminalinstall [-c] [-t]`
`BookGen Terminalinstall [--checkinstall] [--checkterminalinstall]`

Arguments:

-c, --checkinstall:
    Optional argument. When specified checks, if terminal profile installed or not.
    If exit code is 0, profile is installed.

-t, --checkterminalinstall:
    Optional argument. When specified checks, if windows terminal is installed or not.
    If exit code is 0, terminal is installed.

Without arguments, performs terminal profile install.

# Version

Print the current program and config API version

`BookGen Version`

# Shell

Autocompleter command, that is used by Powershell

# Subcommands

Listst all available subcommands

# Schemas

Creates a schemas.md documentation file, describing
the various config schemas used by bookgen.

`BookGen Schemas [-v] [-d [directory]]`
`BookGen Schemas [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Stats

Displays various statistics about the bookgen project.

`BookGen Stats [-v] [-d [directory]]`
`BookGen Stats [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues


# Validate

Validate the configuration files used by bookgen in the specified folder.

`BookGen Validate [-v] [-d [directory]]`
`BookGen Validate [--verbose] [--dir [directory]]`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues
