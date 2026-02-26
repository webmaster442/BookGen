@echo off
title "Bookgen shell start script"
where /q pwsh
if %errorlevel%==0 (
    pwsh -NoExit -File "%~dp0bin\BookGenShell.ps1"
) else (
    powershell -NoExit -File "%~dp0bin\BookGenShell.ps1"
)
