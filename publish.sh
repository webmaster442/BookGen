#!/bin/bash
dotnet publish -c Release -f netcoreapp2.2 --runtime win-x64 --self-contained -o ../bin/publish/BookGen-win-x64
dotnet publish -c Release -f netcoreapp2.2 --runtime osx-x64 --self-contained -o ../bin/publish/BookGen-osx-x64
dotnet publish -c Release -f netcoreapp2.2 --runtime linux-x64 --self-contained -o ../bin/publish/BookGen-linux-x64
dotnet publish -c Release -f netcoreapp2.2 --runtime linux-arm --self-contained -o ../bin/publish/BookGen-linux-arm
cd bin/publish
echo linux-x64
tar -zcvf BookGen-linux-x64.tar.gz BookGen-linux-x64
rm -r BookGen-linux-x64

echo linux-arm
tar -zcvf BookGen-linux-arm.tar.gz BookGen-linux-arm
rm -r BookGen-linux-arm

echo OSX
tar -zcvf BookGen-osx-x64.tar.gz BookGen-osx-x64
rm -r BookGen-osx-x64

