# Workflow & Markdown

## Getting started
To use BookGen, first you have to create a few files:
* bookgen.json - Tool configuration file
* summary.md - Table of contents document with links to the book chapters
* index.md -  Start page/Cover page of your book.

The summary.md and index.md file names can be changed, but you have to reference the correct files in the bookgen.json file. The configuration file name (bookgen.json) can't be changed. The tool looks for this file in the specified folder.

You don't have to create these fieles manually. You can use the following command that will start an interactive initialization progress: ```BookGen Build -a Initialize -d {workdir}```

The ```-d {workdir}``` argumeent is optional. If you don't specify the working directory, then the current folder will be used.

## Editing your book

For editing you will need a Text editor of your choice or you can use the BookGen editor to create files for your book. For rendering BookGen uses the Markdig libary that supports GitHub Flavored Markdown https://github.github.com/gfm/ and the following extra options have been turned on:

* Footnotes - https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/FootnotesSpecs.md
* Figures, Footers, Cite - https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/FigureFooterAndCiteSpecs.md
* 

If your are completely new to markdown I suggest reading the official markdown documentation at https://daringfireball.net/projects/markdown/

## Testing

For testing purposes BookGen includes a Built-in HTTP test server. To start a test build and preview your book use the following command: ```BookGen Build -a Test -d {dir}```

## Building

If you are ready with your book you can build an export from it with the following commands:

```
BookGen Build -a BuildEpub -d {dir}
BookGen Build -a BuildPrint -d {dir}
BookGen Build -a BuildWeb -d {dir}
BookGen Build -a BuildWordpress -d {dir}
```

## Templates

BookGen focuses on the content not the looks. But this doesn't mean that you can't style your documents. When you initialize the book you are given the option to extract all the internal templates for customization, or you can specify the template (html) in the config file with the list of required assests. Assets are images, css and js files that are required to make your book work.