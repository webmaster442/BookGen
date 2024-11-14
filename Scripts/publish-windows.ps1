# publish script

if ($Host.version.Major -lt 7)
{
	Write-Host "This script requires powershell 7 or newer"
	Read-Host -Prompt "Press any key to continue"
	exit
}

cd ..

Write-Host "Updating Getting started doc"
Show-Markdown getting-started.md > Libs\BookGen.Contents\getting-started.mdr

Write-Host "Publish..."
dotnet publish BookGen.sln -c Release -r win-x64 -p:PublishReadyToRun=true --self-contained true -o bin\publish\data

Write-Host "Creating html docs..."
cd bin\Publish\data
.\BookGen.exe Md2HTML -i ..\..\..\Commands.md -ns -o Commands.html
.\BookGen.exe Md2HTML -i ..\..\..\Changelog.md -ns -o ChangeLog.html
.\BookGen.exe Md2HTML -i ..\..\..\notes.md -ns -o RelaseNotes.html
.\BookGen.exe Md2HTML -i ..\..\..\markdown-cheatsheet.md -o Markdown-cheatsheet.html
$version = (.\BookGen.exe version -bd) | Out-String
$version = $version -replace "`t|`n|`r",""
cd ..
cd ..
cd ..

cd Bootstrappers
dotnet build -c Release
cd ..

copy-item bin\bootstaper\Release\BookGen.exe bin\Publish
copy-item bin\bootstaper\Release\IntegrityCheck.exe bin\Publish
copy-item bin\bootstaper\Release\Bookgen.Win.dll bin\Publish
copy-item bin\bootstaper\Release\Documents.html bin\Publish

cd bin\publish

Write-Host "Getting powershell core..."
$psCoreUrl = "https://github.com/PowerShell/PowerShell/releases/download/v7.4.6/PowerShell-7.4.6-win-x64.zip"
Invoke-WebRequest -Uri $psCoreUrl -OutFile pwsh.zip
Expand-Archive -Path pwsh.zip -DestinationPath "powershell"
Remove-Item pwsh.zip

Write-Host "Getting Node.js..."
$nodeUrl = "https://nodejs.org/dist/v22.11.0/node-v22.11.0-win-x64.zip"
Invoke-WebRequest -Uri $nodeUrl -OutFile node.zip
Expand-Archive -Path node.zip -DestinationPath data
Remove-Item node.zip

Write-Host "Creating installer for ISO image..."
$publishFiles=$(Get-ChildItem -Name -File -Recurse -Include *.*)
cd ..\..
cd Setup
Write-Output "#define MyAppVersion ""$version""" | Out-File -FilePath "version.iss" -Encoding ASCII
Write-Output "[Files]" | Out-File -FilePath "cdfiles.iss" -Encoding ASCII
foreach ($file in $publishFiles)
{
	$targetDir=$(Split-Path $file)
	Write-Output "Source: ""{src}\$file""; DestDir: ""{app}\$targetDir""; Flags: ignoreversion external" | Out-File -Append -FilePath "cdfiles.iss" -Encoding ASCII
}
& 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe' setup-iso.iss
cd..

cd bin\Publish
Write-Host "Creating Integrity file..."
.\IntegrityCheck.exe /compute
cd ..\..

Write-Host "Creating ISO..."
copy-item autorun.inf bin\Publish
.\Scripts\Folder2Iso.exe -i bin\Publish -v "BookGen" -o bin\bookgen.iso

Write-Host "Cleanup..."
Remove-Item bin\Release\ -Recurse
Remove-Item bin\bootstaper\Release\ -Recurse
Remove-Item bin\Publish\ -Recurse

cd Scripts
Write-Host "Done"