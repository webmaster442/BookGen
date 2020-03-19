#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

workdir=$(pwd)
Zip="/z/app/compress/7z/7z.exe"

command -v dotnet >/dev/null 2>&1 || { echo >&2 "This program Requires .NET Core Runtime, but it's not installed. Exiting"; exit 1; }

printf "\nBuilding BookGen ..."
dotnet publish BookGen.publish.sln -c Release -f netcoreapp3.1 -r win-x64 --self-contained -o ./bin/Publish/BookGen
./bin/Publish/BookGen/BookGen Build -a BuildPrint -d ./BookGen.wiki
cp ./BookGen.Shell/ShellStart.cmd ./bin/Publish/
cd bin/Publish/BookGen
rm *.pdb

cd $workdir/bin
$Zip a publish.zip ./Publish/*

printf "\nFinished"