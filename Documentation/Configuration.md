# Configuration

BoookGen uses a json based configuration file. A default configuration file can be created in gui mode in the current working directory.

The Configuration is divided into Sections.

## General options

General options can be found at the beginning of the configuration. The follwing 

```js
  //Table of contents markdown file. Will be used to reference chapters
  "TOCFile": "",
  //Path to a directory that contains images that will be used in the book
  "ImageDir": "",
  //Web deploy target hostname. Used to create correct links & include assets
  "HostName": "",
  //First page of the book
  "Index": "",
  //Version information, identifies the format & config version for the tool. DO NOT CHANGE!
  "Version": 100,
  //For web deploy. If enabled than the links that point outside of the HostName will open in a new tab
  "LinksOutSideOfHostOpenNewTab": true,
  //Images that are smaller than the specified size in bytes
  //will be transformed to base64 text and included in the output HTML
  "InlineImageSizeLimit": 51200,
```

## Search Page options

Configures the Search page texts. Only used by the web and test targets.

```js
  "SearchOptions": {
    "SearchPageTitle": "Search",
    "SearchTextBoxText": "Type here to search",
    "SearchButtonText": "Search",
    "SearchResults": "Results",
    "NoResults": "No Results found"
  },
```

## Metadata

Configures basic metadata for web and e-book targets.

```js
  "Metadata": {
    "Author": "Place author name here",
    "CoverImage": "Place cover here",
    "Title": "Book title"
  },
```

## Build Targets

BookGen uses Build targets to individually configure the output formats that the tool supports. Currently there are 3 supported targets:

* TargetWeb : Target for web deploy & test website
* TargetPrint : Creates a huge HTML file that can be imported to word processing tools
* TargetEpub : Creates an epub format version of the book

Each build target consists of the following options:

```js
{
//Path to ouptut directory, relative to source
//Can be also a full path  
    "OutPutDirectory": "Path to output directory",
//Template file.
//If not specified built in version will be used
    "TemplateFile": "",
//List of additional scripts and js and other files
//that are required by your template.
    "TemplateAssets": [
      {
        //Specifies source file
        "Source": "",
        //Specifies target path
        "Target": ""
      }
    ],
//Specifies CSS classes that will be aplied to
//Elements of the Generated HTML
    "StyleClasses": {
      "Heading1": "", //classes for <H1>
      "Heading2": "", //classes for <H2>
      "Heading3": "", //classes for <H3>
      "Image": "", //classes for <image>
      "Table": "", //classes for <table>
      "Blockquote": "", //classes for <blockquoute>
      "Figure": "", ///classes for <figure>
      "FigureCaption": "", //classes for <figurecaption>
      "Link": "", //classes for <a>
      "OrderedList": "", //classes for <ol>
      "UnorederedList": "", //classes for <ul>
      "ListItem": "" //classes for <li>
    }
  },
```

Note: **To have a succesfull build you will need to configure at least one build target!**