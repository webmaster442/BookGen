![logo](Branding/icon-bookgen.webp)

# BookGen - A C# documentation generator

BookGen is a command line toolchain for writing books and documentation in markdown. It was inspired by GitBook. Generally speaking you can call it a static website generator, but it has some neat features compared to other products.

It is designed to be cross platform, but It's developed and tested under Windows. It features a web based, cross platform Markdown editor.

* MIT Licensed
* Written in C#, targets .NET 10.0
* Extremely fast, compared to GitBook and other NodeJs stuff
* It hasn't got a template engine, so you don't have to learn a new template language. However it's extendable via shortcodes, like wordpress
* Can be extended with Scripting API
* Can be extended with NodeJs and Python Scripts
* Should be cross platform, but I mainly test and use it on Windows
* Can resize & convert pictures during build.
* Syntax highlighting is rendered during compile for Epub and printable documents.
* Many output formats: 
    * Static website
    * Printable or Word processor importable plain XHTML
    * EPub v. 3.0
    * Wordpress compatible XML export file
    * RSS and Atom feeds
    * Word OpenXML (*.docx) document


# Build instructions

```bash
git clone https://github.com/webmaster442/BookGen.git
.\download-assets.ps1
dotnet build
```
## Documentation

https://github.com/webmaster442/BookGen/wiki
