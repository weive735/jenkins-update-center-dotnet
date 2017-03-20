@setlocal 

set PATH=%~dp0tool\apache-maven-3.3.9\bin;%PATH%

set WWW_ROOT=..\server\wwwroot
set REPO_URL=http://localhost:5000

set args=-id default
set args=%args% -h /dev/null
set args=%args% -o update-center.json
::set args=%args% -r release-history.json
set args=%args% -repository %REPO_URL%
set args=%args% -hpiDirectory %WWW_ROOT%
set args=%args% -nowiki
set args=%args% -key "..\..\..\rootCA\my-update-center.key"
set args=%args% -certificate "..\..\..\rootCA\my-update-center.crt"
set args=%args% -root-certificate "..\..\..\rootCA\my-update-center.crt"
set args=%args% -pretty
set args=%args% -www "%WWW_ROOT%"

pushd export\win7-x64\backend-update-center2 || exit /b 1
mvn exec:java -Dexec.args="%args%"
popd
