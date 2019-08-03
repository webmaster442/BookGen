#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

function osx 
{
	printf "\nBuilding BookGen for OS-X..."
	dotnet publish -c Release -f netcoreapp2.2 --runtime osx-x64 --self-contained -o ../bin/publish/BookGen-osx-x64
	cd bin/publish
	tar -zcvf BookGen-osx-x64.tar.gz BookGen-osx-x64
	rm -r BookGen-osx-x64
	cd ..
}

function linux-arm
{
	printf "\nBuilding BookGen for ARM Linux..."
	dotnet publish -c Release -f netcoreapp2.2 --runtime linux-arm --self-contained -o ../bin/publish/BookGen-linux-arm
	cd bin/publish
	tar -zcvf BookGen-linux-arm.tar.gz BookGen-linux-arm
	rm -r BookGen-linux-arm
	cd ..
}

function linux-x64
{
	printf "\nBuilding BookGen for x64 Linux..."
	dotnet publish -c Release -f netcoreapp2.2 --runtime linux-x64 --self-contained -o ../bin/publish/BookGen-linux-x64
	cd bin/publish
	tar -zcvf BookGen-linux-x64.tar.gz BookGen-linux-x64
	rm -r BookGen-linux-x64
	cd ..
}

function win
{
	printf "\nBuilding BookGen for Windows x64..."
	dotnet publish -c Release -f netcoreapp2.2 --runtime win-x64 --self-contained -o ../bin/publish/BookGen-win-x64
}

read -p "Build for Windows x64 [Y/N] " -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	win
fi
echo ""

read -p "Build for Linux [Y/N] " -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	linux-x64
	linux-arm
fi
echo ""


read -p "Build for OS-X? [Y/N] " -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
	osx
fi
echo ""
