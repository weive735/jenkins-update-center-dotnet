@setlocal
@echo off

set PATH=%~dp0\tool\apache-maven-3.3.9\bin;%PATH%
set export_dir=%~dp0export\win7-x64

echo === Build Instant Web Server of ASP.NET Core ====
dotnet restore -r win7-x64 || exit /b 1
dotnet publish -c Release -o "%export_dir%\server" -r win7-x64 || exit /b 1

echo === Jenkins Build Update Center Tools ===
pushd src\ikedam-backend-update-center2 || exit /b 1
call mvn compile || exit /b 1
::call mvn package appassembler:assemble || exit /b 1
xcopy /S /D /Y /I target "%export_dir%\backend-update-center2\target" || exit /b 1
copy pom.xml "%export_dir%\backend-update-center2" || exit /b 1
popd