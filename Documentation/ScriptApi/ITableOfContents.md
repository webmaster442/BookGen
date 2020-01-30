# ITableOfContents Interface

Namespace: BookGen.Api

 Interface for accesing the table of contents 

## Properties

* `IEnumerable<String> Chapters { get;  }`
     A flat list of chapters without hierarchy 

* `IEnumerable<String> Files { get;  }`
     All files referenced in the Table of Contents 

## Methods

* `IEnumerable`1 GetLinksForChapter( String chapter = );`
     Gets Links for a chapter 
    * `chapter`: chapter name. Can be null. If null, all links returned from the TOC
