@setlocal
@echo off

dotnet restore -r win7-x64 || exit /b 1
dotnet publish -c Release -o "%~dp0export\win7-x64" -r win7-x64 || exit /b 1
