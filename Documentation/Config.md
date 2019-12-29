# Configuration properties for current version

**TOCFile**
   Type: String
   Can be left unchanged: no
   Table of contents Markdown file

**ImageDir**
   Type: String
   Can be left unchanged: no
   Image directory relative to workdir

**HostName**
   Type: String
   Can be left unchanged: no
   Publish host name. Must include protocoll (http or https) and must end with a /

**Index**
   Type: String
   Can be left unchanged: no
   Index or first page.

**Version**
   Type: Int32
   Can be left unchanged: no
   Config file version. !DO NOT CHANGE!

**LinksOutSideOfHostOpenNewTab**
   Type: Boolean
   Can be left unchanged: no
   If set to true, then links that point out of the given HostName will be opened in a new window/tab

**InlineImageSizeLimit**
   Type: Int64
   Can be left unchanged: no
   Inline images as base64 that are less then this size in bytes. 0 = inlines all files

**Metadata**
   Type: Metadata
   Can be left unchanged: no
   Metadata information for output

**TargetWeb**
   Type: BuildConfig
   Can be left unchanged: no
   Web output configuration

**TargetPrint**
   Type: BuildConfig
   Can be left unchanged: no
   Printable HTML output configuration

**TargetEpub**
   Type: BuildConfig
   Can be left unchanged: no
   e-pub format output configuration

**TargetWordpress**
   Type: BuildConfig
   Can be left unchanged: no
   Wordpress compatible xml output configuration

**Translations**
   Type: Dictionary\<String, String\>
   Can be left unchanged: no
   Translateable strings that can be used in the template

## Properties of type: Metadata
---
**Author**
   Type: String
   Can be left unchanged: no
   Author name

**CoverImage**
   Type: String
   Can be left unchanged: no
   Cover image for social sites, etc..

**Title**
   Type: String
   Can be left unchanged: no
   Title of your book
   
## Properties of type: Asset
---
**Source**
   Type: String
   Can be left unchanged: yes
   path relative to input directory

**Target**
   Type: String
   Can be left unchanged: yes
   path relative to output directory


## Properties of type: StyleClasses
---
**Heading1**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<H1>```

**Heading2**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<H2>```

**Heading3**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<H3>```

**Image**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<IMG>```

**Table**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<TABLE>```

**Blockquote**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<BLOCKQUOTE>```

**Figure**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<FIGURE>```

**FigureCaption**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<FIGCAPTION>```

**Link**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<A>```

**OrderedList**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<OL>```

**UnorederedList**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<UL>```

**ListItem**
   Type: String
   Can be left unchanged: yes
   css classes for: ```<LI>```


## Properties of type: BuildConfig
---
**OutPutDirectory**
   Type: String
   Can be left unchanged: no
   Output directory, relative to work directory

**TemplateFile**
   Type: String
   Can be left unchanged: yes
   HTML template path, relative to work directory. If not specified built in template is used.

**TemplateAssets**
   Type: List\<Asset\>
   Can be left unchanged: yes
   List of assets required by the template

**StyleClasses**
   Type: StyleClasses
   Can be left unchanged: no
   CSS classes that will be aplied to generated html elements

**TemplateOptions**
   Type: Dictionary\<String, String\>
   Can be left unchanged: yes
   Additional template options