# Getting started

BookGen installs various programs that can be used to achieve Book generation
and writing tasks with the help of Markdown.

# Installed programs

* BookGen - Main command line program
* BookGen.Launcher - Graphical launcher
* BookGen.ShellHelper - Shell helper, used by BookGen shell
* BookGen.Update - Update program
* tidy - HTML Tidy, used by BookGen to prettify HTML, can be used as stand alone program

# Bookgen shell commands

`cdg` - Graphical change directory. Opens folder browser to set current working directory

`intro` - BookGen shell short intro message

`bookgen-info` - Displays this text

`launcher` - Opens the bookgen launcher in the current working directory

# Common BookGen commands

`BookGen md2html -i input.md -o out.html` - Convert the input.md file to out.html

`BookGen SubCommands` - Lists all available subcommands

`BookGen Init` - Initialize folder as a new bookgen project

`BookGen Gui` - Start in terminal gui mode. Only available, if folder contains a BookGen project.
