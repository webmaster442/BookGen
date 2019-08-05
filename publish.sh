#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

function build
{
	printf "\nBuilding BookGen ..."
	dotnet publish -c Release -f netcoreapp2.2 -o ../bin/BookGen
	cd bin
	cd BookGen
	rm *.pdb
	cd ..
	tar -zcvf BookGen.tar.gz BookGen
	rm -r BookGen
	cd ..
}

build
