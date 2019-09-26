#!/bin/bash
#-----------------------------------------------------------------------------
# (c) 2019 Ruzsinszki Gábor
# This code is licensed under MIT license (see LICENSE for details)
#-----------------------------------------------------------------------------

function build-core3
{
	printf "\nBuilding BookGen ..."
	dotnet publish -c Release -f -f netcoreapp3.0 -o ./bin/BookGen
	cd bin/BookGen
	rm *.pdb
	cd ..
	tar -zcvf BookGen.tar.gz BookGen
	cd ..
	printf "\nFinished. Package Path: bin/BookGen.tar.gz"
}

build-core3
