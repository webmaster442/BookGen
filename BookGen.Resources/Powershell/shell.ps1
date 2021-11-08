function prompt {
    $git = $(BookGen.ShellHelper.exe "prompt" $(Get-Location).Path) | Out-String
    'PS ' +  $(Get-Location) + ' '+$git+ $(if ($NestedPromptLevel -ge 1) { '>>' }) + ' > '
}

Write-Host "To get info on using bookgen type: Bookgen Help"
Write-Host "To get list of commands type: Bookgen SubCommands"
