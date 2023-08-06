![logo](https://raw.githubusercontent.com/webmaster442/BookGen/master/Branding/bookgen-splash.svg)

# BookGen - A C# documentation generator

BookGen is a command line toolchain for writing books and documentation in markdown. It was inspired by GitBook. Generaly speeking you can call it a static website generator, but it has some neat features compared to other products.

It is designed to be cross platform, but It's developed and tested under Windows. It features a web based, cross platform Markdown editor.

* MIT Licensed
* Written in C#, targets .NET 7.0
* Extremely fast, compared to GitBook and other NodeJs stuff
* It hasn't got a template engine, so you don't have to learn a new template language. However it's extendable via shortcodes, like wordpress
* Can be extended with Scripting API
* Can be extended with NodeJs, Python, Php Scripts
* Should be cross platform (Only tested & developed on windows)
* Can resize & convert pictures during build.
* Syntax highlighting is rendered during compile for Epub and printable documents.
* Many output formats: 
    * Static website with Bootstrap template
    * Printable or Word processor importable plain HTML
    * EPub v. 3.0
    * Wordpress compatible XML export file

To Build and develop you will need:
* Visual Studio 2022 with latest updates - https://visualstudio.microsoft.com/vs/
* .NET 7 SDK - https://dotnet.microsoft.com/download

# Getting the repo

```bash
git clone https://github.com/webmaster442/BookGen.git
```
## Documentation

https://github.com/webmaster442/BookGen/wiki
