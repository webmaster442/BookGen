# IScriptHost Interface

Namespace: BookGen.Api

 Interface for accesing the current script runtime 

## Properties

* `String SourceDirectory { get;  }`
     Source directory of input files 

* `IReadOnlyConfig Configuration { get;  }`
     Current configuration in read-only mode. 

* `ITableOfContents TableOfContents { get;  }`
     Currently processed book table of contents 

* `IReadOnlyBuildConfig CurrentBuildConfig { get;  }`
     Currently active build configuration 

* `ILog Log { get;  }`
     Script host log 

