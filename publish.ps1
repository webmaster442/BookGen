Clear-Host

# publish windows & linux self-contained
dotnet publish -c release -o "bin\publish\windows\bin" --self-contained true -r win-x64 -p PublishReadyToRun BookGen.slnx
dotnet publish -c release -o "bin\publish\linux\bin" --self-contained true -r linux-x64 -p PublishReadyToRun BookGen.slnx

# copy installer scripts
Copy-Item "Installers\install.cmd" "bin\publish\windows\install.cmd"
Copy-Item "Installers\install.sh" "bin\publish\linux\install.sh"

# copy assets
Copy-Item "bin\Release\assets.zip" "bin\publish\windows\bin\assets.zip"
Copy-Item "bin\Release\assets.zip" "bin\publish\linux\bin\assets.zip"

# write version.txt
.\bin\publish\windows\bin\BookGen.exe version > .\bin\publish\windows\version.txt
.\bin\publish\windows\bin\BookGen version > .\bin\publish\linux\version.txt

# Generate docs
.\bin\publish\windows\bin\BookGen Schemas
.\bin\publish\windows\bin\BookGen md2html -i Schemas.md -o "bin\publish\windows\Schemas.html" -t "Configuration schemas"
.\bin\publish\windows\bin\BookGen md2html -i Schemas.md -o "bin\publish\linux\Schemas.html" -t "Configuration schemas"
.\bin\publish\windows\bin\BookGen md2html -i Changelog.md -o "bin\publish\windows\Changelog.html" -t "Change Log"
.\bin\publish\windows\bin\BookGen md2html -i Changelog.md -o "bin\publish\linux\Changelog.html" -t "Change Log"
.\bin\publish\windows\bin\BookGen md2html -i Commands.md -o "bin\publish\windows\Commands.html" -t "BookGen Commands"
.\bin\publish\windows\bin\BookGen md2html -i Commands.md -o "bin\publish\linux\Commands.html" -t "BookGen Commands"
Remove-Item Schemas.md
