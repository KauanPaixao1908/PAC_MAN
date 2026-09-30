@echo off
rem Executa o Pac-Verdao a partir da pasta deste arquivo, de onde quer que seja chamado.
cd /d "%~dp0"
dotnet run --project "%~dp0src\PacVerdao\PacVerdao.csproj" -- %*
if errorlevel 1 pause
