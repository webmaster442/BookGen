# Getting started

BookGen installs various programs that can be used to achieve Book generation
and writing tasks with the help of Markdown.

# Installed programs

* BookGen - Main command line program
* BookGen.Launcher - Graphical launcher - Not available on Linux
* BookGen.ShellHelper - Shell helper, used by BookGen shell
* BookGen.Update - Update program - Not available on Linux
* tidy - HTML Tidy, used by BookGen to prettify HTML, can be used as stand alone program

Note: tidy is only bundled with the Windows release. On linux install it with your
package manager or download from: https://www.html-tidy.org/

# Bookgen shell commands

`cdg` - Graphical change directory. Opens folder browser to set current working directory

`intro` - BookGen shell short intro message

`bookgen-info` - Displays this text

`launcher` - Opens the bookgen launcher in the current working directory

`www` - Web starting util for seaching

`organize` - Organize files in the current working directory

# Common BookGen commands

`BookGen md2html -i input.md -o out.html` - Convert the input.md file to out.html

`BookGen SubCommands` - Lists all available subcommands

`BookGen Init` - Initialize folder as a new bookgen project

`BookGen Gui` - Start in terminal gui mode. Only available, if folder contains a BookGen project.
