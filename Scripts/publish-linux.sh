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
mkdir -p package//opt/bookgen
mkdir -p package/DEBIAN
cd ..

cp Setup/linux-start.sh bin/package/usr/local/bin/bookgen
cp Setup/package.control bin/package/DEBIAN/control
cp -r bin/publish-linux/* bin/package/opt/bookgen

cd bin
chmod 775 ./package/DEBIAN/
chmod +x ./package/usr/local/bin/bookgen
chmod +x ./package/opt/bookgen/BookGen
dpkg-deb --build ./package/
mv package.deb bookgen.deb 
rm -r package
