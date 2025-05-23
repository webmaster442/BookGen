if ($Host.version.Major -lt 7)
{
	Write-Host "This script requires powershell 7 or newer"
	Read-Host -Prompt "Press any key to continue"
	exit
}

$ErrorActionPreference = "Stop"

cd ..

Write-Host "Updating Getting started doc"
Show-Markdown getting-started.md > Source\BookGen.Contents\getting-started.mdr

Write-Host "Publish..."
dotnet publish BookGen.slnx -c Release -r win-x64 -p:PublishReadyToRun=true --self-contained true -o bin\publish\app

cd bin\publish\app
.\BookGen.exe Schemas