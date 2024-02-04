#!/bin/bash
cd ..
cp BookGen.sln BookGen-Linux.sln
dotnet sln BookGen-Linux.sln remove Prog/BookGen.Launcher
dotnet publish BookGen-Linux.sln -c Release -r linux-x64 -p:PublishReadyToRun=true --self-contained true -o bin/publish-linux

cd bin/publish-linux
./BookGen Md2HTML -i ../../Commands.md -ns -o Commands.html
./BookGen Md2HTML -i ../../Changelog.md -ns -o ChangeLog.html
./BookGen Md2HTML -i ../../notes.md -ns -o RelaseNotes.html
rm tidy.exe

cd ..
mkdir -p package/usr/local/bin
mkdir -p package/DEBIAN
cd ..
cp Setup/linux-start.sh bin/package/usr/local/bin/bookgen
cp Setup/package.control bin/package/DEBIAN/control
cd bin
chmod 775 ./package/DEBIAN/
dpkg-deb --build ./package/