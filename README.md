# PAC_MAN
by Kauan Paixao for the world

## PACMAN001M - Pac-Man do Palmeiras

Jogo estilo Pac-Man feito no Visual Studio em **Windows Forms (.NET Framework 4.7.2)**,
usando as mesmas ferramentas do projeto da aula: `PictureBox` desenhada no evento
`Paint` (`Pen`, `SolidBrush`, `FillRectangle`, `DrawRectangle`, `FillEllipse`,
`DrawEllipse`), `Timer` com o evento `Tick`, `Button` com `Click` e `Label`.

O Palmeiras (bola verde) tem que pegar todas as taças da Libertadores (bolinhas
amarelas) fugindo dos rivais Corinthians (preto), Flamengo (vermelho) e
São Paulo (branco). Pegando o Abel Ferreira (bola laranja) os rivais ficam azuis
e dá pra derrotar eles. São 3 vidas, se acabar o Palmeiras é rebaixado pra
Série B. São 5 fases, da fase de grupos até a grande final, e ganhando a final o
Palmeiras vira o maior time do universo.

### Como rodar

- **Visual Studio:** abra `PACMAN001M/PACMAN001M.slnx` e aperte F5.
- **Atalho:** dê dois cliques no `jogar.bat` (ele compila com o Visual Studio e abre o jogo).

### Como jogar

- **START / STOP:** começa, pausa e continua. Depois do fim de jogo, começa de novo.
- **Setas do teclado ( ^  ↓  <  > ):** mudam a direção do Palmeiras.
- Os botões ^, ↓, <, > na tela fazem a mesma coisa com o mouse.
- **SAIR:** fecha o jogo.

### Pontos

- Taça: 10
- Abel Ferreira: 50
- Rival derrotado: 200

### Arquivos principais

- `PACMAN001M/Form1.cs`: o código do jogo
- `PACMAN001M/Form1.Designer.cs`: a tela (tabuleiro, botões e textos)
- `PACMAN001M/Program.cs`: abre o Form1
