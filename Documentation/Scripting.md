# Scripting API

BookGen offeres a C# Scripting API. This makes it possible to create user defined, project specific shortcodes in C#. The Scripts are compiled directly by BookGen, so no additional dependency is required. To develop C# scripts you only need a text editor, but it's highly recomended to use the .NET Core SDK and Visual Studio code for minimal intelisense support.

To use .NET Core SDK and Visual Studio Code for .NET Core development please read the following article: https://code.visualstudio.com/docs/languages/dotnet

## Getting Started

To get started with Scripting it's highly recomended to use the Initialization process of BookGen. It allows creating a sample script and a C# project that can be used for development.

## Script Interface

Every C# script to be recognized as a shortcode must implement the `IScript` interface:

```csharp
    /// <summary>
    /// Interface for scripts.
    /// Every Script must implement this interface.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Script name. Later you can reference the script as a shorcode with this name.
        /// </summary>
        string InvokeName { get; }

        /// <summary>
        /// The main entrypoint of the script. It gets executed when parsing the shortcode.
        /// </summary>
        /// <param name="host">Current script host</param>
        /// <param name="arguments">Arguments for the script</param>
        /// <returns>Markdown string</returns>
        string ScriptMain(IScriptHost host, IArguments arguments);
    }
```

The `InvokeName` Property is the Shortcode name and the `ScriptMain` Method gets executed when a Shortcode is being porocessed. It must return a HTML string. 

The `host` argument provides acces to the Script Host itself. Using this you can acces the currently used Configuration and Table of contents and can use the Log to write additional information to the console when your scrip is executed.

The `arguments` variable contains all arguments passed to your shortcode. Arguments can be accesed through their names.

To be able to use The `˙IScript` interface you must use the `BookGen.Api` namespace that is defined in the `BookGen.Api` assembly. If you plan to develop your own scripts you must add this Assembly as a reference to your development project. Note that this step is automatically done when you create your project with BookGen and enable scripting.

## Demo Script tutorial

Suppose you have created a BookGen project. In the `BookGen.json` file set a directory for Scripts. In that directory create a Script1.cs file Which should have the following content:

```csharp
using BookGen.Api;
using System.Collections.Generic;

namespace Script
{
    public class Script1 : IScript
    {
        public string InvokeName => "Script1";

        public string ScriptMain(IScriptHost host, IArguments arguments)
        {
            host.Log.Detail("Executing Script1...");
            return "<p>Hello from C# Script</p>"
        }
    }
}
``` 

After this you can use the Created Shortcode in any of your md or C# files with the following shortcode:

`<!--{Script1}-->`

If your script contains errors, that prevent it to be compiled, then you can acces the compile errors if you restart booken in verbose output.

### Some details about naming

Scripts are treated as internal shortcodes, they are evaluated by the same engine, but keep in mind that Internal shortcodes have a protected name. This means that if you try to define a shortcode with the ? name, it won't get executed, because this is used for translating internally and you can't define an other purpose for it.