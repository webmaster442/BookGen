# Help

BookGen - Markdown to Book tool.

For the tool to work in the work folder there must be a bookgen.json config file.
This config file can be created with the following command:

`BookGen Init`

To get information regarding the configuration file bookgen.json file run:

`Bookgen ConfigHelp`

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

# AssemblyDocument

Creates markdown documentation from an assembly and it's xml documentation.

`BookGen AssemblyDocument -a [assembly.dll] -o [output directory] {-s}`
`BookGen AssemblyDocument --assembly [assembly.dll] --output [output directory] {--singlepage}`

Arguments:

-a, --assembly:
    Input assembly file (dll). The XML documentation has to be located next to this file.

-o, --output:
    Specifies the output directory. Each tipe will be written to a
    sepperate .md file

-s, --singlepage
    Uses the Single page generator. This generator doesn't support as much features
    as the normal one, it's more like an experimental feature.

# Build

Build project to an output format

`BookGen Build -a [action] {-v} {-d [directory]} {-n}`
`BookGen Build --action [action] {--verbose} {--dir [directory]} {--nowait}`

Arguments:

-a, --action: 
    Specifies the build action. See below.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

-n, --nowait:
    Optional argument, when specified & the program is fisihed,
    then it immediately exits, without key press.

Build Actions:

# ConfigHelp

Prints the currently available config options

# Download

Download a file from the network


`BookGen Download [url] {-v} {-d [directory]}`
`BookGen Download [url] {--verbose} {--dir [directory]}`

Parameters:

url: The URL to download to the directory speciifed by the directory
argument. The result file name will be determined from the url.

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Edit

Open a file for editing with configured editor.

`BookGen edit [filename]`

# Externallinks

Collects all external links mentioned in a chapter. Usefull when targeting
printable documentation.

`BookGen ExternalLinks -o [output.md] {-d [directory]}`
`BookGen ExternalLinks --output [output.md] {--dir [directory]}`

Arguments:

-o, --output: 
    Output markdown file path.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# Gui

Starts the program with a command line gui interface


`BookGen Gui {-v} {-d [directory]}`
`BookGen Gui {--verbose} {--dir [directory]}`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Html2Pdf

Converts a HTML file to a printable PDF using edges headless mode.

`BookGen Html2Pdf -i [input] -o [output]`
`BookGen Html2Pdf --input [input] --output [output]`

-i, --input:
    Input html file with extension of .htm or .html

-o, --output:
    Output PDF file.

# ImgConvert

Converts images from a format to an other format


`BookGen ImgConvert -i [input] -o [output]  {-f [format]} {-q [quality]} {-w [width]} {-h [height]}`
`BookGen ImgConvert --input [input] --output [output] {--format [format]} {--quality [quality]} {--width [width]} {--hegiht [height]}`

Arguments:

-i, --input:
    Input file or Directory.

-o, --output:
    Output file or directory. 
    If input is a directory, then output must be a directory too.
    If input is a file, then input must be a file too.
    In directory mode the -f or --format argument is nesceccary. 

-f, --format:
    Optional argument. Specifies or overrides output format

-w, --width
    Optional argument. Specifies output file max width in pixels.

-h, --height
    Optional argument. Specifies output file max height in pixels.

Supported formats: jpg, jpeg, png, webp, gif

# Init

Initializes a folder as BookGen project

`BookGen Init {-d [directory]}`
`BookGen Init {--dir [directory]}`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# JsonArgs

Creates an empty json arguments template file for a given bookgen command.

`BookGen JsonArgs -c [command] {-d [directory]}`
`BookGen JsonArgs --command [command] {--dir [directory]}`

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

# Math2Svg

Renders a single markdown file containing Tex formulas to svg files

`BookGen Math2Svg -i [input.txt] -o [output dir]`
`BookGen Math2Svg --input [input.txt] --output [output dir]`

Arguments:

-i, --input: 
    Input file containing tex formulas

-o, --output: 
    Output directory path

Note: This module to work requires internet connection.

# Md2HTML

Renders a single markdown file to an HTML file

`BookGen Md2HTML -i [input.md] -o [output.html] {-c [cssfile.css]}`
`BookGen Md2HTML --input [input.md] --output [output.html] {--css [cssfile.css]}`

Arguments:

-i, --input: 
    Input markdown file path

-o, --output: 
    Output html file path. If file name is "con", outputs to console.

-c. --css:
    Optional argument. Specifies the css file to be aplied to the html.
    If not specified built in styles get aplied.

-nc, --no-css
    Optional argument. If specified, built in styles don't get aplied.

-ns, --no-syntax
    Optional argument. Disables syntax highlight

-r, --raw
    Optional argument. Disables full html generation, only outputs
    the html produced by the markdown formatting.

-t, --title
    Optional argument. Specifies the rendered HTML page title.
    Only has affect, when -r or --raw is not specified.

-s, --svg
    Enables SVG Passthrough. When enabled SVG files will be
    embedded in resulting html, instead of being rendered to
    webp.

-tf, --template
    Specify a template html file. The file must contain the folloing
    tags:
    `<!--{title}-->` - For document title
    `<!--{css}-->` - For css contents
    `<!--{content}-->` - For document content

# MdTable

Converts a CSV or Spreadsheet cell range to a markdown table

`BookGen MdTable {-d [delimiter]}`
`BookGen MdTable {--delimiter [delimiter]}`

Arguments:

-d, --delimiter:
    Optional argument. Specifies the delimiter char. By default it assumes
    that the text is a spreadsheet range, which uses tabs. Only needed
    when you want to import CSV.

Note: the command gets the data from the clipboard and the generated
markdown is also written to the clipboard.

# New

Creates a new file with the given template. If no arguments are given, then
it lists the available templates with descriptions.

`Bookgen New {-t [template]} {-o [fileName]}`
`Bookgen New {--template [template]} {--output [fileName]}`

# Pack

Pack / backup the bookgen related files of the current project into a 
single zip file.

`BookGen pack {-v} {-d [directory]} -o [fileName]`
`BookGen pack {--verbose} {--dir [directory]} --output [fileName]`

Arguments:

-o, --output:
    Required. Specifies the destination zip file. Note: if specified 
    file extension differs from zip, then zip will be enforced.

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Preview

Launch a browser that allows markdown file previewing

`BookGen preview {-v} {-d [directory]}`
`BookGen preview {--verbose} {--dir [directory]}`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# ProjectConvert

Converts between configuration JSON and YML formats.


`BookGen ProjectConvert {-d [directory]} {-b}`
`BookGen ProjectConvert {--dir [directory]} {--backup}`
    
Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-b, --backup:
    Optional argument. If given backups the current configuration. If
    the backup allready exists, then it will be simply overwritten.

Note: If the configuration is in json (bookgen.json found) then it will
be converted to yml (bookgen.yml) format.
If the configuration is in yml (bookgen.yml found) then it will
be converted to json (bookgen.json) format.
If both formats exist, then the command exits with a waring.

# QrCode

Renders an url into a QRCode image

`BookGen QrCode -d [url] -s [size] -o [output]`
`BookGen QrCode --data [url] --size [size] --output [output]`

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

`BookGen Serve {-d [directory]}`
`BookGen Serve {--dir [directory]}`
    
Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

# Settings

Get or set bookgen application specific settings

`BookGen Settings list`
    List all currently supported application wide setting

`BookGen Settings get <name>`
    Gets a setting value, prints it to output and exits

`BookGen Settings set <name> <value>`
    Sets a setting value and exits

# Stat

`BookGen Stat {-d [directory]}`
`BookGen Stat {--dir [directory]}`
`BookGen Stat {-i [input.md]}`
`BookGen Stat {-input [input.md]}`

Arguments:

-d, --dir: 
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-i, --input: 
    Input markdown file path

# Tags

Create or update the tags database and display various stats

`BookGen Tags {-v} {-d [directory]} {-a}`
`BookGen Tags {--verbose} {--dir [directory]} {--auto}`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

-a, --auto:
    Auto generate tags from content files. For it to properly work the
    ÿBookLanguageÿ config has to be set correctly for the language of the book

# Tasks

Open the task runner menu

`BookGen Tasks {-v} {-d [directory]} {-c}`
`BookGen Tasks {--verbose} {--dir [directory]} {--create}`

Your bookgen project can contain a tasks.xml file, where you can
add various commands that help with your book building workflow.

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

-c, --create:
    If the tasks.xml file doesn't exist in the project, then it creates a sample
    one and opens the menu. You can edit the file via your editor of choice.
    The sample tasks.xml is designed in a way, to introduce every feature of the 
    task runner.

# Terminalinstall

Installs a bookgen profile to the Windows Termninal

`BookGen Terminalinstall {-c} {-t}`
`BookGen Terminalinstall {--checkinstall} {--checkterminalinstall}`

Arguments:

-c, --checkinstall:
    Optional argument. When specified checks, if terminal profile installed or not.
    If exit code is 0, profile is installed.

-t, --checkterminalinstall:
    Optional argument. When specified checks, if windows terminal is installed or not.
    If exit code is 0, terminal is installed.

Without arguments, performs terminal profile install.

# Toc

Scans the folder and the configuration for markdown conent files
and writes out the missing Table of content entries to a file,
so the Table of conents can be easily updated.

`BookGen Toc {-v} {-d [directory]}`
`BookGen Toc {--verbose} {--dir [directory]}`

Arguments:

-d, --dir:
    Optional argument. Specifies work directory. If not specified, then
    the current directory will be used as working directory.

-v, --verbose: 
    Optional argument, turns on detailed logging. Usefull for locating issues

# Version

Print the current program and config API version

`BookGen Version {-bd} {-api}`
`BookGen Version {--builddate} {--apiversion}`

Arguments:

-bd, --builddate:
    Optional argument. Display only build date

-api, --apiversion:
    Optional argument. Display only API version

# Shell

Autocompleter command, that is used by Powershell

# Subcommands

Listst all available subcommands

# Wiki

Opens the BookGen Wiki page
