@echo off
REM Wait for the calling program to terminate
TIMEOUT /t 3 /nobreak > NUL
REM Move update files from new folder to current running folder
FOR %%F in (new\*.*) DO MOVE /Y %%F .
REM Move update directories from new folder to current running folder
FOR /D %%D in (new\*.*) DO MOVE /Y %%D
REM remove empty directory
RMDIR new
REM Delete downloaded zip file
DEL {{tempfile}}
ECHO Update complete
REM Delete self
DEL ReplaceContents.bat