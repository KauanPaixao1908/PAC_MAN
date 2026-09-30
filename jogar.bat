@echo off
rem Compila e abre o jogo PACMAN001M (precisa do Visual Studio instalado).
cd /d "%~dp0PACMAN001M"

set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto semvs

for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"
if not defined MSBUILD goto semvs

"%MSBUILD%" PACMAN001M.csproj /p:Configuration=Debug /v:minimal /nologo
if errorlevel 1 goto erro
start "" "bin\Debug\PACMAN001M.exe"
goto fim

:semvs
if exist "bin\Debug\PACMAN001M.exe" goto abrir
echo Nao encontrei o Visual Studio.
echo Abra o arquivo PACMAN001M\PACMAN001M.slnx no Visual Studio e aperte F5.
pause
goto fim

:abrir
start "" "bin\Debug\PACMAN001M.exe"
goto fim

:erro
echo Deu erro ao compilar o jogo.
pause

:fim
