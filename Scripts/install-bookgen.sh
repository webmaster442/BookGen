#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

#global variables
dotnetAvailable=false
installpath="/opt"
linkToBin=true

function version_gt() { test "$(printf '%s\n' "$@" | sort -V | head -n 1)" != "$1"; }

function isDotnetInstalled()
{
    local dotnetPath=`command -v dotnet`
    if [ "$dotnetPath" != "" ]; then
        #dotnet command available, check version
        local dotnetVersion=`dotnet --version`
        if version_gt dotnetVersion "3.0"; then
            echo "dotnet core 3.0 installed"
            dotnetAvailable=true
        else
            echo "Found an older version of dotnet core runtime"
            echo "Please upgrade to version 3.0 or higher"
            dotnetAvailable=false
        fi
    else
        echo "dotnet core runtime 3.0 is not installed. Please install it"
        echo "For installation visit: https://dotnet.microsoft.com/download/dotnet-core/3.0"
        dotnetAvailable=false
    fi
}

function setInstallPath()
{
    if [ "$(expr substr $(uname -s) 1 10)" == "MINGW64_NT" ]; then
        installpath="/c"
        linkToBin=false
    fi
    echo "Install target directory: $installpath"
}

function install()
{
    if [ -d "$installpath/BookGen" ]; then
        echo "Removeing previous install..."
        rm -r $installpath/BookGen
    fi
    curl -L -o BookGen-Binary.tar.gz https://github.com/webmaster442/BookGen/releases/latest/download/BookGen-Binary.tar.gz
    tar xf BookGen-Binary.tar.gz -C $installpath
    if [ "$linkToBin" = true ] ; then
        chmod +x $installpath/BookGen/BookGen
        ln -s  $installpath/BookGen/BookGen /usr/local/bin
    fi
    echo "Installed to $installpath/BookGen/"
}

isDotnetInstalled

if [ "$dotnetAvailable" = true ] ; then
    setInstallPath
    install
fi