#!/usr/bin/env sh
# Executa o Pac-Verdão a partir da pasta deste arquivo, de onde quer que seja chamado.
cd "$(dirname "$0")" || exit 1
exec dotnet run --project src/PacVerdao/PacVerdao.csproj -- "$@"
