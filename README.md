# PAC_MAN
by Kauan Paixao for the world

## Pac-Man do Palmeiras

Jogo estilo Pac-Man feito em C# no Visual Studio (projeto "Aplicativo de Console", .NET 8).

O Palmeiras (`P`) tem que pegar todas as taças da Libertadores (`.`) fugindo dos
rivais Corinthians (`C`), Flamengo (`F`) e São Paulo (`S`). Pegando o Abel Ferreira
(`A`) dá pra derrotar os rivais por alguns segundos. São 3 vidas, se acabar o
Palmeiras é rebaixado pra Série B. O jogo tem 5 fases, da fase de grupos até a
grande final, e se ganhar a final o Palmeiras vira o maior time do universo.

### Como rodar

Precisa do .NET 8.

- **Visual Studio:** abra o `PacmanPalmeiras.sln` e aperte F5 (ou Ctrl+F5).
- **Windows, sem abrir o Visual Studio:** dê dois cliques no `jogar.bat`.
- **Terminal:** na pasta do projeto, `dotnet run --project PacmanPalmeiras`.

### Arquivos

```
PacmanPalmeiras.sln          solução do Visual Studio
jogar.bat                    atalho para rodar o jogo
PacmanPalmeiras/
  PacmanPalmeiras.csproj     projeto
  Program.cs                 começo do programa (Main)
  Jogo.cs                    laço do jogo, fases, pontos, vidas e telas
  Mapa.cs                    labirinto
  Jogador.cs                 o Palmeiras
  Rival.cs                   os times rivais
  Direcao.cs                 direções (cima, baixo, esquerda, direita)
```

### Controles

- Setas ou W A S D: mover
- ESC: sair

### Pontos

- Taça: 10
- Abel Ferreira: 50
- Rival derrotado: 200
