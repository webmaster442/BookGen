# IScript Interface

Namespace: BookGen.Api

 Interface for scripts. Every Script must implement this interface. 

## Properties

* `String InvokeName { get;  }`
     Script name. Later you can reference the script as a shorcode with this name. 

## Methods

* `String ScriptMain( BookGen.Api.IScriptHost host,  BookGen.Api.IArguments arguments);`
     The main entrypoint of the script. It gets executed when parsing the shortcode. 
    * `host`: Current script host
    * `arguments`: Arguments for the script
