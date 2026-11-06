using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

// Nome do projeto (agrupa as classes do jogo)
namespace PACMAN001
{
    // Janela principal do jogo
    public partial class Form1 : Form
    {
        // Tabela com as teclas e as imagens do Pac-Man
        string[,] pacman = new string[20, 10];

        // Mapa do labirinto (uma linha de texto por linha do mapa)
        string[] tabuleiro = new string[100];

        // Número de linhas do labirinto
        int lintabuleiro = 21;

        // Número de colunas do labirinto
        int coltabuleiro = 23;

        // Largura de cada casa, em pixels
        int largura = 20;

        // Altura de cada casa, em pixels
        int altura = 20;

        // Coluna onde o Pac-Man está
        int cursorcol = 1;

        // Linha onde o Pac-Man está
        int cursorlin = 1;

        // Direção escolhida pelo jogador (w, a, s ou d)
        char tecla = ' ';

        // Contador de tempo da partida
        int tempo = 0;

        // Tempo máximo da partida (600 ciclos de 300 ms = 3 minutos)
        int tempolimite = 600;

        // Momento em que os fantasmas começam a andar
        int tempoliberar = 10;

        // Distância (em pixels) em que o fantasma começa a perseguir
        int raiopixels = 250;

        // Pontos do jogador
        int pontos = 0;

        // Alterna entre 0 e 1 para animar a boca do Pac-Man
        int flagimagempacman = 0;

        // Tempo que ainda resta do poder (comer fantasmas)
        int poder = 0;

        // Diz se o jogo está pausado
        bool pausado = false;

        // Diz se um fantasma pegou o Pac-Man
        bool pego = false;

        // Relógio só dos fantasmas (eles andam no ritmo dele)
        System.Windows.Forms.Timer timerFantasmas = new System.Windows.Forms.Timer();

        // Linha onde cada fantasma começa
        int[] fantasmalininicial = new int[4];

        // Coluna onde cada fantasma começa
        int[] fantasmacolinicial = new int[4];

        // Linha de cada power-up
        int[] powerlin = new int[4];

        // Coluna de cada power-up
        int[] powercol = new int[4];

        // Caixas de imagem dos 4 fantasmas
        PictureBox[] pbFantasmas = new PictureBox[4];

        // Caixas de imagem dos 4 power-ups
        PictureBox[] pbPowers = new PictureBox[4];

        // Imagens dos 4 fantasmas (PNG com fundo transparente)
        Image[] imgFantasmas = new Image[4];

        // Imagem do power-up
        Image imgPower;

        // Imagem da bolinha que dá pontos
        Image imgBola;

        // Lista de fantasmas (cabem até 20, o jogo usa 4)
        FANTASMA[] fantasmas=new FANTASMA[20];

        // Roda quando a janela abre: prepara tudo
        public Form1()
        {
            // Cria os botões, textos e imagens da janela
            InitializeComponent();

            // Fundo da janela preto
            BackColor = Color.Black;

            // Texto do tempo branco
            lblTempo.ForeColor = Color.White;

            // Texto dos pontos branco
            lblPontos.ForeColor = Color.White;

            // Junta os botões para deixar todos escuros com borda branca
            Button[] botoes = { btnInicializar, btnSair };

            // Para cada botão...
            for (int b = 0; b < botoes.Length; b++)
            {
                // Botão com visual plano
                botoes[b].FlatStyle = FlatStyle.Flat;

                // Borda branca
                botoes[b].FlatAppearance.BorderColor = Color.White;

                // Borda com 1 pixel
                botoes[b].FlatAppearance.BorderSize = 1;

                // Fundo cinza escuro
                botoes[b].BackColor = Color.FromArgb(40, 40, 40);

                // Texto branco
                botoes[b].ForeColor = Color.White;
            }

            // Ajusta o tamanho do tabuleiro à janela
            Form1_Resize(this, EventArgs.Empty);

            // Mostra a tela de abertura
            pbTabuleiro.BackgroundImage = Image.FromFile("abertura.png");

            // Tecla W
            pacman[0, 0] = "w";

            // Imagem 1 do Pac-Man olhando para cima
            pacman[0, 1] = "pacman_cima.png";

            // Imagem 2 do Pac-Man olhando para cima
            pacman[0, 2] = "pacman_cima_2.png";

            // Tecla S
            pacman[1, 0] = "s";

            // Imagem 1 do Pac-Man olhando para baixo
            pacman[1, 1] = "pacman_baixo.png";

            // Imagem 2 do Pac-Man olhando para baixo
            pacman[1, 2] = "pacman_baixo_2.png";

            // Tecla A
            pacman[2, 0] = "a";

            // Imagem 1 do Pac-Man olhando para a esquerda
            pacman[2, 1] = "pacman_Esquerda.png";

            // Imagem 2 do Pac-Man olhando para a esquerda
            pacman[2, 2] = "pacman_Esquerda_2.png";

            // Tecla D
            pacman[3, 0] = "d";

            // Imagem 1 do Pac-Man olhando para a direita
            pacman[3, 1] = "pacman_Direita.png";

            // Imagem 2 do Pac-Man olhando para a direita
            pacman[3, 2] = "pacman_Direita_2.png";

            // Fantasma 0 começa na linha 9
            fantasmalininicial[0] = 9;

            // Fantasma 0 começa na coluna 11
            fantasmacolinicial[0] = 11;

            // Fantasma 1 começa na linha 10
            fantasmalininicial[1] = 10;

            // Fantasma 1 começa na coluna 11
            fantasmacolinicial[1] = 11;

            // Fantasma 2 começa na linha 11
            fantasmalininicial[2] = 11;

            // Fantasma 2 começa na coluna 11
            fantasmacolinicial[2] = 11;

            // Fantasma 3 começa na linha 12
            fantasmalininicial[3] = 12;

            // Fantasma 3 começa na coluna 11
            fantasmacolinicial[3] = 11;

            // Liga a posição 0 à caixa do fantasma 0
            pbFantasmas[0] = pbFantasma0;

            // Liga a posição 1 à caixa do fantasma 1
            pbFantasmas[1] = pbFantasma1;

            // Liga a posição 2 à caixa do fantasma 2
            pbFantasmas[2] = pbFantasma2;

            // Liga a posição 3 à caixa do fantasma 3
            pbFantasmas[3] = pbFantasma3;

            // Liga a posição 0 à caixa do power-up 0
            pbPowers[0] = pbPower0;

            // Liga a posição 1 à caixa do power-up 1
            pbPowers[1] = pbPower1;

            // Liga a posição 2 à caixa do power-up 2
            pbPowers[2] = pbPower2;

            // Liga a posição 3 à caixa do power-up 3
            pbPowers[3] = pbPower3;

            // Imagem do fantasma 0 (Fluminense)
            imgFantasmas[0] = Image.FromFile("Fantasma_Fluminense.png");

            // Imagem do fantasma 1 (Corinthians)
            imgFantasmas[1] = Image.FromFile("fantasma_corinthians.png");

            // Imagem do fantasma 2 (Flamengo)
            imgFantasmas[2] = Image.FromFile("fantasma_flamengo.png");

            // Imagem do fantasma 3 (São Paulo)
            imgFantasmas[3] = Image.FromFile("fantasma_sao_paulo.png");

            // Carrega a imagem do power-up (Abel Ferreira)
            imgPower = Image.FromFile("power_up_Abel_Ferreira.png");

            // Carrega a imagem da bolinha
            imgBola = Image.FromFile("bola.png");

            // Cria os 4 fantasmas
            for (int i = 0; i < 4; i++)
            {
                // Cria o fantasma na posição inicial dele
                fantasmas[i] = new FANTASMA(fantasmalininicial[i], fantasmacolinicial[i]);
            }

            // A cada tique, chama o método que move os fantasmas
            timerFantasmas.Tick += timerFantasmas_Tick;
        }

        // Botão SAIR
        private void btnSair_Click(object sender, EventArgs e)
        {
            // Fecha o jogo
            Close();
        }

        // Roda quando a janela muda de tamanho
        private void Form1_Resize(object sender, EventArgs e)
        {
            // Se a janela estiver muito pequena, não faz nada
            if ((ClientSize.Width < 100) || (ClientSize.Height < 200)) return;

            // Tamanho de cada casa, com base na altura da janela
            int tam = (ClientSize.Height - 90) / lintabuleiro;

            // Se a largura da janela for menor, usa ela
            if ((ClientSize.Width - 24) / coltabuleiro < tam)
            {
                // Tamanho com base na largura
                tam = (ClientSize.Width - 24) / coltabuleiro;
            }

            // Tamanho mínimo da casa: 10 pixels
            if (tam < 10)
            {
                // Usa 10 pixels
                tam = 10;
            }

            // Salva a nova largura da casa
            largura = tam;

            // Salva a nova altura da casa
            altura = tam;

            // Margem da esquerda para centralizar o tabuleiro
            int esq = (ClientSize.Width - coltabuleiro * tam) / 2;

            // Posiciona o tabuleiro
            pbTabuleiro.Location = new Point(esq, 12);

            // Define o tamanho do tabuleiro
            pbTabuleiro.Size = new Size(coltabuleiro * tam, lintabuleiro * tam);

            // Posiciona o botão INICIALIZAR
            btnInicializar.Location = new Point(esq, 24 + lintabuleiro * tam);

            // Posiciona o botão SAIR
            btnSair.Location = new Point(esq + coltabuleiro * tam - btnSair.Width, 24 + lintabuleiro * tam);

            // Posiciona o texto do tempo
            lblTempo.Location = new Point(esq, 56 + lintabuleiro * tam);

            // Posiciona o texto dos pontos
            lblPontos.Location = new Point(esq + coltabuleiro * tam / 2, 56 + lintabuleiro * tam);

            // Posiciona a caixa que recebe as teclas
            txbTeclado.Location = new Point(esq + 10, 12 + lintabuleiro * tam - 24);
        }

        // Transforma o tempo restante em texto mm:ss
        private string formatarTempo()
        {
            // Segundos que faltam (cada ciclo = 0,3 segundo)
            int segundos = (tempolimite - tempo) * 3 / 10;

            // Minutos que faltam
            int minutos = segundos / 60;

            // Segundos que sobram depois dos minutos
            segundos = segundos % 60;

            // Devolve o texto, ex.: 02:45
            return minutos.ToString("00") + ":" + segundos.ToString("00");
        }

        // Diz se outro fantasma já está nessa casa
        private bool posicaoOcupada(int lin, int col, int quem)
        {
            // Olha os 4 fantasmas
            for (int j = 0; j < 4; j++)
            {
                // Se for outro fantasma e estiver na mesma casa...
                if ((j != quem) && (fantasmas[j].getFantasmaLin() == lin) && (fantasmas[j].getFantasmaCol() == col))
                {
                    // Casa ocupada
                    return true;
                }
            }

            // Casa livre
            return false;
        }

        // Roda a cada 300 ms: é o "coração" do jogo
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Passa 1 ciclo de tempo
            tempo++;

            // Se o Pac-Man está com poder
            if (poder > 0)
            {
                // Gasta 1 ciclo do poder
                poder--;
            }

            // Jogador quer ir para CIMA
            if (tecla == 'w')
            {
                // Se a casa de cima não é parede
                if ((tabuleiro[cursorlin - 1][cursorcol] == '.')||(tabuleiro[cursorlin - 1][cursorcol] == ' ') || (tabuleiro[cursorlin - 1][cursorcol] == 'P'))
                {
                    // Anda 1 casa para cima
                    cursorlin--;

                    // Se tem bolinha nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        // +1 ponto
                        pontos++;
                    }

                    // Se tem power-up nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        // Liga o poder por 25 ciclos (7,5 segundos)
                        poder = 25;

                        // +10 pontos
                        pontos = pontos + 10;
                    }

                    // Copia a linha do mapa para poder alterar
                    var sb = new StringBuilder(tabuleiro[cursorlin]);

                    // Apaga o item que o Pac-Man pegou
                    sb[cursorcol] = ' ';

                    // Salva a linha alterada no mapa
                    tabuleiro[cursorlin]=sb.ToString();
                }
            }

            // Jogador quer ir para BAIXO
            if (tecla == 's')
            {
                // Se a casa de baixo não é parede
                if ((tabuleiro[cursorlin + 1][cursorcol] == '.')|| (tabuleiro[cursorlin + 1][cursorcol] == ' ') || (tabuleiro[cursorlin + 1][cursorcol] == 'P'))
                {
                    // Anda 1 casa para baixo
                    cursorlin++;

                    // Se tem bolinha nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        // +1 ponto
                        pontos++;
                    }

                    // Se tem power-up nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        // Liga o poder por 25 ciclos (7,5 segundos)
                        poder = 25;

                        // +10 pontos
                        pontos = pontos + 10;
                    }

                    // Copia a linha do mapa para poder alterar
                    var sb = new StringBuilder(tabuleiro[cursorlin]);

                    // Apaga o item que o Pac-Man pegou
                    sb[cursorcol] = ' ';

                    // Salva a linha alterada no mapa
                    tabuleiro[cursorlin] = sb.ToString();
                }
            }

            // Jogador quer ir para a ESQUERDA
            if (tecla == 'a')
            {
                // Se a casa da esquerda não é parede
                if ((tabuleiro[cursorlin][cursorcol-1] == '.') || (tabuleiro[cursorlin][cursorcol-1] == ' ') || (tabuleiro[cursorlin][cursorcol-1] == 'P'))
                {
                    // Anda 1 casa para a esquerda
                    cursorcol--;

                    // Se tem bolinha nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        // +1 ponto
                        pontos++;
                    }

                    // Se tem power-up nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        // Liga o poder por 25 ciclos (7,5 segundos)
                        poder = 25;

                        // +10 pontos
                        pontos = pontos + 10;
                    }

                    // Copia a linha do mapa para poder alterar
                    var sb = new StringBuilder(tabuleiro[cursorlin]);

                    // Apaga o item que o Pac-Man pegou
                    sb[cursorcol] = ' ';

                    // Salva a linha alterada no mapa
                    tabuleiro[cursorlin] = sb.ToString();
                }
            }

            // Jogador quer ir para a DIREITA
            if (tecla == 'd')
            {
                // Se a casa da direita não é parede
                if ((tabuleiro[cursorlin][cursorcol + 1] == '.')|| (tabuleiro[cursorlin][cursorcol+1] == ' ') || (tabuleiro[cursorlin][cursorcol+1] == 'P'))
                {
                    // Anda 1 casa para a direita
                    cursorcol++;

                    // Se tem bolinha nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        // +1 ponto
                        pontos++;
                    }

                    // Se tem power-up nessa casa
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        // Liga o poder por 25 ciclos (7,5 segundos)
                        poder = 25;

                        // +10 pontos
                        pontos = pontos + 10;
                    }

                    // Copia a linha do mapa para poder alterar
                    var sb = new StringBuilder(tabuleiro[cursorlin]);

                    // Apaga o item que o Pac-Man pegou
                    sb[cursorcol] = ' ';

                    // Salva a linha alterada no mapa
                    tabuleiro[cursorlin] = sb.ToString();
                }
            }

            // Vê se o Pac-Man encostou em algum fantasma
            verificarBatida();

            // Número da direção (escolhe a imagem do Pac-Man)
            int dir = 0;

            // Converte a tecla em número
            switch (tecla)
            {
                // Tecla W
                case 'w':

                    // 0 = cima
                    dir = 0;

                    // Sai do switch
                    break;

                // Tecla S
                case 's':

                    // 1 = baixo
                    dir = 1;

                    // Sai do switch
                    break;

                // Tecla A
                case 'a':

                    // 2 = esquerda
                    dir = 2;

                    // Sai do switch
                    break;

                // Tecla D
                case 'd':

                    // 3 = direita
                    dir = 3;

                    // Sai do switch
                    break;
            }

            // Mostra a imagem certa do Pac-Man (direção + animação)
            pbPACMAN.BackgroundImage = Image.FromFile(pacman[dir, flagimagempacman + 1]);

            // Troca 0 por 1 (ou 1 por 0) para abrir/fechar a boca
            flagimagempacman = Math.Abs(flagimagempacman - 1);

            // Pac-Man do tamanho de uma casa
            pbPACMAN.Size = new Size(largura, altura);

            // Coloca o Pac-Man na posição dele na tela
            pbPACMAN.Location = new Point(cursorcol * largura, cursorlin * altura);

            // Mostra o Pac-Man
            pbPACMAN.Visible = true;

            // Para cada fantasma e power-up
            for (int i = 0; i < 4; i++)
            {
                // Esconde a caixa do fantasma (ele é desenhado no tabuleiro)
                pbFantasmas[i].Visible = false;

                // Esconde a caixa do power-up (ele é desenhado no tabuleiro)
                pbPowers[i].Visible = false;
            }

            // Vê se o jogo acabou (vitória ou game over)
            verificarFimDeJogo();
        }

        // Roda a cada X ms (calculado no iniciarPartida): faz os fantasmas andarem
        private void timerFantasmas_Tick(object sender, EventArgs e)
        {
            // Se o jogo não está rodando, não faz nada
            if (timer1.Enabled == false) return;

            // Para cada um dos 4 fantasmas
            for (int i = 0; i < 4; i++)
            {
                // Se já é hora de soltar os fantasmas
                if (tempo >= tempoliberar)
                {
                    // Guarda a linha antes de mover
                    int linantes = fantasmas[i].getFantasmaLin();

                    // Guarda a coluna antes de mover
                    int colantes = fantasmas[i].getFantasmaCol();

                    // Distância na horizontal até o Pac-Man (pixels)
                    double dx = (cursorcol - fantasmas[i].getFantasmaCol()) * largura;

                    // Distância na vertical até o Pac-Man (pixels)
                    double dy = (cursorlin - fantasmas[i].getFantasmaLin()) * altura;

                    // Se o Pac-Man está com poder
                    if (poder > 0)
                    {
                        // Fantasma foge do Pac-Man
                        fantasmas[i].setFugir(tabuleiro, cursorlin, cursorcol);
                    }

                    // Senão, se o Pac-Man está perto (dentro do raio)
                    else if (Math.Sqrt(dx * dx + dy * dy) <= raiopixels)
                    {
                        // Fantasma persegue o Pac-Man
                        fantasmas[i].setMover(tabuleiro, cursorlin, cursorcol);
                    }

                    // Caso contrário
                    else
                    {
                        // Fantasma anda sem rumo
                        fantasmas[i].setMoverLivre(tabuleiro);
                    }

                    // Se bateu em outro fantasma, volta para onde estava
                    if (posicaoOcupada(fantasmas[i].getFantasmaLin(), fantasmas[i].getFantasmaCol(), i) == true)
                    {
                        // Coloca o fantasma nessa posição
                        fantasmas[i].setPosicaoInicial(linantes, colantes);

                        // Esquece a última direção do fantasma
                        fantasmas[i].zerarDirecao();
                    }
                }
            }

            // Vê se algum fantasma encostou no Pac-Man
            verificarBatida();

            // Vê se o jogo acabou (vitória ou game over)
            verificarFimDeJogo();
        }

        // Vê se o Pac-Man e algum fantasma estão na mesma casa
        private void verificarBatida()
        {
            // Para cada fantasma, vê se encostou no Pac-Man
            for (int i = 0; i < 4; i++)
            {
                // Se o Pac-Man e o fantasma estão na mesma casa
                if ((cursorcol == fantasmas[i].getFantasmaCol()) && (cursorlin == fantasmas[i].getFantasmaLin()))
                {
                    // Se o Pac-Man está com poder
                    if (poder > 0)
                    {
                        // Procura uma posição inicial livre para o fantasma voltar
                        for (int k = 0; k < 4; k++)
                        {
                            // Qual posição inicial testar (0 a 3)
                            int idx = (i + k) % 4;

                            // Se essa posição está livre
                            if (posicaoOcupada(fantasmalininicial[idx], fantasmacolinicial[idx], i) == false)
                            {
                                // Coloca o fantasma nessa posição
                                fantasmas[i].setPosicaoInicial(fantasmalininicial[idx], fantasmacolinicial[idx]);

                                // Achou lugar livre: para de procurar
                                break;
                            }
                        }

                        // +15 pontos por comer o fantasma
                        pontos = pontos + 15;
                    }

                    // Caso contrário
                    else
                    {
                        // Pac-Man foi pego!
                        pego = true;
                    }
                }
            }
        }

        // Vê se o jogo acabou e mostra VITÓRIA ou GAME OVER
        private void verificarFimDeJogo()
        {
            // Conta quantos itens ainda faltam pegar
            int restantes = 0;

            // Para cada linha do mapa...
            for (int lin = 0; lin < lintabuleiro; lin++)
            {
                // ...e cada coluna dessa linha
                for (int col = 0; col < coltabuleiro; col++)
                {
                    // Se tem bolinha ou power-up
                    if ((tabuleiro[lin][col] == '.') || (tabuleiro[lin][col] == 'P'))
                    {
                        // +1 item restante
                        restantes++;
                    }
                }
            }

            // Mostra o tempo na tela
            lblTempo.Text = "TEMPO:  " + formatarTempo();

            // Mostra os pontos na tela
            lblPontos.Text="PONTOS: "+ pontos.ToString();

            // Se o botão não estiver sendo clicado agora (para não perder o clique)
            if (btnInicializar.Focused == false)
            {
                // Manda as teclas para a caixa de teclado
                txbTeclado.Focus();
            }

            // Redesenha o tabuleiro
            pbTabuleiro.Invalidate();

            // Se pegou tudo e não foi pego: VITÓRIA
            if ((restantes == 0) && (pego == false))
            {
                // Para o relógio do jogo
                timer1.Enabled = false;

                // Para o relógio dos fantasmas
                timerFantasmas.Enabled = false;

                // Botão volta a ser INICIALIZAR
                btnInicializar.Text = "INICIALIZAR";

                // Escreve VITÓRIA ao lado do tempo
                lblTempo.Text = "TEMPO:  " + formatarTempo() + "  -  VITÓRIA!";

                // Mostra a imagem de vitória
                pbTabuleiro.BackgroundImage = Image.FromFile("vitoria.jpg");

                // Estica a imagem para ocupar o tabuleiro todo
                pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;

                // Esconde o Pac-Man
                pbPACMAN.Visible = false;

                // Esconde os fantasmas e power-ups
                for (int i = 0; i < 4; i++)
                {
                    // Esconde a caixa do fantasma (ele é desenhado no tabuleiro)
                    pbFantasmas[i].Visible = false;

                    // Esconde a caixa do power-up (ele é desenhado no tabuleiro)
                    pbPowers[i].Visible = false;
                }
            }

            // Senão, se o tempo acabou ou foi pego: GAME OVER
            else if ((tempo >= tempolimite) || (pego == true))
            {
                // Para o relógio do jogo
                timer1.Enabled = false;

                // Para o relógio dos fantasmas
                timerFantasmas.Enabled = false;

                // Botão volta a ser INICIALIZAR
                btnInicializar.Text = "INICIALIZAR";

                // Escreve GAME OVER ao lado do tempo
                lblTempo.Text = "TEMPO:  " + formatarTempo() + "  -  GAME OVER";

                // Mostra a imagem de game over
                pbTabuleiro.BackgroundImage = Image.FromFile("gameover.jpg");

                // Estica a imagem para ocupar o tabuleiro todo
                pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;

                // Esconde o Pac-Man
                pbPACMAN.Visible = false;

                // Esconde os fantasmas e power-ups
                for (int i = 0; i < 4; i++)
                {
                    // Esconde a caixa do fantasma (ele é desenhado no tabuleiro)
                    pbFantasmas[i].Visible = false;

                    // Esconde a caixa do power-up (ele é desenhado no tabuleiro)
                    pbPowers[i].Visible = false;
                }
            }
        }

        // Desenha o labirinto, as bolinhas, os power-ups e os fantasmas
        private void pbTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            // Só desenha se o jogo está rodando ou pausado
            if ((timer1.Enabled == true) || (pausado == true))
            {
                // Caneta preta (contorno das paredes)
                Pen pen = new Pen(Color.Black, 1);

                // Pincel preto (pinta as paredes)
                SolidBrush bs = new SolidBrush(Color.Black);

                // Caneta cinza (contorno dos caminhos)
                Pen caminhopen = new Pen(Color.Gainsboro, 1);

                // Pincel cinza (pinta os caminhos)
                SolidBrush caminhobs = new SolidBrush(Color.Gainsboro);

                // Caneta das casas já visitadas (mesma cor do caminho)
                Pen brancopen = new Pen(Color.Gainsboro, 1);

                // Pincel das casas já visitadas
                SolidBrush brancobs = new SolidBrush(Color.Gainsboro);

                // Para cada linha do mapa...
                for (int lin = 0; lin < lintabuleiro; lin++)
                {
                    // ...e cada coluna dessa linha
                    for (int col = 0; col < coltabuleiro; col++)
                    {
                        // Se é parede (*)
                        if (tabuleiro[lin][col] == '*')
                        {
                            // Desenha a borda da casa
                            e.Graphics.DrawRectangle(pen, col * altura, lin * largura, altura, largura);

                            // Pinta a casa
                            e.Graphics.FillRectangle(bs, col * altura, lin * largura, altura, largura);
                        }

                        // Se tem bolinha ou power-up
                        if ((tabuleiro[lin][col] == '.') || (tabuleiro[lin][col] == 'P'))
                        {
                            // Desenha a borda da casa
                            e.Graphics.DrawRectangle(caminhopen, col * altura, lin * largura, altura, largura);

                            // Pinta a casa
                            e.Graphics.FillRectangle(caminhobs, col * altura, lin * largura, altura, largura);
                        }

                        // Se tem bolinha
                        if (tabuleiro[lin][col] == '.')
                        {
                            // Largura da bolinha (45% da casa)
                            int tamBolaL = largura * 9 / 20;

                            // Altura da bolinha (45% da casa)
                            int tamBolaA = altura * 9 / 20;

                            // Desenha a bolinha no meio da casa
                            e.Graphics.DrawImage(imgBola, col * largura + (largura - tamBolaL) / 2, lin * altura + (altura - tamBolaA) / 2, tamBolaL, tamBolaA);
                        }

                        // Se é caminho vazio (espaço)
                        if (tabuleiro[lin][col] == ' ')
                        {
                            // Desenha a borda da casa
                            e.Graphics.DrawRectangle(brancopen, col * altura, lin * largura, altura, largura);

                            // Pinta a casa
                            e.Graphics.FillRectangle(brancobs, col * altura, lin * largura, altura, largura);
                        }
                    }
                }

                // Para cada um dos 4 power-ups
                for (int i = 0; i < 4; i++)
                {
                    // Se esse power-up ainda não foi pego
                    if (tabuleiro[powerlin[i]][powercol[i]] == 'P')
                    {
                        // Desenha o power-up
                        e.Graphics.DrawImage(imgPower, powercol[i] * largura, powerlin[i] * altura, largura, altura);
                    }
                }

                // Para cada um dos 4 fantasmas
                for (int i = 0; i < 4; i++)
                {
                    // Desenha o fantasma onde ele está
                    e.Graphics.DrawImage(imgFantasmas[i], fantasmas[i].getFantasmaCol() * largura, fantasmas[i].getFantasmaLin() * altura, largura, altura);
                }
            }
        }

        // Tecla pressionada na janela (não usado)
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        // Texto da caixa mudou (não usado)
        private void txbTeclado_TextChanged(object sender, EventArgs e)
        {
        }

        // Lê as teclas W, A, S, D
        private void txbTeclado_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se o jogo não está rodando, ignora a tecla
            if (timer1.Enabled == false) return;

            // Se a tecla é W, A, S ou D
            if ((e.KeyChar == 'w') || (e.KeyChar == 's') || (e.KeyChar == 'a') || (e.KeyChar == 'd'))
            {
                // Guarda a direção
                tecla = e.KeyChar;
            }
        }

        // Pausa o jogo
        private void pausarJogo()
        {
            // Marca como pausado
            pausado = true;

            // Para o relógio do jogo
            timer1.Enabled = false;

            // Para o relógio dos fantasmas
            timerFantasmas.Enabled = false;

            // Botão vira CONTINUAR
            btnInicializar.Text = "CONTINUAR";

            // Manda as teclas para a caixa de teclado
            txbTeclado.Focus();
        }

        // Continua o jogo
        private void continuarJogo()
        {
            // Marca como não pausado
            pausado = false;

            // Botão vira PAUSAR
            btnInicializar.Text = "PAUSAR";

            // Liga o relógio do jogo
            timer1.Enabled = true;

            // Liga o relógio dos fantasmas
            timerFantasmas.Enabled = true;

            // Manda as teclas para a caixa de teclado
            txbTeclado.Focus();
        }

        // Lê as setas e o ENTER
        private void txbTeclado_KeyDown(object sender, KeyEventArgs e)
        {
            // Se o jogo está pausado, só o ENTER funciona
            if (pausado == true)
            {
                // Se apertou ENTER
                if (e.KeyCode == Keys.Enter)
                {
                    // Continua o jogo
                    continuarJogo();
                }

                // Evita o "bip" do ENTER
                e.SuppressKeyPress = true;

                // Sai daqui
                return;
            }

            // Se o jogo não está rodando, ignora a tecla
            if (timer1.Enabled == false) return;

            // Se apertou ENTER durante o jogo: pausa
            if (e.KeyCode == Keys.Enter)
            {
                // Pausa o jogo
                pausarJogo();

                // Evita o "bip" do ENTER
                e.SuppressKeyPress = true;

                // Sai daqui
                return;
            }

            // Seta para cima
            if (e.KeyCode == Keys.Up)
            {
                // Direção: cima
                tecla = 'w';
            }

            // Seta para baixo
            if (e.KeyCode == Keys.Down)
            {
                // Direção: baixo
                tecla = 's';
            }

            // Seta para a esquerda
            if (e.KeyCode == Keys.Left)
            {
                // Direção: esquerda
                tecla = 'a';
            }

            // Seta para a direita
            if (e.KeyCode == Keys.Right)
            {
                // Direção: direita
                tecla = 'd';
            }
        }

        // Tecla digitada na janela (não usado)
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        // Botão INICIALIZAR / PAUSAR / CONTINUAR
        private void btnInicializar_Click(object sender, EventArgs e)
        {
            // Se o jogo está rodando
            if (timer1.Enabled == true)
            {
                // Pausa o jogo
                pausarJogo();

                // Sai daqui
                return;
            }

            // Se o jogo está pausado
            if (pausado == true)
            {
                // Continua o jogo
                continuarJogo();

                // Sai daqui
                return;
            }

            // Senão, começa um jogo novo
            iniciarPartida();
        }

        // Começa uma partida nova
        private void iniciarPartida()
        {
            // Marca como não pausado
            pausado = false;

            // Linha 0 do mapa.  * = parede | . = bolinha | P = power-up | espaço = vazio
            tabuleiro[0] = "***********************";

            // Linha 1 do mapa
            tabuleiro[1] = "*P...................P*";

            // Linha 2 do mapa
            tabuleiro[2] = "*.*********.*********.*";

            // Linha 3 do mapa
            tabuleiro[3] = "*.....................*";

            // Linha 4 do mapa
            tabuleiro[4] = "*.****.**.*.*.**.****.*";

            // Linha 5 do mapa
            tabuleiro[5] = "*.*.**.****.****.**.*.*";

            // Linha 6 do mapa
            tabuleiro[6] = "*.*.**.*..*.*..*.**.*.*";

            // Linha 7 do mapa
            tabuleiro[7] = "*...**.*.......*.**...*";

            // Linha 8 do mapa
            tabuleiro[8] = "*.*.*....**.**....*.*.*";

            // Linha 9 do mapa
            tabuleiro[9] = "*.....**.*...*.**.....*";

            // Linha 10 do mapa
            tabuleiro[10] = "*.***....*...*....***.*";

            // Linha 11 do mapa
            tabuleiro[11] = "*.....**.*...*.**.....*";

            // Linha 12 do mapa
            tabuleiro[12] = "*.*.*....**.**....*.*.*";

            // Linha 13 do mapa
            tabuleiro[13] = "*...**.*.......*.**...*";

            // Linha 14 do mapa
            tabuleiro[14] = "*.*.**.*..*.*..*.**.*.*";

            // Linha 15 do mapa
            tabuleiro[15] = "*.*.**.****.****.**.*.*";

            // Linha 16 do mapa
            tabuleiro[16] = "*.*.**.**.*.*.**.****.*";

            // Linha 17 do mapa
            tabuleiro[17] = "*.....................*";

            // Linha 18 do mapa
            tabuleiro[18] = "*.*********.*********.*";

            // Linha 19 do mapa
            tabuleiro[19] = "*P......... .........P*";

            // Linha 20 do mapa
            tabuleiro[20] = "***********************";

            // Contador de power-ups achados
            int qtd = 0;

            // Para cada linha do mapa...
            for (int lin = 0; lin < lintabuleiro; lin++)
            {
                // ...e cada coluna dessa linha
                for (int col = 0; col < coltabuleiro; col++)
                {
                    // Se tem power-up
                    if (tabuleiro[lin][col] == 'P')
                    {
                        // Guarda a linha do power-up
                        powerlin[qtd] = lin;

                        // Guarda a coluna do power-up
                        powercol[qtd] = col;

                        // +1 power-up achado
                        qtd++;
                    }
                }
            }

            // Botão vira PAUSAR
            btnInicializar.Text = "PAUSAR";

            // Manda as teclas para a caixa de teclado
            txbTeclado.Focus();

            // Liga o relógio do jogo
            timer1.Enabled = true;

            // O Pac-Man anda a cada 300 ms
            timer1.Interval = 300;

            // Casas por segundo do Pac-Man (1000 ms / 300 ms = 3,33)
            double velPacman = 1000.0 / timer1.Interval;

            // Fantasma anda sempre 1 casa por segundo a menos que o Pac-Man
            double velFantasma = velPacman - 1;

            // Converte de volta para ms por passo (300 ms -> 429 ms)
            timerFantasmas.Interval = (int)Math.Round(1000.0 / velFantasma);

            // Ninguém foi pego ainda
            pego = false;

            // Liga o relógio dos fantasmas
            timerFantasmas.Enabled = true;

            // Redesenha o tabuleiro
            pbTabuleiro.Invalidate();

            // Zera o tempo
            tempo = 0;

            // Mostra o tempo na tela
            lblTempo.Text = "TEMPO:  " + formatarTempo();

            // Zera os pontos
            pontos = 0;

            // Tira o poder
            poder = 0;

            // Pac-Man começa na coluna 11
            cursorcol = 11;

            // Pac-Man começa na linha 19
            cursorlin = 19;

            // Pac-Man começa parado
            tecla = ' ';

            // Volta os 4 fantasmas para o início
            for (int i = 0; i < 4; i++)
            {
                // Coloca o fantasma nessa posição
                fantasmas[i].setPosicaoInicial(fantasmalininicial[i], fantasmacolinicial[i]);
            }
        }
    }

    // Classe do fantasma: guarda a posição e faz ele andar
    public class FANTASMA
    {
        // Coluna e linha onde o fantasma está
        private int fantasmacol, fantasmalin;

        // Largura de cada casa, em pixels
        private int largura = 20;

        // Altura de cada casa, em pixels
        private int altura = 20;

        // 1 = anda a cada tique do relógio dos fantasmas
        private int MAXATRASO = 1;

        // Ciclos que faltam para o fantasma andar
        private int atraso;

        // Gerador de números aleatórios
        private Random rnd = new Random();

        // Última direção do fantasma (-1 = nenhuma)
        private int ultimadir = -1;

        // Cria um fantasma numa posição padrão
        public FANTASMA()
        {
            // Coluna do fantasma
            fantasmacol = 20;

            // Linha do fantasma
            fantasmalin = 5;

            // Reinicia a espera até o próximo passo
            atraso = MAXATRASO;
        }

        // Cria um fantasma na linha e coluna informadas
        public FANTASMA(int lin, int col)
        {
            // Coluna do fantasma
            fantasmacol = col;

            // Linha do fantasma
            fantasmalin = lin;

            // Reinicia a espera até o próximo passo
            atraso = MAXATRASO;
        }

        // Coloca o fantasma numa posição
        public void setPosicaoInicial(int lin, int col)
        {
            // Coluna do fantasma
            fantasmacol = col;

            // Linha do fantasma
            fantasmalin = lin;
        }

        // Faz o fantasma FUGIR do Pac-Man
        public void setFugir(string[] tb, int ln, int cl)
        {
            // Diminui a espera
            atraso--;

            // Só anda quando a espera chega a zero
            if (atraso <= 0)
            {
                // Distância atual até o Pac-Man (a maior até agora)
                double melhor = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin) * (ln - fantasmalin));

                // Melhor direção para fugir (-1 = ficar parado)
                int escolha = -1;

                // Distância de teste
                double d;

                // Se em CIMA não tem parede
                if (tb[fantasmalin - 1][fantasmacol] != '*')
                {
                    // Distância até o Pac-Man se subir
                    d = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin + 1) * (ln - fantasmalin + 1));

                    // Se fica mais longe do que a melhor opção até agora
                    if (d > melhor)
                    {
                        // Guarda a nova maior distância
                        melhor = d;

                        // Melhor opção: cima
                        escolha = 0;
                    }
                }

                // Se em BAIXO não tem parede
                if (tb[fantasmalin + 1][fantasmacol] != '*')
                {
                    // Distância até o Pac-Man se descer
                    d = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin - 1) * (ln - fantasmalin - 1));

                    // Se fica mais longe do que a melhor opção até agora
                    if (d > melhor)
                    {
                        // Guarda a nova maior distância
                        melhor = d;

                        // Melhor opção: baixo
                        escolha = 1;
                    }
                }

                // Se à ESQUERDA não tem parede
                if (tb[fantasmalin][fantasmacol - 1] != '*')
                {
                    // Distância até o Pac-Man se for para a esquerda
                    d = Math.Sqrt((cl - fantasmacol + 1) * (cl - fantasmacol + 1) + (ln - fantasmalin) * (ln - fantasmalin));

                    // Se fica mais longe do que a melhor opção até agora
                    if (d > melhor)
                    {
                        // Guarda a nova maior distância
                        melhor = d;

                        // Melhor opção: esquerda
                        escolha = 2;
                    }
                }

                // Se à DIREITA não tem parede
                if (tb[fantasmalin][fantasmacol + 1] != '*')
                {
                    // Distância até o Pac-Man se for para a direita
                    d = Math.Sqrt((cl - fantasmacol - 1) * (cl - fantasmacol - 1) + (ln - fantasmalin) * (ln - fantasmalin));

                    // Se fica mais longe do que a melhor opção até agora
                    if (d > melhor)
                    {
                        // Guarda a nova maior distância
                        melhor = d;

                        // Melhor opção: direita
                        escolha = 3;
                    }
                }

                // Se escolheu cima
                if (escolha == 0)
                {
                    // Fantasma sobe 1 casa
                    fantasmalin--;
                }

                // Se escolheu baixo
                if (escolha == 1)
                {
                    // Fantasma desce 1 casa
                    fantasmalin++;
                }

                // Se escolheu esquerda
                if (escolha == 2)
                {
                    // Fantasma vai 1 casa para a esquerda
                    fantasmacol--;
                }

                // Se escolheu direita
                if (escolha == 3)
                {
                    // Fantasma vai 1 casa para a direita
                    fantasmacol++;
                }

                // Esquece a última direção
                ultimadir = -1;

                // Reinicia a espera até o próximo passo
                atraso = MAXATRASO;
            }
        }

        // Esquece a última direção do fantasma
        public void zerarDirecao()
        {
            // Esquece a última direção
            ultimadir = -1;
        }

        // Faz o fantasma andar sem rumo (direção sorteada)
        public void setMoverLivre(string[] tb)
        {
            // Diminui a espera
            atraso--;

            // Só anda quando a espera chega a zero
            if (atraso <= 0)
            {
                // Lista das direções livres
                int[] opcoes = new int[4];

                // Quantas direções livres achou
                int quantas = 0;

                // Direção de volta (para não ficar indo e voltando)
                int inversa = -1;

                // Se a direção é cima (0)
                if (ultimadir == 0)
                {
                    // Voltar seria descer
                    inversa = 1;
                }

                // Se a direção é baixo (1)
                if (ultimadir == 1)
                {
                    // Voltar seria subir
                    inversa = 0;
                }

                // Se a direção é esquerda (2)
                if (ultimadir == 2)
                {
                    // Voltar seria ir para a direita
                    inversa = 3;
                }

                // Se a direção é direita (3)
                if (ultimadir == 3)
                {
                    // Voltar seria ir para a esquerda
                    inversa = 2;
                }

                // Tenta 2 vezes achar uma direção livre
                for (int tentativa = 0; tentativa < 2; tentativa++)
                {
                    // Se ainda não achou nenhuma direção livre
                    if (quantas == 0)
                    {
                        // Se é a 2ª tentativa
                        if (tentativa == 1)
                        {
                            // Na 2ª tentativa, deixa voltar
                            inversa = -1;
                        }

                        // Se CIMA está livre (e não é voltar)
                        if ((tb[fantasmalin - 1][fantasmacol] != '*') && (inversa != 0))
                        {
                            // Guarda a opção cima
                            opcoes[quantas] = 0;

                            // +1 direção livre
                            quantas++;
                        }

                        // Se BAIXO está livre (e não é voltar)
                        if ((tb[fantasmalin + 1][fantasmacol] != '*') && (inversa != 1))
                        {
                            // Guarda a opção baixo
                            opcoes[quantas] = 1;

                            // +1 direção livre
                            quantas++;
                        }

                        // Se ESQUERDA está livre (e não é voltar)
                        if ((tb[fantasmalin][fantasmacol - 1] != '*') && (inversa != 2))
                        {
                            // Guarda a opção esquerda
                            opcoes[quantas] = 2;

                            // +1 direção livre
                            quantas++;
                        }

                        // Se DIREITA está livre (e não é voltar)
                        if ((tb[fantasmalin][fantasmacol + 1] != '*') && (inversa != 3))
                        {
                            // Guarda a opção direita
                            opcoes[quantas] = 3;

                            // +1 direção livre
                            quantas++;
                        }
                    }
                }

                // Se achou alguma direção livre
                if (quantas > 0)
                {
                    // Sorteia uma delas
                    ultimadir = opcoes[rnd.Next(quantas)];

                    // Se a direção é cima (0)
                    if (ultimadir == 0)
                    {
                        // Fantasma sobe 1 casa
                        fantasmalin--;
                    }

                    // Se a direção é baixo (1)
                    if (ultimadir == 1)
                    {
                        // Fantasma desce 1 casa
                        fantasmalin++;
                    }

                    // Se a direção é esquerda (2)
                    if (ultimadir == 2)
                    {
                        // Fantasma vai 1 casa para a esquerda
                        fantasmacol--;
                    }

                    // Se a direção é direita (3)
                    if (ultimadir == 3)
                    {
                        // Fantasma vai 1 casa para a direita
                        fantasmacol++;
                    }
                }

                // Reinicia a espera até o próximo passo
                atraso = MAXATRASO;
            }
        }

        // Faz o fantasma PERSEGUIR o Pac-Man
        public void setMover(string[] tb, int ln, int cl)
        {
            // Diferença de colunas e de linhas até o Pac-Man
            int deltacol, deltalin;

            // Valores para comparar as 4 direções
            double d0, d1, d2, d3;

            // Colunas até o Pac-Man
            deltacol = cl - fantasmacol;

            // Linhas até o Pac-Man
            deltalin = ln - fantasmalin;

            // Diminui a espera
            atraso--;

            // Só anda quando a espera chega a zero
            if (atraso <= 0)
            {
                /*
                if ((Math.Abs(deltacol) >= Math.Abs(deltalin)) && (deltacol >= 0) && (tb[fantasmalin][fantasmacol + 1] != '*'))
                {
                    fantasmacol++;
                }
                else
                if ((Math.Abs(deltacol) >= Math.Abs(deltalin)) && (deltacol <= 0) && (tb[fantasmalin][fantasmacol - 1] != '*'))
                {
                    fantasmacol--;
                }
                if ((Math.Abs(deltacol) <= Math.Abs(deltalin)) && (deltalin >= 0) && (tb[fantasmalin + 1][fantasmacol] != '*'))
                {
                    fantasmalin++;
                }
                else
                if ((Math.Abs(deltacol) <= Math.Abs(deltalin)) && (deltalin <= 0) && (tb[fantasmalin - 1][fantasmacol] != '*'))
                {
                    fantasmalin--;
                }
                else
                {
                */

                    // Valor para testar ir para a DIREITA
                    d0 = Math.Sqrt( (cl - fantasmacol+1)*(cl - fantasmacol + 1)+ (ln - fantasmalin)* (ln - fantasmalin) );

                    // Valor para testar ir para a ESQUERDA
                    d1 = Math.Sqrt((cl - fantasmacol - 1) * (cl - fantasmacol - 1) + (ln - fantasmalin) * (ln - fantasmalin));

                    // Valor para testar ir para BAIXO
                    d2 = Math.Sqrt((cl - fantasmacol ) * (cl - fantasmacol) + (ln - fantasmalin+1) * (ln - fantasmalin+1));

                    // Valor para testar ir para CIMA
                    d3 = Math.Sqrt((cl - fantasmacol ) * (cl - fantasmacol) + (ln - fantasmalin-1) * (ln - fantasmalin-1));

                    // Se ir para a DIREITA é o melhor e não tem parede
                    if ((d0 >= d1) && (d0 >= d2) && (d0 >= d3) && (tb[fantasmalin][fantasmacol + 1] != '*'))
                    {
                        // Fantasma vai 1 casa para a direita
                        fantasmacol++;
                    }

                    // Senão, se ir para a ESQUERDA é o melhor e não tem parede
                    else if ((d1 >= d0) && (d1 >= d2) && (d1 >= d3) && (tb[fantasmalin][fantasmacol - 1] != '*'))
                    {
                        // Fantasma vai 1 casa para a esquerda
                        fantasmacol--;
                    }

                    // Senão, se ir para BAIXO é o melhor e não tem parede
                    else if ((d2 >= d0) && (d2 >= d1) && (d2 >= d3) && (tb[fantasmalin + 1][fantasmacol] != '*'))
                    {
                        // Fantasma desce 1 casa
                        fantasmalin++;
                    }

                    // Senão, se ir para CIMA é o melhor e não tem parede
                    else if ((d3 >= d0) && (d3 >= d1) && (d3 >= d2) && (tb[fantasmalin - 1][fantasmacol] != '*'))
                    {
                        // Fantasma sobe 1 casa
                        fantasmalin--;
                    }

                    // Caso contrário
                    else
                    {
                        // Se o melhor caminho tem parede, sorteia uma direção
                        switch (rnd.Next(1000) % 125)
                        {
                            // Sorteou 4 ou 0...
                            case 4:

                            // ...tenta ir para a DIREITA
                            case 0:

                                // Se à DIREITA não tem parede
                                if (tb[fantasmalin][fantasmacol + 1] != '*')
                                {
                                    // Fantasma vai 1 casa para a direita
                                    fantasmacol++;
                                }

                                // Sai do switch
                                break;

                            // Sorteou 5 ou 1...
                            case 5:

                            // ...tenta ir para a ESQUERDA
                            case 1:

                                // Se à ESQUERDA não tem parede
                                if (tb[fantasmalin][fantasmacol - 1] != '*')
                                {
                                    // Fantasma vai 1 casa para a esquerda
                                    fantasmacol--;
                                }

                                // Sai do switch
                                break;

                            // Sorteou 6 ou 2...
                            case 6:

                            // ...tenta ir para BAIXO
                            case 2:

                                // Se em BAIXO não tem parede
                                if (tb[fantasmalin + 1][fantasmacol] != '*')
                                {
                                    // Fantasma desce 1 casa
                                    fantasmalin++;
                                }

                                // Sai do switch
                                break;

                            // Qualquer outro número: tenta ir para CIMA
                            default:

                                // Se em CIMA não tem parede
                                if (tb[fantasmalin - 1][fantasmacol] != '*')
                                {
                                    // Fantasma sobe 1 casa
                                    fantasmalin--;
                                }

                                // Sai do switch
                                break;
                        }
                    //}
                }

                // Reinicia a espera até o próximo passo
                atraso = MAXATRASO;
            }
        }

        // Informa a coluna do fantasma
        public int getFantasmaCol()
        {
            // Devolve a coluna
            return fantasmacol;
        }

        // Informa a linha do fantasma
        public int getFantasmaLin()
        {
            // Devolve a linha
            return fantasmalin;
        }
    }
}
