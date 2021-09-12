# 2021.09.12
* New: Added an Update command to make future updates easy
* New: Added ImgConvert commmand to convert imgages
* New: XML documentation generator reworked. Now uses XmlDocMarkdown
* New: Syntax rendering reworked, improved
* New: Rewrote launcher (again), Has more features.
	* New layout
	* Options to start VS code, File explorer, Preview and bookgen shell for folder
	* Filtering for recent folders
	* Integrated changelog
	* Integrated updtate laucher
	* Can install to path
* Fix: Improved XHTML compatibility for epub
* Fix: Fixed a memory leak in the generator
* Fix: Various code quality fixes
* Fix: Autocompleter now reacts better
* Change: Removed HTML compressor code due to bad quality of dependency code
* Change: No more ISO image releases, due to auto updater.
* Change: Rar self extracting installer provided

# 2021.05.26
* Removed Node.exe bundle
* Gui extended with help
* New subcommand that allows HTML compression
* Powershell Autocomplete improvements
* Rewrote launcher
* New HTTP server that works on local subnetwork
* New HttpServ subcommand

# 2021.04.13
* Now uses .NET 5.0
* Node.exe bundled with release
* Script runtime resolve order chganged
* Rendering pipieline modernized
* Multi threaded build for static websties
* Folder locking: Prevent multiple instances working on same directory
* Launcher program: starts poweshell with preconfigured bookgen software. Supports Windows terminal
* Removed editor code
* New Modules: Stat, Edit, Pagegen
* Updated dependency packages

# 2020.07.23
* Fixes issues in md2html module image embed
* Added powershell autocomplete install module
* Better argument handling
* Updated SkiaSharp & Svg.Skia

# 2020.06.29
* Updated md2html module help
* Fixed md2html module syntax rendering issue

# 2020.06.24
* Added extra options for single page generation
* Refined printable HTML output
* Updated Moq libary dependency for testing
* Added app setting to disable automatic url start
* Server module for serving static files from a directory
* Console log now has colors
* Updated help
* Diched auto update code, now uses the dotnet tool infrastructure for update and install
* Markdown doc generator module
* Integrated SkiaSharp and an image pipeline
* Added support for Python and PHP scripts
* Added app wide configuration for Script runtime locations and timeout configs
* Editor extended with build actions
* Added a Http server module for testing purposes
* Gui rewitten
* Help updated: split to chaopters
* Static generated website uses turbolinks for faster navigation
* Shell Autocomplete support, by default for powershell
* Shell changed to powershell
* Wordpress builder fixed and extended with additional options
* Generated Json uses UTF-8
* More polished editor, that is based on Ace.js
* Static website now has Turbolinks integrated
* Keyword extractor and link extractor generator module

# 2020.02.01
* Changed versioning scheme. Assembly version reflects config file version, Builds are labeled by relase date
* Breaking: Command line calling syntax changed. See documentation for details.
* Single markdown file to HTML rendering
* Auto updater added. Currently only supports windows
* Epub export updated & refactored to support Epub 3.2 format
* Fully documented with BookGen
* Shortcodes can be Extended with C# Shortcodes via Scripting API
* Can be Extended with Node.js scripts via Shortcode
* Now comes with a Web based editor that is cross platform.
* Initializer reworked
	* Can now create VSCode compatible tasks.json file
	* Can now create Scripting Project csproj file
* Added experimental Assembly documenter
* Source
	* Now uses .NET Core Json serializer
	* Adapted to use Nullable reference types
	* Bookgen now uses subCommand architecture, this means better command extensibility


# 1.0 RC 2 - 2019.10.12
* SRI Generator speed increase with internal cache
* FontAwesome dependency for default template eliminated
* Bootstrap is now loaded from host - CDN dependencies eliminated
* Better ToC file parsing
* Epub xml tweaks
* Better config file generation - less work for configuring
* Content can now include shortcodes
* Fixes issue with image inlineing
* Fixes issue with image copy
* Editor preview

# 1.0 RC 1 - 2019.09.24
* Help now displays Usage informations correctly
* Help now displays config file format
* Updated to .NET Core 3.0

# 1.0 Preview 2 - 2019.09.02

* Unix: Fixed shell file line endings (hopefully)
* Unix: Paths with \ are rewritten to paths with / 
* Wordpress compatible XML file output
* GUI: Display usage doesn't display Press a key to continue 2x
* Initializer Keyboard input handling fixed
* Initializer crash fixed
* Template embeding fixed
* Windows srcipt startup directory fixed
* Built-in template enhancments: lightbox and cookie warning (GDPR)
* Proper translation support for templates
* Static site output builder: Page generation is multi threaded

# 1.0 Previrew 1 - 2019.08.06

* Initial release
* Requires .NET Core 2.2
