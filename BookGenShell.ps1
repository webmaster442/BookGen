# -----------------------------------------------------------------------------
# BookGen PowerShell Registration script
# Version 2.2
# Last modified: 2022-06-12
# -----------------------------------------------------------------------------

#cdg command

function cdg()
{
    [void] [System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms')
    $FolderBrowserDialog = New-Object System.Windows.Forms.FolderBrowserDialog -Property @{
        ShowNewFolderButton = $false
		SelectedPath = $(Get-Location).Path
        Description = "Select Folder"
    }
    [void] $FolderBrowserDialog.ShowDialog(((New-Object System.Windows.Forms.Form -Property @{TopMost = $true })))
    Push-Location $FolderBrowserDialog.SelectedPath
	$FolderBrowserDialog.Dispose()
}

# register scripts folder to the path
$env:Path += ";$PSScriptRoot"

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

# set prompt
function prompt {
    $git = $(BookGen.ShellHelper.exe "prompt" $(Get-Location).Path) | Out-String
    'PS ' +  $(Get-Location) + ' '+$git+ $(if ($NestedPromptLevel -ge 1) { '>>' }) + ' > '
}


# if argument given set to startup folder
if ($args.Count -eq 1) {
	Set-Location $args[0]
}

# welcome message
Write-Host "To get info on using bookgen type: Bookgen Help"
Write-Host "To get list of commands type: Bookgen SubCommands"
Write-Host "To graphicaly select working directory type: cdg"
Write-Host ""
bookgen version
Write-Host ""
