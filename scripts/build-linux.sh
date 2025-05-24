#!/bin/bash
clear

cd ..

dotnet publish BookGen.slnx -c Release -r linux-x64 -p:PublishReadyToRun=true --self-contained true -o bin/publish-linux

cd bin/publish-linux

./BookGen Schemas
./BookGen md2html -i Schemas.md -o Docs/Schemas.html -t "BookGen Schemas"
./BookGen md2html -i ../../../Changelog.md -o Docs/ChangeLog.html -t "Change Log"
./BookGen.md2html -i ../../../Commands.md -o Docs/Commands.html -t "BookGen Commands"
rm schemas.md

cd ..

mkdir -p package/usr/local/bin
mkdir -p package/opt/bookgen
mkdir -p package/DEBIAN

cd ..

cp scripts/linux-start.sh bin/package/usr/local/bin/bookgen
cp scripts/package.control bin/package/DEBIAN/control
cp -r bin/publish-linux/* bin/package/opt/bookgen

cd bin

chmod 775 ./package/DEBIAN/
chmod +x ./package/usr/local/bin/bookgen
chmod +x ./package/opt/bookgen/BookGen
dpkg-deb --build ./package/
mv package.deb bookgen.deb 
rm -r package
