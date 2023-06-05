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
$version = (.\BookGen.exe version -bd) | Out-String
$version = $version -replace "`t|`n|`r",""
cd ..
cd ..
cd ..

cd Bootstrappers
dotnet build -c Release
cd ..

copy-item bin\bootstaper\Release\BookGen.exe bin\Publish
copy-item bin\bootstaper\Release\BookGen.Launcher.exe bin\Publish
copy-item bin\bootstaper\Release\Bookgen.Win.dll bin\Publish


Write-Host "Creating zip package..."
$compress = @{
  Path = "bin\publish\*"
  CompressionLevel = "Optimal"
  DestinationPath = "bin\published.Zip"
}
Compress-Archive @compress

Write-Host "Creating hash..."
Get-FileHash -Path bin\published.zip -Algorithm SHA256 > bin\published.txt

Write-Host "Creating installer..."
cd Setup
Write-Output "#define MyAppVersion ""$version""" | Out-File -FilePath "version.iss" -Encoding ASCII
& 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe' setup.iss
cd ..


Write-Host "Cleanup..."
Remove-Item bin\Release\ -Recurse
Remove-Item bin\bootstaper\Release\ -Recurse
Remove-Item bin\Publish\ -Recurse

cd Scripts
Write-Host "Done"