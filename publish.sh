#!/bin/bash
cd bin/nupkg
rm *.nupkg
cd ../..
dotnet build -c Release
dotnet pack -c Release
dotnet nuget push bin/nupkg/*.nupkg --source "github"
