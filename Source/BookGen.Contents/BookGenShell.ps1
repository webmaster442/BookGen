# -----------------------------------------------------------------------------
# BookGen PowerShell Registration script
# Version 3.1
# Last modified: 2025-05-15
# -----------------------------------------------------------------------------

# NodeJS install test
function Test-NodeJs 
{
    try 
    {
        # Try to get the node version
        $nodeVersion = node --version
        if ($nodeVersion) 
        {
            return $true
        }
    }
    catch 
    {
        return $false
    }
}

# Node commands

function Get-NodePath 
{
    try 
    {
        $nodePath = (Get-Command node.exe -ErrorAction Stop).Source
        $nodeDir = Split-Path $nodePath
        return $nodeDir
    } 
    catch 
    {
        Write-Error "node.exe not found in the system PATH."
    }
}

function npm {
    param (
        [string[]]$Args
    )

    $scriptPath = Get-NodePath
    $nodeExe = Join-Path $scriptPath "node.exe"

    if (-Not (Test-Path $nodeExe)) 
    {
        $nodeExe = "node"
    }

    $npmPrefixJs = Join-Path $scriptPath "node_modules/npm/bin/npm-prefix.js"
    $npmCliJs = Join-Path $scriptPath "node_modules/npm/bin/npm-cli.js"

    $npmPrefixNpmCliJs = & $nodeExe $npmPrefixJs
    $npmPrefixNpmCliJsPath = Join-Path $npmPrefixNpmCliJs "node_modules/npm/bin/npm-cli.js"

    if (Test-Path $npmPrefixNpmCliJsPath) 
    {
        $npmCliJs = $npmPrefixNpmCliJsPath
    }

    & $nodeExe $npmCliJs @Args
}

function npx {
    param (
        [string[]]$Args
    )

    $scriptPath = Get-NodePath
    $nodeExe = Join-Path $scriptPath "node.exe"

    if (-Not (Test-Path $nodeExe)) {
        $nodeExe = "node"
    }

    $npmPrefixJs = Join-Path $scriptPath "node_modules/npm/bin/npm-prefix.js"
    $npxCliJs = Join-Path $scriptPath "node_modules/npm/bin/npx-cli.js"

    $npmPrefixNpxCliJsPath = & $nodeExe $npmPrefixJs
    $npmPrefixNpxCliJs = Join-Path $npmPrefixNpxCliJsPath "node_modules/npm/bin/npx-cli.js"

    if (Test-Path $npmPrefixNpxCliJs) {
        $npxCliJs = $npmPrefixNpxCliJs
    }

    & $nodeExe $npxCliJs @Args
}

function corepack {
    param (
        [string[]]$Args
    )

    $scriptPath = Get-NodePath
    $nodeExe = Join-Path $scriptPath "node.exe"
    $corepackJs = Join-Path $scriptPath "node_modules/corepack/dist/corepack.js"

    if (Test-Path $nodeExe) {
        & $nodeExe $corepackJs @Args
    } else {
        $env:PATHEXT = $env:PATHEXT -replace ";.JS;", ";"
        & node $corepackJs @Args
    }
}

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

# intro message
function intro()
{
    clear
    bookgen version
    Write-Host "┌────────────────────────────────────────────────────────┐"
    Write-Host "│ Added commands:                                        │"
    Write-Host "│  intro: displays this message                          │"
    Write-Host "│  bookgen-info: displays the getting  started guide     │"
    Write-Host "│  cdg: menu driven change driectory                     │"
    Write-Host "│  organize: organize current directory files to subdirs │"
    Write-Host "└────────────────────────────────────────────────────────┘"
    Write-Host "  \"
    Write-Host "   \   \"
    Write-Host "        \ /\"
    Write-Host "        ( )"
    Write-Host "      .( o )."

    Bookgen.exe terminalinstall -t
    if ($LastExitCode -eq 0) 
    {
        Bookgen.exe terminalinstall -c
        if ($LastExitCode -ne 0) 
        {
            Write-Host ""
            Write-Host "To install this shell as a windows terminal profile run:";
            Write-Host "Bookgen terminalinstall"
        }
    }

    if (Test-NodeJs)
    {
        $nodeVersion = node --version
        Write-Host "Node version: $nodeVersion"
    }

    Write-Host ""
    Write-Host "─────────────────────────────────────────────────────────────────────"
}

#Set UTF8 encoding
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[console]::InputEncoding = [System.Text.Encoding]::UTF8

#Set BookGenRoot variable
$env:BookGenPath = $PSScriptRoot

# register scripts folder to the path
$env:Path += ";$PSScriptRoot"

if (-not (Test-NodeJs)) {
    #check if it's bundled
    $nodeDirs = Get-ChildItem -Path $PSScriptRoot -Directory | Where-Object { $_.Name -like "node-*" }
    foreach ($dir in $nodeDirs)
    {
        $nodePath = Join-Path -Path $dir.FullName -ChildPath "node.exe"
        if (Test-Path -Path $nodePath)
        {
            $env:NodeJsDir = "$($dir.FullName)"
            $env:NodeExe = $nodePath

            $env:Path += ";$($dir.FullName)"
            Write-Host "Added $($dir.FullName) to PATH."
            break
        }
    }
}

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
    if (-not [string]::IsNullOrWhiteSpace($git)) {
        'PS ' +  $(Get-Location) + "`n"+$git+ $(if ($NestedPromptLevel -ge 1) { '>>' }) + ' > '
    }
    else {
        'PS ' +  $(Get-Location) + $(if ($NestedPromptLevel -ge 1) { '>>' }) + ' > '
    }
}


# if argument given set to startup folder
if ($args.Count -eq 1) {
    Set-Location $args[0]
}

# welcome message
intro