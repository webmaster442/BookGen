# Getting started

BookGen installs various programs that can be used to achieve Book generation
and writing tasks with the help of Markdown.

# Installed programs

* BookGen - Main command line program
* BookGen.Shellprog - Shell helper program

# Bookgen shell commands

`cdg` - Graphical change directory. Opens folder browser to set current working directory

`intro` - BookGen shell short intro message

`bookgen-info` - Displays this text

`organize` - Organize files in the current working directory

# Common BookGen commands

`BookGen md2html -i input.md -o out.html` - Convert the input.md file to out.html

`BookGen SubCommands` - Lists all available subcommands

`BookGen Gui` - Start in terminal gui mode. Only available, if folder contains a BookGen project.

# Template variables

* `{{Title}}` - Title of the acual page
* `{{Content}}` - Content of the actual page
* `{{Host}}` - Host url, set in the configuration file

# Template functions

* `{{BuildDate(format)}}` - Actual build date. Format is optional, default is `yyyy-MM-dd HH:mm:ss`.
* `{{JSPageToc(source, target)}}` - Generates a JavaScript table of contents from the source div's headdings and displays it to the target div.