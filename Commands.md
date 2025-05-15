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

Converts a HTML file to a png using edges or chromes headless mode.
The tool will use chrome, if it's installed, otherwise it will use edge.

`BookGen Html2Pdf -i [input] -o [output]`
`BookGen Html2Pdf --input [input] --output [output]`

-i, --input:
    Input html file with extension of .htm or .html

-o, --output:
    Output PDF file.

# Html2Png

Converts a HTML file to a png using edges or chromes headless mode.
The tool will use chrome, if it's installed, otherwise it will use edge.

`BookGen Html2Png -i [input] -o [output] {-w [width]} {-h [height]}`
`BookGen Html2Png --input [input] --output [output] {--width [width]} {--height [height]}`

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

# NewPage

Creates a new markdown page.

`BookGen NewPage -n [file] {-d [directory]}` 
`BookGen NewPage --name [file] {-dir [directory]}` 

Arguments:

-n, --name:
    File name. Specifies new file name

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

# Version

Print the current program and config API version

`BookGen Version`

# Shell

Autocompleter command, that is used by Powershell

# Subcommands

Listst all available subcommands