@echo off
REM KILL Running process
TIMEOUT /t 1 /nobreak > NUL
TASKKILL /IM {{program}} > NUL
REM Move update files from new folder to current running folder
FOR %F in (new\*.*) DO MOVE /Y %F .
REM Move update directories from new folder to current running folder
FOR /D %D in (new\*.*) DO MOVE /Y %D
REM remove empty directory
RM new
REM Delete downloaded zip file
DEL /y {{tempfile}}
ECHO Update complete
PAUSE
REM Delete update script
DEL ReplaceContents.bat