---
title: Plugin tutorial
tags: ''
---

# Bookgen plugin tutorial

BookGen supports plugins that can hook into the book building process, allowing
you to extend and customize how your book is generated.

## Requirements

Plugins must target **.NET Standard 2.1**. This ensures that your plugin is
compatible with the BookGen runtime and can be loaded during the build.

## Packaging

A plugin is distributed as a ZIP file that uses the `.plugin` file extension.
The archive must contain the following:

- The plugin **DLL** file (the entry assembly).
- Any **dependencies** that the plugin needs in order to run.
- A **`manifest.json`** file, placed in the root of the archive, that describes
  the plugin.

## The manifest.json file

The `manifest.json` file describes the plugin and tells BookGen which assembly
to load. It contains the following properties:

| Property        | Required | Description                                                        |
| --------------- | -------- | ------------------------------------------------------------------ |
| `entryAssembly` | Yes      | The name of the plugin DLL file (must have a `.dll` extension).    |
| `author`        | Yes      | The author of the plugin.                                          |
| `description`   | Yes      | A short description of what the plugin does.                       |
| `apiVersion`    | Yes      | The plugin API version. Must be a valid version with major `1`.    |
| `url`           | No       | An optional URL with more information about the plugin.            |

### Example

```json
{
    "entryAssembly": "MyPlugin.dll",
    "author": "Jane Doe",
    "description": "An example plugin that customizes the build process.",
    "apiVersion": "1.0.0",
    "url": "https://example.com/my-plugin"
}
```

### JSON Schema for validating the manifest.json file:

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "PackageManifest",
  "description": "Manifest describing a BookGen plugin package.",
  "type": "object",
  "additionalProperties": false,
  "required": [
    "entryAssembly",
    "description",
    "apiVersion",
    "author"
  ],
  "properties": {
    "entryAssembly": {
      "type": "string",
      "description": "The plugin entry assembly. Must have a .dll extension.",
      "minLength": 1,
      "pattern": "(?i)\\.dll$"
    },
    "author": {
      "type": "string",
      "description": "The author of the plugin.",
      "minLength": 1
    },
    "description": {
      "type": "string",
      "description": "A human readable description of the plugin.",
      "minLength": 1
    },
    "apiVersion": {
      "type": "string",
      "description": "The API version. Must be a valid version string with a major version of 1.",
      "minLength": 1,
      "pattern": "^1(\\.\\d+){0,3}$"
    },
    "url": {
      "type": "string",
      "description": "A URL to the plugin's homepage or repository.",
      "minLength": 1,
      "format": "uri"
    }
  },
  "additionalItems": false
}
```

## Project structure & setup

All of the types that make up the plugin API (the interfaces, base classes and
data types you hook into) live in the **`BookGen.Api`** assembly. In order to
use the plugin API, your project **must reference `BookGen.Api`**. Without this
reference the plugin API types are not available and your plugin cannot be
compiled against them.

You can add this as a reference to your project by either cloning the BookGen 
repository and referencing the `BookGen.Api` project, or by adding a DLL
reference to the `BookGen.Api.dll` file that is included in the BookGen 
distribution.

NuGet package for the `BookGen.Api` assembly is **not yet available**, 
since the plugin API isn't mature enough to be published as a NuGet package.

A simple plugin project structure looks like this:

```markup
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- Bookgen plugins use netstandard 2.1 -->
    <TargetFramework>netstandard2.1</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <!-- To use new language features and not limit to just C# 8.0 -->
    <LangVersion>latest</LangVersion>
    <!-- Important for plugin loadability -->
    <EnableDynamicLoading>true</EnableDynamicLoading>
  </PropertyGroup>
  <ItemGroup>
    <EmbeddedResource Include="Template.html" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\BookGen.Api\BookGen.Api.csproj">
      <Private>false</Private>
      <ExcludeAssets>runtime</ExcludeAssets>
    </ProjectReference>
  </ItemGroup>
  <ItemGroup>
    <None Update="manifest.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
  <Target Name="PackagePlugin" AfterTargets="Build">
    <PropertyGroup>
      <PluginPackage>$(OutputPath)..\$(AssemblyName).plugin</PluginPackage>
    </PropertyGroup>
    <ZipDirectory Overwrite="true" SourceDirectory="$(TargetDir)" DestinationFile="$(PluginPackage)" />
  </Target>
</Project>
```

The `TargetFramework` is set to `netstandard2.1` to ensure compatibility with the BookGen runtime. 
The `EnableDynamicLoading` property is set to `true` to allow the plugin to be loaded dynamically
at runtime and **this is required for all BookGen plugins.**

`LangVersion` is set to `latest` to allow the use of the latest C# language features, which can be useful for
plugin development, but it's not strictly required. You can set it to a specific version if you prefer. 

**Please note that even if you set `LangVersion` to `latest` there might be language features that will not compile,
because the `.netstandard2.1` target framework does not support all the latest C# features.
This is a limitation of the target framework, not the `LangVersion` setting.**

When referencing the `BookGen.Api` project, the `Private` property is set to `false` to prevent the API assembly 
from being copied to the output directory. The `ExcludeAssets` property is set to `runtime` to exclude the runtime 
assets of the API assembly from the plugin package, as they are not needed for the plugin to function.
These settings ensure that the plugin package only contains the necessary files for the plugin to run, without
including unnecessary dependencies.

The `manifest.json` file is included in the project and set to copy to the output directory,
so it will be included in the plugin package.

The `PackagePlugin` target is defined to create the plugin package after the build process. 
It uses the `ZipDirectory` task to create a ZIP file with the `.plugin` extension, containing
the plugin DLL and any dependencies. This way when you build the plugin project, 
it will automatically create a `.plugin` file in the output directory.

## Plugin API

The main entry point for a plugin is the `IBookPlugin` interface. A plugin assembly must contain only one class 
that implements this interface. The class must have a public parameterless constructor, so that BookGen can
instantiate it when loading the plugin.

If the plugin assembly contains more than one class that implements `IBookPlugin`, BookGen will not load the plugin.
