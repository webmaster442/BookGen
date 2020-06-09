#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

cls

$workdir = [string](Get-Location)

if (-Not (Get-Command "dotnet" -errorAction SilentlyContinue)) {
	echo "This program Requires .NET Core Runtime, but it's not installed. Exiting"
	exit 1
}

echo "Building BookGen ..."
echo ""
dotnet build -c Release
dotnet pack -c Release

echo "Finished"