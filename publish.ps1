Clear-Host

function Invoke-Publish {
    param(
        [bool] $SelfContained,
        [string] $WindowsArchiveName,
        [string] $LinuxArchiveName
    )

    if (Test-Path "bin\publish\windows") {
        Remove-Item "bin\publish\windows*" -Recurse -Force
    }

    if (Test-Path "bin\publish\linux") {
        Remove-Item "bin\publish\windows*" -Recurse -Force
    }

    # publish windows & linux
    if ($SelfContained) {
        dotnet publish -c release -o "bin\publish\windows\bin" --self-contained true -r win-x64 -p PublishReadyToRun BookGen.slnx
        dotnet publish -c release -o "bin\publish\linux\bin" --self-contained true -r linux-x64 -p PublishReadyToRun BookGen.slnx
    }
    else {
        dotnet publish -c release -o "bin\publish\windows\bin" -r win-x64 -p PublishReadyToRun BookGen.slnx
        dotnet publish -c release -o "bin\publish\linux\bin" -r linux-x64 -p PublishReadyToRun BookGen.slnx
    }

    # copy installer scripts
    Copy-Item "Installers\install.cmd" "bin\publish\windows\install.cmd"
    Copy-Item "Installers\run_shell.cmd" "bin\publish\linux\run_shell.cmd"

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

    # zip
    if ($SelfContained) {
        Compress-Archive -Path "bin\publish\windows\*" -DestinationPath "bin\publish\$WindowsArchiveName" -Force
        clear
        tar -czvf "bin\publish\$LinuxArchiveName" -C "bin\publish\linux" .
    }
    else {
        Compress-Archive -Path "bin\publish\windows\*" -DestinationPath "bin\publish\$WindowsArchiveName" -Force
        clear
        tar -czvf "bin\publish\$LinuxArchiveName" -C "bin\publish\linux" .
    }
}
cd assets
.\compile-dictionaries.ps1
cd ..

# Framework-dependent build and archives
Invoke-Publish -SelfContained $false -WindowsArchiveName "BookGen-windows.zip" -LinuxArchiveName "BookGen-linux.tar.gz"

# Self-contained build and archives
Invoke-Publish -SelfContained $true -WindowsArchiveName "BookGen-windows-selefcontained.zip" -LinuxArchiveName "BookGen-linux-selefcontained.tar.gz"