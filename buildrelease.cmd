@echo off
SET MSBUILD="c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
%MSBUILD% /m BookGen.sln /p:Configuration=Release
cd bin
cd Release
del *.pdb
del *.xml