@echo off
cd /d "%~dp0"
setlocal

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%CSC%" (
  echo Could not find the .NET Framework compiler ^(csc.exe^).
  pause
  exit /b 1
)

"%CSC%" -nologo -target:winexe -optimize+ -win32manifest:app.manifest -out:FadeAll.exe FadeAll.cs
if errorlevel 1 (
  echo.
  echo Build failed - see errors above.
  pause
  exit /b 1
)

echo Built FadeAll.exe
