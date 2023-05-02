#!/bin/bash
cd ..
cp BookGen.sln BookGen-Linux.sln
dotnet sln BookGen-Linux.sln remove Prog/BookGen.Launcher
dotnet sln BookGen-Linux.sln remove Prog/BookGen.Update
dotnet publish BookGen-Linux.sln -c Release -r linux-x64 -p:PublishReadyToRun=true --self-contained true -o bin/publish-linux
cd bin/publish-linux
./BookGen Md2HTML -i ../../Commands.md -ns -o Commands.html
./BookGen Md2HTML -i ../../Changelog.md -ns -o ChangeLog.html
./BookGen Md2HTML -i ../../notes.md -ns -o RelaseNotes.html
cd bin/publish-linux
rm tidy.exe
zip -r ../publish-linux.zip .
cd ..
sha256sum publish-linux.zip > linux-hash.txt
