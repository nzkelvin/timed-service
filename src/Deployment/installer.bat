cd %~dp0
cd..
cd TimedService\bin\Debug
set SERVICE_FILE="%CD%\TimedService.exe"
echo %SERVICE_FILE%
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe %SERVICE_FILE%
pause