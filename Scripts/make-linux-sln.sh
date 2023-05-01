#!/bin/bash
cd ..
cp BookGen.sln BookGen-Linux.sln
dotnet sln BookGen-Linux.sln remove Prog/BookGen.Launcher
