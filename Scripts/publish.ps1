# publish script

function HelpMd() 
{
	$files = @(Get-ChildItem -Name -Path .\Prog\BookGen\Resources\*Module.txt)
	foreach ($file in $files)
	{
		$name = $file.replace('Module.txt', '')
		$name = $name.replace('Help.', '')
		Write-Output "# $name" | Out-File -Append -FilePath "commands.md"
		Write-Output "`r`n" | Out-File -Append -FilePath "commands.md"
		Write-Output "``````bash" | Out-File -Append -FilePath "commands.md"
		echo ".\Prog\BookGen\Resources\$file"
		Get-Content -Path ".\Prog\BookGen\Resources\$file" | Out-File -Append -FilePath "commands.md"
		Write-Output "``````" | Out-File -Append -FilePath "commands.md"
		Write-Output "`r`n" | Out-File -Append -FilePath "commands.md"
	}
}

cd ..

Write-Host "Updating help.md..."
Remove-Item commands.md
HelpMd

Write-Host "Publish..."
dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=true --self-contained true -o bin\publish\

Write-Host "Creating html docs..."
cd bin\Publish\
.\BookGen.exe Md2HTML -i ..\..\commands.md -ns -o Commands.html
.\BookGen.exe Md2HTML -i ..\..\Changelog.md -ns -o ChangeLog.html
.\BookGen.exe Md2HTML -i ..\..\notes.md -ns -o RelaseNotes.html
$version = (.\BookGen.exe version -bd) | Out-String
$version = $version -replace "`t|`n|`r",""
cd ..
cd ..

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
Remove-Item bin\Publish\ -Recurse

cd Scripts
Write-Host "Done"