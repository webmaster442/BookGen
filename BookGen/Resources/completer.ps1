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
