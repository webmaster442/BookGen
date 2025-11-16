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

# Markdown files

Markdown files are the main input format for BookGen. They can contain text, images, links, and other elements. BookGen supports github flavored markdown, which means it supports additional features like tables, task lists, and more. Front matter is supported via YAML syntax. The front matter is used to store metadata about the document, such as title, tags, and other properties.

To create a new page execute the command: `BookGen newpage -n test.md`. This will create a new markdown file named `test.md` in the current working directory. You can then edit this file with your favorite text editor. It will contain a basic template with the YAML front matter and a placeholder for the content.

# Templates

Template tags use a mustache like syntax startiong with the `{{` symbols and ending with `}}`. Tags can include simple properties or functions. Function and property names are case insensitive.

There are a few special properties that have special meaning. These are:

* `{{Title}}` - Title of the acual page
* `{{Content}}` - Content of the actual page
* `{{Host}}` - Host url, set in the configuration file

The `{{content}}` mustache tag is a special placeholder used to represent the markdown content within a file. **Important:** A markdown document must not include the `{{content}}` tag within its own content. Including this tag will cause infinite recursion during rendering, resulting in a failure to render the document.

The `title` attribute comes from the YAML front matter of the document. The YAML front matter must include a `title` and a `tags` property. The Title is the document title and the tags are a comma sepperated string of keywords that can be set for the metadata.

You can also add additional data to the YAML front matter. The YAML front matter via the `Data` property, which is a Dictionary.  For example, adding:

```yaml
Data:
  foo: bar
```
allows you to access this value in templates or markdown content using the mustache syntax `{{foo}}`. All property names are case insensitive in this case too, when accessing the front matter data.

The `LastModified` time stamp is determined from the input file.


# Template functions

* `{{BuildDate(format)}}` - Actual build date. Format is optional, default is `yyyy-MM-dd HH:mm:ss`.
* `{{JSPageToc(source, target)}}` - Generates a JavaScript table of contents from the source div's headdings and displays it to the target div.

