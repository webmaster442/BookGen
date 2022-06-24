# publish script
cd ..

Write-Host "Restoring packages..."
dotnet restore
Write-Host "Build..."
dotnet build -c Release --no-restore
Write-Host "Publish..."
dotnet publish -c Release --no-restore --no-build -o bin\publish\

Write-Host "Creating changelog..."
cd bin\Publish\ 
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