@echo off
rem Roda o jogo a partir da pasta deste arquivo, de onde quer que seja aberto.
cd /d "%~dp0"
dotnet run --project "%~dp0PacmanPalmeiras.csproj"
if errorlevel 1 pause
