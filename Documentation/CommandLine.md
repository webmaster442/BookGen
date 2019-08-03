# Command line options

```bash
	BookGen -a, --action [action] -d, --dir [directory]
	BookGen -g, --gui -d, --dir [directory]
```

## Arguments

```-a, --action```  - Specify build action
```-g, --gui``` - Start in GUI mode.
```-d, --dir``` - Specify working directory
```-v, --verbose``` - 
    
Supported Actions:

```Test```
Build website with test params & runs it in browser
        
```BuildPrint```
Build printable html
        
```BuildEpub```
Build epub document

```Clean```
Clean output directory (needs valid config)

```CreateConfig```
Create configuration file in directory, bookgen.json file
    
```ValidateConfig```
Validates configuration file, bookgen.json and prints out a list of issues in the config file, if there are.

If no directory parameter is given Current Directory will be used as working  directory.