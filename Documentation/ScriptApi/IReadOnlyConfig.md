# IReadOnlyConfig Interface

Namespace: BookGen.Api.Configuration

 Provides acces the current configuration 

## Properties

* `String HostName { get;  }`
     Host name 

* `String ImageDir { get;  }`
     Images directory 

* `String Index { get;  }`
     Index file 

* `Int64 InlineImageSizeLimit { get;  }`
     Inline image size in bytes 

* `Boolean LinksOutSideOfHostOpenNewTab { get;  }`
     Links that do not target the HostName open in new tabs 

* `IReadOnlyMetadata Metadata { get;  }`
     Metadata information 

* `String ScriptsDirectory { get;  }`
     Scripts folder 

* `IReadOnlyBuildConfig TargetEpub { get;  }`
     Build configuration for epubs 

* `IReadOnlyBuildConfig TargetPrint { get;  }`
     Build configuration for Printing 

* `IReadOnlyBuildConfig TargetWeb { get;  }`
     Build configuration for static website 

* `IReadOnlyBuildConfig TargetWordpress { get;  }`
     Build configuration for Wordpress export 

* `String TOCFile { get;  }`
     Table of contents file 

* `IReadOnlyTranslations Translations { get;  }`
     Translations 

* `Int32 Version { get;  }`
     Config file version 

