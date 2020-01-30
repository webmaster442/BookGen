# 2020.02.01
* Changed versioning scheme. Assembly version reflects config file version, Builds are labeled by relase date
* Breaking: Command line calling syntax changed. See documentation for details.
* Single markdown file to HTML rendering
* Auto updater added. Currently only supports windows
* Epub export updated & refactored to support Epub 3.2 format
* Auto updater added. Currently only supports windows.
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
