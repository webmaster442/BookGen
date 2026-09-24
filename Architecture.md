# BookGen Architecture

## 1. Introduction and Goals

BookGen is a command-line toolchain for generating books and documentation from Markdown sources.

Primary goals:
- Provide reproducible builds for multiple output formats (web, epub, feed, print, WordPress export).
- Keep command handling extensible and testable.
- Support plugin-based custom builds through a stable API contract (`BookGen.Api`, netstandard2.1).
- Offer shell and script automation support for author workflows.

## 2. Architecture Constraints

- Core runtime targets `.NET 11` (`net11.0`) for executables and main libraries.
- Plugin API targets `.NET Standard 2.1` for compatibility (`BookGen.Api`).
- CLI-first architecture with dependency injection (`Microsoft.Extensions.DependencyInjection`).
- Rendering and pipeline logic is in reusable libraries, not in program entry points.
- Plugins are loaded dynamically and isolated via `AssemblyLoadContext`.

## 3. System Scope and Context

### 3.1 Business Context
Book authors and documentation teams use BookGen to transform Markdown + configuration into publishable outputs.

```nomnoml
[Author/CI/CD] -> [BookGen CLI]
[BookGen CLI] -> [Book Sources\n(config.json, toc.json, md files)]
[BookGen CLI] -> [Generated Outputs\n(web, epub, feed, print, wordpress)]
[BookGen CLI] -> [Plugin Package (.plugin/.dll)]
```

### 3.2 Technical Context

```nomnoml
[BookGen (Exe)] -> [BookGen.Cli]
[BookGen (Exe)] -> [BookGen.Lib]
[BookGen (Exe)] -> [BookGen.Vfs]
[BookGen (Exe)] -> [BookGen.Api]
[BookGen (Exe)] -> [BookGen.Shell.Shared]
[BookGen.Shellprog (Exe)] -> [BookGen.Cli]
[BookGen.Shellprog (Exe)] -> [BookGen.Shell.Shared]
[BookGen.Lib] -> [BookGen.Vfs]
[BookGen.SamplePlugin] -> [BookGen.Api]
[Test\nBookgen.Tests] -> [BookGen + Lib + Cli + Shell.Shared]
```

## 4. Solution Strategy

- **Command orchestration layer**: `BookGen` + `BookGen.Cli` provide command discovery, parsing, validation, and execution.
- **Domain and build pipelines**: `BookGen.Lib` encapsulates environment initialization, config validation/upgrades, markdown rendering, and multi-step output pipelines.
- **I/O abstraction**: `BookGen.Vfs` provides scoped filesystem interfaces (`IReadOnlyFileSystem`, `IWritableFileSystem`) used across commands and pipelines.
- **Extensibility**: `BookGen.Api` defines plugin contracts; `BookGen.Infrastructure.Plugins` loads and executes plugin implementations.
- **Shell integration**: `BookGen.Shellprog` and `BookGen.Shell.Shared` support interactive and shell-oriented workflows.

## 5. Building Block View

### 5.1 Level 1
```nomnoml
[BookGen Runtime|
- CLI command execution
- Build command entry points
- DI wiring]
[Library Core|
- BookEnvironment
- Pipeline steps
- Rendering]
[Plugin Surface|
- BookGen.Api contracts
- Plugin loader]
[Infrastructure Support|
- VFS
- Shell helpers
- Packaged assets]

[BookGen Runtime] -> [Library Core]
[BookGen Runtime] -> [Plugin Surface]
[BookGen Runtime] -> [Infrastructure Support]
```

### 5.2 Level 2 (Main executable internals)
```nomnoml
[Program.cs]
[CommandRunner]
[BuildCommandBase + Commands]
[BookEnvironment]
[Pipeline]
[PluginRunner]

[Program.cs] -> [CommandRunner]
[CommandRunner] -> [BuildCommandBase + Commands]
[BuildCommandBase + Commands] -> [BookEnvironment]
[BuildCommandBase + Commands] -> [Pipeline]
[BuildCommandBase + Commands] -> [PluginRunner]
```

### 5.3 Key Project Responsibilities

- `Source/BookGen`: main executable, command implementations, DI composition, plugin invocation.
- `Source/BookGen.Cli`: command framework (`CommandRunner`, command tree, global options, validation).
- `Source/BookGen.Lib`: core domain model, config migration/validation, rendering, pipelines, preview HTTP support.
- `Source/BookGen.Vfs`: file system abstraction with scoped access.
- `Source/BookGen.Api`: stable plugin contracts for external builders.
- `Source/BookGen.Shellprog`: shell helper executable.
- `Source/BookGen.Shell.Shared`: shared shell/logging/browser/process helpers.
- `Source/BookGen.Contents`: distributable bundled content and tool assets.
- `Test/Bookgen.Tests`: NUnit test suite across CLI and command behavior.

## 6. Runtime View

### 6.1 Scenario: `build web`

```nomnoml
[User] -> [BookGen Program]
[BookGen Program] -> [CommandRunner]
[CommandRunner] -> [BuildWebCommand]
[BuildWebCommand] -> [BookEnvironment.Initialize]
[BuildWebCommand] -> [Pipeline.CreateWebPipeLine]
[Pipeline] -> [Steps: CopyAssets -> Render -> Index -> Pager]
[Pipeline] -> [Output Folder]
```

Flow summary:
1. `Program.cs` configures logging and services, then runs `CommandRunner`.
2. `BuildWebCommand` (via `BuildCommandBase`) sets source/output scopes.
3. `BookEnvironment.Initialize` validates config and TOC, applies optional overlay, acquires lock.
4. Web pipeline executes ordered steps from `BookGen.Lib.Pipeline.StaticWebsite`.
5. Generated files are written through `IWritableFileSystem`.

### 6.2 Scenario: `build plugin`

```nomnoml
[User] -> [BuildPlugin Command]
[BuildPlugin Command] -> [PluginPathResolver]
[BuildPlugin Command] -> [BookEnvironment.Initialize]
[BuildPlugin Command] -> [PluginRunner]
[PluginRunner] -> [Extract .plugin + manifest]
[PluginRunner] -> [AssemblyLoadContext]
[AssemblyLoadContext] -> [IBuildPluginV1.Build]
[IBuildPluginV1.Build] -> [Output Folder]
```

Flow summary:
1. Plugin package path is resolved (`.plugin` or `.dll` in dev mode).
2. Environment is initialized like regular builds.
3. Plugin runner validates package manifest and extracts payload.
4. Plugin assembly is loaded in collectible `AssemblyLoadContext`.
5. Exactly one `IBuildPluginV1` implementation is instantiated and executed.
6. Context is unloaded and GC-assisted cleanup is performed.

## 7. Deployment View

BookGen is primarily a local/CI process architecture.

```nomnoml
[Developer Workstation / CI Agent|
- BookGen.exe
- BookGen.Shellprog.exe
- assets.zip / dictionaries.zip
- optional plugins/*.plugin]

[Developer Workstation / CI Agent] -> [File System Workspace|
book sources + config + output]
```

Deployment characteristics:
- No mandatory long-running backend service.
- Outputs are static artifacts suitable for hosting elsewhere.
- Optional preview/server capabilities are hosted in-process when used.

## 8. Cross-cutting Concepts

- **Dependency Injection**: service wiring in executable entry points.
- **Logging**: configurable console/json/file logging with shared providers.
- **Validation**: argument validation in command args + configuration schema/domain validation.
- **Pipeline pattern**: deterministic ordered steps per output target.
- **Filesystem abstraction**: commands and pipelines depend on VFS interfaces, not raw `System.IO` calls.
- **Plugin isolation**: dynamic loading with unloadable contexts.
- **Asset packaging**: content and dictionaries shipped as zipped artifacts.

## 9. Architecture Decisions

- Use separate projects to isolate concerns (CLI framework, domain/rendering, plugin API, VFS, shell helpers).
- Keep plugin API in `netstandard2.1` to lower compatibility friction for plugin authors.
- Use a step-based pipeline model for output generation to keep build stages explicit and composable.
- Use command auto-discovery (`AddCommandsFrom`) plus explicit default command registration.
- Use scoped filesystem wrappers to centralize path safety and testability.

## 10. Quality Requirements

Top quality goals:
1. **Reliability**: deterministic command execution and clear exit codes.
2. **Extensibility**: plugin contracts and build-command architecture.
3. **Maintainability**: modular projects with focused responsibilities.
4. **Usability**: strong CLI help, shell completion support, script command.
5. **Portability**: cross-platform runtime support for core tooling and assets.

## 11. Risks and Technical Debt

- Dynamic plugin loading can fail due to missing dependencies or malformed manifests.
- Pipeline failures abort generation; diagnostics quality depends on step-level logging.
- External native/tool dependencies (rendering helpers, conversion tools) can vary by OS/runtime packaging.
- Version drift between plugin implementations and API contracts must be managed carefully.

## 12. Glossary

- **BookEnvironment**: runtime context containing configuration, TOC, source/output scopes, and assets.
- **Pipeline**: ordered set of build steps producing one output format.
- **IBuildPluginV1**: plugin entry contract for custom build behavior.
- **VFS**: virtual/scoped file system abstraction used by commands and pipelines.
- **Shellprog**: helper executable for shell-oriented workflows and command interaction.