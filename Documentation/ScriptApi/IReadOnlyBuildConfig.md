# IReadOnlyBuildConfig Interface

Namespace: BookGen.Api.Configuration

 Current build configuration 

## Properties

* `String OutPutDirectory { get;  }`
     Config output directory 

* `String TemplateFile { get;  }`
     Template file path 

* `IReadOnlyList<IReadOnlyAsset> TemplateAssets { get;  }`
     List of required assets 

* `IReadOnylStyleClasses StyleClasses { get;  }`
     Additional style classes that will be aplied 

* `IReadOnlyTemplateOptions TemplateOptions { get;  }`
     Additional template options 

