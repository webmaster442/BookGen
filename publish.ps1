Clear-Host

function New-Folders {
    if (Test-Path "bin\publish\windows") {
        Remove-Item "bin\publish\windows*" -Recurse -Force
    }

    if (Test-Path "bin\publish\linux") {
        Remove-Item "bin\publish\linux*" -Recurse -Force
    }

    New-Item -Path "bin\publish\windows" -ItemType Directory -Force
    New-Item -Path "bin\publish\linux" -ItemType Directory -Force
}

function Invoke-Publish {
    param(
        [bool] $SelfContained,
        [string] $WindowsArchiveName,
        [string] $LinuxArchiveName
    )

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
    Copy-Item "PublishFiles\install.cmd" "bin\publish\windows\install.cmd"
    Copy-Item "PublishFiles\start_bookgen_shell.cmd" "bin\publish\windows\start_bookgen_shell.cmd"

    # copy assets
    Copy-Item "bin\Release\assets.zip" "bin\publish\windows\bin\assets.zip"
    Copy-Item "bin\Release\assets.zip" "bin\publish\linux\bin\assets.zip"

    # write version.txt
    .\bin\publish\windows\bin\BookGen.exe version > .\bin\publish\windows\version.txt
    .\bin\publish\windows\bin\BookGen version > .\bin\publish\linux\version.txt

    # make docs folder
    New-Item -Path "bin\publish\windows\docs" -ItemType Directory -Force
    New-Item -Path "bin\publish\linux\docs" -ItemType Directory -Force

    # copy license
    Copy-Item ".\LICENCE" "bin\publish\windows\docs\LICENCE.txt"
    Copy-Item ".\LICENCE" "bin\publish\linux\docs\LICENCE.txt"

    # Generate docs
    .\bin\publish\windows\bin\BookGen Schemas
    .\bin\publish\windows\bin\BookGen md2html -i Schemas.md -o "bin\publish\windows\docs\Schemas.html" -t "Configuration schemas"
    .\bin\publish\windows\bin\BookGen md2html -i Schemas.md -o "bin\publish\linux\docs\Schemas.html" -t "Configuration schemas"
    .\bin\publish\windows\bin\BookGen md2html -i Changelog.md -o "bin\publish\windows\docs\Changelog.html" -t "Change Log"
    .\bin\publish\windows\bin\BookGen md2html -i Changelog.md -o "bin\publish\linux\docs\Changelog.html" -t "Change Log"
    .\bin\publish\windows\bin\BookGen md2html -i Commands.md -o "bin\publish\windows\docs\Commands.html" -t "BookGen Commands"
    .\bin\publish\windows\bin\BookGen md2html -i Commands.md -o "bin\publish\linux\docs\Commands.html" -t "BookGen Commands"
    Remove-Item Schemas.md

    # zip
    if ($SelfContained) {
        Compress-Archive -Path "bin\publish\windows\*" -DestinationPath "bin\publish\$WindowsArchiveName" -Force
        Clear-Host
        tar -czvf "bin\publish\$LinuxArchiveName" -C "bin\publish\linux" .
    }
    else {
        Compress-Archive -Path "bin\publish\windows\*" -DestinationPath "bin\publish\$WindowsArchiveName" -Force
        Clear-Host
        tar -czvf "bin\publish\$LinuxArchiveName" -C "bin\publish\linux" .
    }
}

New-Folders

Get-Tools

# Framework-dependent build and archives
Invoke-Publish -SelfContained $false -WindowsArchiveName "BookGen-windows.zip" -LinuxArchiveName "BookGen-linux.tar.gz"

# Self-contained build and archives
Invoke-Publish -SelfContained $true -WindowsArchiveName "BookGen-windows-selefcontained.zip" -LinuxArchiveName "BookGen-linux-selefcontained.tar.gz"
 
.\PublishFiles\mkisofs.exe -V BookGen -o .\bin\publish\bookgen-windows.iso -udf .\bin\publish\windows

