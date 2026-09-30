# PAC_MAN
by Kauan Paixao for the world

## Pac-Man do Palmeiras

Jogo estilo Pac-Man feito em C# (console).

O Palmeiras (`P`) tem que pegar todas as taças da Libertadores (`.`) fugindo dos
rivais Corinthians (`C`), Flamengo (`F`) e São Paulo (`S`). Pegando o Abel Ferreira
(`A`) dá pra derrotar os rivais por alguns segundos. São 3 vidas, se acabar o
Palmeiras é rebaixado pra Série B. O jogo tem 5 fases, da fase de grupos até a
grande final, e se ganhar a final o Palmeiras vira o maior time do universo.

### Como rodar

Precisa do .NET 8. O jeito mais fácil no Windows é dar dois cliques no `jogar.bat`.

Ou pelo terminal, na pasta do projeto (onde está o `Program.cs`):

```
dotnet run
```

Ou abra o `PacmanPalmeiras.csproj` no Visual Studio e aperte F5.

### Controles

- Setas ou W A S D: mover
- ESC: sair

### Pontos

- Taça: 10
- Abel Ferreira: 50
- Rival derrotado: 200
