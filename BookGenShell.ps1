# -----------------------------------------------------------------------------
# BookGen PowerShell Registration script
# Version 2.9.0
# Last modified: 2024-05-20
# -----------------------------------------------------------------------------

# cdg command
function cdg
{
    $argsAsString = $args -join ' '
    if ([string]::IsNullOrWhiteSpace($argsAsString))
	{
		BookGen.Shell.exe "cdg"
	}
	else
	{
		BookGen.Shell.exe "cdg" "$argsAsString"
	}
    $location = [Environment]::GetEnvironmentVariable('cdgPath', 'User')
    Push-Location $location
}

# www command
function www
{
    $argsAsString = $args -join ' '
    if ([string]::IsNullOrWhiteSpace($argsAsString))
	{
		BookGen.Shell.exe "www"
	}
	else
	{
		BookGen.Shell.exe "www" "$argsAsString"
	}
}

# organize command
function organize
{
    $argsAsString = $args -join ' '
    if ([string]::IsNullOrWhiteSpace($argsAsString))
    {
        BookGen.Shell.exe "organize"
    }
    else
    {
        BookGen.Shell.exe "organize" "$argsAsString"
    }
}

# info command
function bookgen-info()
{
	Clear-Host
	Get-Content $env:BookGenPath\getting-started.mdr | Out-Host -Paging
}

# lancher Command
function launcher()
{
	BookGen.Launch.exe $(Get-Location).Path
}

# intro message
function intro()
{
	clear
	bookgen version
	Write-Host " ____________________________________________________ "
	Write-Host "/ To view the getting started doc type: bookgen-info  \"
	Write-Host "| To get info on using bookgen type: Bookgen Help     |"
	Write-Host "| To graphicaly select working directory type: cdg    |"
	Write-Host "| To start the bookgen launcher type: launcher        |"
	Write-Host "\ To redisplay this message type: intro               /"
	Write-Host " ----------------------------------------------------- "
	Write-Host "  \"
	Write-Host "   \   \"
	Write-Host "        \ /\"
	Write-Host "        ( )"
	Write-Host "      .( o )."

    Bookgen.exe terminalinstall -t
    if ($LastExitCode -eq 0) {
        Bookgen.exe terminalinstall -c
        if ($LastExitCode -ne 0) {
            Write-Host ""
            Write-Host "To install this shell as a windows terminal profile run:";
            Write-Host "Bookgen terminalinstall"
        }
    } 
}

#Set UTF8 encoding
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[console]::InputEncoding = [System.Text.Encoding]::UTF8

#Set BookGenRoot variable
$env:BookGenPath = $PSScriptRoot

# register scripts folder to the path
$env:Path += ";$PSScriptRoot"

# set colors
Set-PSReadLineOption -Colors @{
  Parameter  = 'Red'
  String     = 'Cyan'
  Command    = 'Green'
}

# PowerShell parameter completion shim for BookGen
Register-ArgumentCompleter -Native -CommandName BookGen -ScriptBlock {
	param($commandName, $wordToComplete, $cursorPosition)
		BookGen.exe "Shell" "$wordToComplete" | ForEach-Object {
			[System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
	 }
}
# Case invariant registration
Register-ArgumentCompleter -Native -CommandName bookgen -ScriptBlock {
	param($commandName, $wordToComplete, $cursorPosition)
		BookGen.exe "Shell" "$wordToComplete" | ForEach-Object {
			[System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
	 }
}

# PowerShell parameter completion shim for the dotnet CLI
Register-ArgumentCompleter -Native -CommandName dotnet -ScriptBlock {
	param($commandName, $wordToComplete, $cursorPosition)
		dotnet complete --position $cursorPosition "$wordToComplete" | ForEach-Object {
			[System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
	 }
}

# PowerShell parameter completion shim for git
Register-ArgumentCompleter -Native -CommandName git -ScriptBlock {
	param($commandName, $wordToComplete, $cursorPosition)
		BookGen.Shell.exe "git-complete" $cursorPosition "$wordToComplete" | ForEach-Object {
			[System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
	 }
}

# set prompt
function prompt {
    $git = $(BookGen.Shell.exe "prompt" $(Get-Location).Path)
    'PS ' +  $(Get-Location) + ' '+$git+ $(if ($NestedPromptLevel -ge 1) { '>>' }) + ' > '
}


# if argument given set to startup folder
if ($args.Count -eq 1) {
	Set-Location $args[0]
}

# welcome message
intro