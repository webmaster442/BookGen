$psCoreUrl = "https://github.com/PowerShell/PowerShell/releases/download/v7.4.10/PowerShell-7.4.10-win-x64.zip"
$nodeUrl = "https://nodejs.org/dist/v22.16.0/node-v22.16.0-win-x64.zip"

if ($Host.version.Major -lt 7)
{
	Write-Host "This script requires powershell 7 or newer"
	Read-Host -Prompt "Press any key to continue"
	exit
}

$ErrorActionPreference = "Stop"

clear

cd .. # /

Write-Host "Updating Getting started doc"
Show-Markdown getting-started.md > Source\BookGen.Contents\getting-started.mdr

Write-Host "Publish..."
dotnet publish BookGen.slnx -c Release -r win-x64 -p:PublishReadyToRun=true --self-contained true -o bin\publish\app

Write-Host "Generating documents"
cd bin\publish\app
.\BookGen.exe Schemas
.\BookGen.exe md2html -i Schemas.md -o ..\Schemas.html -t "BookGen Schemas"
.\BookGen.exe Md2HTML -i ..\..\..\Changelog.md -o ..\ChangeLog.html -t "Change Log"
.\BookGen.exe Md2HTML -i ..\..\..\Commands.md -o ..\Commands.html -t "BookGen Commands"
del schemas.md

cd ..\..\..\Bootstrappers # /bootstrappers
dotnet build -c Release
cd .. # /
copy-item bin\bootstaper\Release\BookGen.exe bin\Publish
copy-item bin\bootstaper\Release\Bookgen.Win.dll bin\Publish

cd bin\publish # /bin/publish

Write-Host "Getting powershell core..."
Invoke-WebRequest -Uri $psCoreUrl -OutFile pwsh.zip
Expand-Archive -Path pwsh.zip -DestinationPath "powershell"
Remove-Item pwsh.zip

Write-Host "Getting Node.js..."
Invoke-WebRequest -Uri $nodeUrl -OutFile node.zip
Expand-Archive -Path node.zip -DestinationPath app
Remove-Item node.zip

cd ..\..\scripts