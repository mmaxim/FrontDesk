@if "%_echo%"=="" echo off

if not exist build mkdir build

csc.exe /out:build\Microsoft.Web.UI.WebControls.dll @IEWebControls.rsp
xcopy src\Runtime build\Runtime /E /Y /I /Q
