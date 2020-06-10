# -----------------------------------------------------------------------------
#  (c) 2020 Ruzsinszki Gábor
#  This code is licensed under MIT license (see LICENSE for details)
# -----------------------------------------------------------------------------

#set up path
$currentdir = [string](Get-Location)
$env:Path += ";"+$currentdir

function AutocompleteDotnetCli {
    # PowerShell parameter completion shim for the dotnet CLI
    Register-ArgumentCompleter -Native -CommandName dotnet -ScriptBlock {
        param($commandName, $wordToComplete, $cursorPosition)
            dotnet complete --position $cursorPosition "$wordToComplete" | ForEach-Object {
                [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
         }
    }
}

function AutocompleteBookGen {
    # PowerShell parameter completion shim for BookGen
    Register-ArgumentCompleter -Native -CommandName BookGen -ScriptBlock {
        param($commandName, $wordToComplete, $cursorPosition)
            BookGen "Shell" "$wordToComplete" | ForEach-Object {
                [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
         }
    }
    # Case invariant registration
    Register-ArgumentCompleter -Native -CommandName bookgen -ScriptBlock {
        param($commandName, $wordToComplete, $cursorPosition)
            BookGen "Shell" "$wordToComplete" | ForEach-Object {
                [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
         }
    }
}

function main {
    cls
    echo "BookGen shell started"
    if (Get-Command "dotnet" -errorAction SilentlyContinue) {
        AutocompleteDotnetCli
        echo "Autocomplete for dotnet Command is registered"
    }
    AutocompleteBookGen
    echo "Autocomplete for BookGen command is registered"

    echo ""
	cd ..
}

main

# padding begin
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -------------------------------------------------------------------------------
# -
