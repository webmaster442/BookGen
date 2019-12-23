#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

command -v dotnet >/dev/null 2>&1 || { echo >&2 "This program Requires .NET Core Runtime, but it's not installed. Exiting"; exit 1; }

printf "\nBuilding BookGen ..."
dotnet publish BookGen.publish.sln -c Release -f netcoreapp3.1 -r win-x64 --self-contained -o ./bin/BookGen
cd bin/BookGen
rm *.pdb
cd ..
cd ..
printf "\nFinished"