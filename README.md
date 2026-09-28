# PAC_MAN
by Kauan Paixao for the world

## Pac-Verdão: a caça às taças da Libertadores

Jogo estilo Pac-Man feito em **C# (.NET 8)** para o console — "só que melhor".

O Palmeiras (`PA`) precisa pegar **todas as taças da Libertadores** (`•`) do gramado
fugindo dos rivais **Flamengo** (`FL`), **Corinthians** (`CO`) e **São Paulo** (`SP`).
A cada power-up o **Abel Ferreira** (`AB`) entra em campo e, por alguns segundos,
o Palmeiras pode derrotar os rivais que encontrar. São **3 vidas**: perdeu todas,
o Palmeiras é **rebaixado para a Série B**. A campanha passa por cinco fases —
da **fase de grupos** até a **grande final** — e, ao coletar todas as taças da final,
o Palmeiras vira **o maior time do universo**.

### Como jogar

| Tecla | Ação |
|-------|------|
| Setas ou W A S D | mover o Palmeiras |
| P | pausar / continuar |
| ESC | sair (pede confirmação durante a partida) |
| ENTER | confirmar nas telas |

Pontuação: taça = 10, Abel Ferreira = 50, rival derrotado = 200, 400, 800…
(dobra a cada rival derrotado com o mesmo Abel). O recorde fica salvo entre partidas.

### Fases

| # | Fase | Estádio | O que muda |
|---|------|---------|------------|
| 1 | Fase de Grupos | Allianz Parque | rivais lentos, Abel fica 8 s |
| 2 | Oitavas de Final | La Bombonera | novo labirinto, rivais mais rápidos |
| 3 | Quartas de Final | Mineirão | novo labirinto, Abel fica menos tempo |
| 4 | Semifinal | Monumental de Núñez | Palmeiras e rivais mais rápidos |
| 5 | Grande Final | Estádio Centenário | rivais na mesma velocidade do Palmeiras |

Cada rival marca de um jeito, como os fantasmas do Pac-Man original:

- **Flamengo** — marcação individual: vai direto no Palmeiras.
- **Corinthians** — antecipa a jogada: mira 4 casas à frente do Palmeiras.
- **São Paulo** — persegue de longe, mas recua quando chega perto.

Os rivais alternam entre "recuados" (cada um vai para seu canto) e "pressionando".

### Como executar

Requer o [SDK do .NET 8](https://dotnet.microsoft.com/download) e um terminal de
pelo menos **80x24** caracteres (Windows Terminal, PowerShell, cmd, terminal do Linux/macOS).

```bash
dotnet run --project src/PacVerdao
```

Para treinar uma fase específica: `dotnet run --project src/PacVerdao -- --fase 5`

Para rodar os testes automatizados:

```bash
dotnet test
```

### Organização do código

```
src/PacVerdao/
  Program.cs            laço principal: telas, fases e tempo (50 ticks/s)
  Core/                 regras do jogo, sem dependência de console
    Game.cs             estados, pontuação, vidas, power-up, colisões e IA dos rivais
    Maze.cs             labirinto em grade, túnel e busca de caminho (BFS)
    Layouts.cs          os três labirintos
    Phase.cs            as cinco fases da Libertadores e sua dificuldade
    Actors.cs           Palmeiras (jogador) e rivais
    Direction.cs        direções e coordenadas
  UI/                   desenho no console
    Canvas.cs           tela em memória
    ConsoleRenderer.cs  redesenha só o que mudou (sem piscar)
    GameView.cs         campo, placar e painel lateral
    Screens.cs          título, abertura de fase, Série B e título de campeão
    InputHandler.cs     teclado sem bloqueio
    HighScoreStore.cs   recorde salvo em arquivo
tests/PacVerdao.Tests/  testes xUnit (labirintos, regras e telas)
```
