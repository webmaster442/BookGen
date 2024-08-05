# 2024. 08. 05 

* New: Latex formula editor, that can export to SVG (Windows only)
* New: Added new command to create new files
* New: C# scripting support in markdown files via the '''script ''' tag
* New: Added autocomplete in bookgen shell for basic git commands
* New: ISO build now includes powershell core
* New: Added table of contents support via the [toc] tag in markdown
* New: JSON array support for subcommand configuration.
* New: NodeJs is included in the ISO build and BookGen shell integrates it
* Change: Only ISO build is available for Windows
* Change: New icon and branding
* Change: Updated dependencies
* Change: Cplipboard implementation replaced with TextCopy, now works multiplatform
* Change: Linux distribution now uses a deb package for distribution
* Change: SVG Passthrough implemented in md2html command
* Change: md2thml is extended with templating and new options
* Change: Various BookGen related settings now use a single .ZIP file for configuration
* Change: Reworked templating engine
* Change: Reworked CDG command
* Fix: Various linux compatibility fixes
* Fix: Various fixes in the Table of Contents generation
* Fix: Template processor fixes
* Fix: Various fixes

# 2024. 01. 09
* New: Based on .NET 8
* New: Linux install script
* New: Windows builds with bootstappers
* New: Laucnher includes a markdown cheat sheet
* New: Launcher includes a news channel
* New: Launcher has built-in task list
* New: HTML2PDF command (Windows only)
* New: Program in Directly runnable form, packed as ISO image
* New: Terminal output renderer, when building markdown
* New: CDG program can handle a path argument, to specify where to start from
* New: WWW program that can perform web searches easily
* New: toc command, to find files that are not included in the toc file
* New: shortcodes command, to list available shortcodes
* Change: API cleanup & removed C# scripting support
* Change: StockSearch command functionality has been integrated to the WWW command
* Change: Removed auto updater (hard to maintain)
* Change: Removed ZIP format release
* Change: Folder locking is now process based
* Change: Improved stat command
* Change: HTTP server has favicons & last acces time is sent in header
* Change: Replaces File based folder locking wiht a process based one
* Change: Auto updater removed, because of multiple platform issues
* Change: Launcher file browser usability improvements
* Change: Updated component dependencies
* Fix: Image convert command correctly handles SVG files
* Fix: ImgSearch correctly handles spaces in query
* Fix: Shell now sets UTF-8 encoding for output
* Fix: Various HTTP server bugfixes
* Fix: HTML Tidy speed greatly improved and encoding issues are fixed
* Fix: Initializer adapted to new folder structure
* Fix: Fixed a possible crash in init, when using Windows Terminal
* Fix: HTMTidy runtime speed improved & made more robust

# 2023. 05. 06
* New: GUI has been reworked. Instead of Terminal.GUI Spectre.Console is used
* New: Pack subcommand to pack the book source into a zip file
* New: Download command, to quickly download files from the internet to your project
* New: Tasks command, which allows running additional tasks & customizing workflow
* New: Web server now has a dedicated page to make it easier to connect to it, from other devices
* New: Linux build (experimental)
* Change: The Shell's cdg command is now implemented as terminal UI program with more features
* Change: Various fixes to support building and running on Linux distributions 
* Change: Default console log uses Spectre.Console for output. This produces more colourful output
* Change: Reworked subcommand architecture
* Change: Removed & cleaned up NuGet package dependencies
* Change: Test server port is now 8090
* Change: Removed WP-Load program
* Change: Removed Chapters command
* Change: Removed InstallPS Autocomplete command
* Change: Removed support for VS tasks
* Fix: Various Web sever stability fixes & improvements
* Fix: E-pub build time has been reduced
* Fix: Fixes a bug, which caused the progress bar messages while building not always readable
* Fix: BookGen shell colour scheme adjusted in windows terminal for better readability

# 2023.01.12
* New: Rewrote launcher, looks a bit better & has built in preview function
* New: Math2SVG module (Requires internet connection to work)
* New: Output type for post processing (experimental)
* New: QR code generator module (Requires internet connection to work)
* New: Print output produces an XHTML file too, with embedded CSS rules for better compatibility with word processing 
* New: JSON schema for configuration and tags available
* New: Tags module can now auto extract tags with RAKE if needed
* Change: Reworked preview module, now has a basic markdown editor
* Change: BookGen now ships with .NET, no need to install it separately
* Change: Updater now is a separate program.
* Change: Removed markdown editor from launcher
* Change: Json output from now on doesn't encode non ASCII chars specially.
* Change: Better XHTML output for e-Pub generation with the use of HTMLTidy
* Change: Updated dependencies
* Change: configuration files are moved into .bookgen folder for projects

# 2022.07.11
* New: ExternalLinks command
* New: Tags command
* New: Wordpress output generates tags based on output of tags command
* New: Shell on init prints version
* New: Shell extended with graphical cd command called cdg
* New: Shell prints not commited file count
* New: Math2Svg command
* New: Installer offers to install Windows Terminal & Powershell core, if not installed
* New: Installer can now run without admin rights
* Change: Markdown Title finder now warns, if title is not 1st level
* Change: PageGen module removed
* Change: Progress reporting improved
* Change: Md2HTML default template handles print better
* Change: Windows terminal profile has now a distinct theme
* Change: Windows terminal profile now uses powershell core, if installed
* Change: Launcher has now a built in command documentation
* Fix: Console Gui memory leaks
* Fix: Updated wiki documentation

# 2022.05.23
* New: Better, simplistic icon
* New: Wpload implements download feature
* New: Installs as windows terminal fragment, if Windows terminal is installed
* New: cdg command in shell, to graphically change workdir.
* Fix: printable html generation
* Fix: exiting gui properly deletes the folderlock
* Updated dependencies

# 2022.03.02
* New: Quick editor in launcher
* New: WpLoad tool to interact with wordpress backend sites
* Fix: Stat module not working with GUI
* Fix: Stat module better calculation for page size
* Fix: Printable HTML generation, when using footnotes and file doesn't have new line as end.
* Updated prism.js
* Updated dependencies
* System test project reworked
* Pipeline build system

# 2021.11.18
* New: Wiki command - Opens the BookGen wiki page
* New: Stocksearch command - Search for free stock images online
* New: Launcher has more modern Message boxes
* New: Added more options to the console Gui
* New: Log output to JSON with the global -js argument.
* New: Assembly documenting to a single markdown file (usefull for wiki pages)
* New: Installer based on Inno setup
* Change: Build process now indicates with a console progressbar
* Change: Updated bootstrap to 5.1
* Change: BookGen shell now displays git info in prompt, if folder is git repo.
* Change: Autocomplete handles global arguments.
* Change: Launcher folder list is now independent from version
* Change: Now requires .NET 6 runtime
* Change: Configuration can be in yml format
* Change: Init subcommand now allows to select yml as config format
* Fix: Launcher JumpList was not starting the shell in the folder
* Fix: Auto updater now works

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
