#!/bin/bash
function CleanOutputDir
{
	if [ -d "output" ]; then
		rm -r output
	fi
}

function DoTest
{
	echo "Executing: dotnet BookGen.dll -d "$DIR" -a "$1" -n" >> "$LOGDIR/test.log"
	dotnet BookGen.dll -d "$DIR" -a "$1" -n >> "$LOGDIR/test.log"
	echo "Exit Code of $1: $?" >> "$LOGDIR/test.log"
	printf "\n\n\n" >> "$LOGDIR/test.log"
}

LOGDIR=$(pwd)
cd BookStub
CleanOutputDir
DIR=$(pwd)
cd ..
cd ..
cd bin/Debug/netcoreapp3.0

STARTDATE=$(date)
echo "Test start time: $STARTDATE" > "$LOGDIR/test.log"
echo "Log dir: $LOGDIR" >> "$LOGDIR/test.log"
echo "Input dir: $DIR" >> "$LOGDIR/test.log"


DoTest "BuildEpub"
DoTest "BuildPrint"
DoTest "BuildWeb"
DoTest "BuildWordpress"

cd $LOGDIR
cd BookStub
CleanOutputDir
cd ..
