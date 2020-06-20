@echo off
REM This is the IMS update script.  It blocks until IMS is no longer running, unzips/extracts the
REM file in argument %1, and then starts IMS again.
:WaitForIMSExit
tasklist /FI "IMAGENAME eq IMS-Service.exe" 2>NUL | find /I /N "IMS-Service.exe">NUL
if %ERRORLEVEL%==0 goto WaitForIMSExit
Powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Expand-Archive -Force '%CD%\%1' '%CD%\'"
del %1
start IMS-Service.exe