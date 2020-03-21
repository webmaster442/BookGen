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
dotnet publish BookGen.publish.sln -c Release -f netcoreapp3.1 -r win-x64 --self-contained -o .\bin\Publish\BookGen
.\bin\Publish\BookGen\BookGen Build -a BuildPrint -d .\BookGen.wiki
cp .\BookGen.Shell\ShellStart.cmd .\bin\Publish\
cd bin\Publish\BookGen
rm *.pdb

cd $workdir\bin

cp .\Release\*.nupkg .\

$compress = @{
  Path = ".\Publish\*"
  CompressionLevel = "Optimal"
  DestinationPath = ".\publish.zip"
}

Compress-Archive -Force @compress

rm -r .\Release
rm -r .\Publish

cd $workdir
echo "Finished"