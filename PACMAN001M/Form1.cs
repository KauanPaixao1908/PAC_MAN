using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PACMAN001M
{
    public partial class Form1 : Form
    {
        //Jogo estilo Pac-Man só que melhor
        //Kauan Paixão - RA: 26002387
        //O Palmeiras (verde) pega as taças da Libertadores (amarelo)
        //fugindo do Corinthians (preto), Flamengo (vermelho) e São Paulo (branco).
        //Pegando o Abel Ferreira (laranja) dá pra derrotar os rivais.

        int contadorTempo;          //tempo que o Abel Ferreira fica em campo
        int col = 0, lin = 0;       //posição do Palmeiras (coluna e linha do mapa)
        int direcao = 0;            //0 = parado, 1 = cima, 2 = baixo, 3 = esquerda, 4 = direita
        int direcaoDesejada = 0;    //direção do último botão apertado

        int pontos = 0;
        int vidas = 3;
        int fase = 1;
        int tacas = 0;
        int turno = 0;
        bool jogoAcabou = false;

        //tamanho de cada casa do mapa em pixels
        int tamanho = 20;

        //rivais: 0 = Corinthians, 1 = Flamengo, 2 = São Paulo
        int[] rivalCol = new int[3];
        int[] rivalLin = new int[3];
        int[] rivalDirecao = new int[3];
        string[] rivalNome = { "Corinthians", "Flamengo", "São Paulo" };

        Random sorteio = new Random();

        //mapa: # = parede, . = taça, A = Abel Ferreira
        string[] mapaTexto =
        {
            "############################",
            "#............##............#",
            "#A####.#####.##.#####.####A#",
            "#..........................#",
            "#.####.##.########.##.####.#",
            "#......##....##....##......#",
            "######.#####.##.#####.######",
            "######.##          ##.######",
            "######.## ###  ### ##.######",
            "######.   #      #   .######",
            "######.## ######## ##.######",
            "######.##          ##.######",
            "######.## ######## ##.######",
            "#............##............#",
            "#.####.#####.##.#####.####.#",
            "#A..##................##..A#",
            "###.##.##.########.##.##.###",
            "#......##....##....##......#",
            "#.##########.##.##########.#",
            "#..........................#",
            "############################",
        };

        //mapa que muda durante o jogo: 0 = vazio, 1 = parede, 2 = taça, 3 = Abel
        int[,] mapa = new int[21, 28];

        public Form1()
        {
            InitializeComponent();
            NovoJogo();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pdxTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            //Definir as cores das paredes
            Pen pen = new Pen(Color.Blue, 1);
            SolidBrush bs = new SolidBrush(Color.DarkBlue);
            //Cor das taças
            SolidBrush bsTaca = new SolidBrush(Color.Yellow);
            //Cor do Abel Ferreira
            SolidBrush bsAbel = new SolidBrush(Color.Orange);

            for (int l = 0; l < 21; l++)
            {
                for (int c = 0; c < 28; c++)
                {
                    int x = c * tamanho;
                    int y = l * tamanho;

                    if (mapa[l, c] == 1)
                    {
                        //Pintar a parede
                        e.Graphics.FillRectangle(bs, x, y, tamanho, tamanho);
                        e.Graphics.DrawRectangle(pen, x, y, tamanho, tamanho);
                    }
                    else if (mapa[l, c] == 2)
                    {
                        //Taça pequena no meio da casa
                        e.Graphics.FillEllipse(bsTaca, x + 7, y + 7, 6, 6);
                    }
                    else if (mapa[l, c] == 3)
                    {
                        //Abel Ferreira é uma bola maior
                        e.Graphics.FillEllipse(bsAbel, x + 3, y + 3, 14, 14);
                    }
                }
            }

            //Desenhar os rivais
            for (int i = 0; i < 3; i++)
            {
                Color corRival;
                Color corBorda;
                if (contadorTempo > 0)
                {
                    //com medo do Abel ficam azuis
                    corRival = Color.RoyalBlue;
                    corBorda = Color.White;
                }
                else if (i == 0)
                {
                    corRival = Color.Black;   //Corinthians
                    corBorda = Color.White;
                }
                else if (i == 1)
                {
                    corRival = Color.Red;     //Flamengo
                    corBorda = Color.Black;
                }
                else
                {
                    corRival = Color.White;   //São Paulo
                    corBorda = Color.Red;
                }

                SolidBrush bsRival = new SolidBrush(corRival);
                Pen penRival = new Pen(corBorda, 2);
                e.Graphics.FillRectangle(bsRival, rivalCol[i] * tamanho + 2, rivalLin[i] * tamanho + 2, 16, 16);
                e.Graphics.DrawRectangle(penRival, rivalCol[i] * tamanho + 2, rivalLin[i] * tamanho + 2, 16, 16);
            }

            //Desenhar o Palmeiras
            Pen pen2 = new Pen(Color.White, 1);
            SolidBrush bs2 = new SolidBrush(Color.Green);
            e.Graphics.FillEllipse(bs2, col * tamanho + 1, lin * tamanho + 1, 18, 18);
            e.Graphics.DrawEllipse(pen2, col * tamanho + 1, lin * tamanho + 1, 18, 18);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            turno++;

            MoverPalmeiras();
            VerificarColisao();

            //os rivais andam mais rápido a cada fase
            int velocidade = 3;
            if (fase >= 2) velocidade = 2;
            if (fase == 5) velocidade = 1;

            if (timer1.Enabled && turno % velocidade == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    MoverRival(i);
                }
                VerificarColisao();
            }

            if (contadorTempo > 0)
            {
                contadorTempo--;
                if (contadorTempo == 0)
                {
                    lblMensagem.Text = "O Abel saiu de campo!";
                }
            }

            if (timer1.Enabled && tacas == 0)
            {
                PassarDeFase();
            }

            AtualizarTextos();
            pdxTabuleiro.Invalidate();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "START")
            {
                if (jogoAcabou)
                {
                    NovoJogo();
                }
                timer1.Enabled = true;
                btnStartStop.Text = "STOP";
                lblMensagem.Text = NomeDaFase();
            }
            else
            {
                timer1.Enabled = false;
                btnStartStop.Text = "START";
                lblMensagem.Text = "PAUSADO";
            }
        }

        //Setas do teclado: ^ cima, ↓ baixo, < esquerda, > direita
        //(usa ProcessCmdKey porque os botões "pegam" as setas antes do KeyDown)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                direcaoDesejada = 1;
                return true;
            }
            if (keyData == Keys.Down)
            {
                direcaoDesejada = 2;
                return true;
            }
            if (keyData == Keys.Left)
            {
                direcaoDesejada = 3;
                return true;
            }
            if (keyData == Keys.Right)
            {
                direcaoDesejada = 4;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnCima_Click(object sender, EventArgs e)
        {
            direcaoDesejada = 1;
        }

        private void btnBaixo_Click(object sender, EventArgs e)
        {
            direcaoDesejada = 2;
        }

        private void btnEsquerda_Click(object sender, EventArgs e)
        {
            direcaoDesejada = 3;
        }

        private void btnDireita_Click(object sender, EventArgs e)
        {
            direcaoDesejada = 4;
        }

        private void NovoJogo()
        {
            pontos = 0;
            vidas = 3;
            fase = 1;
            jogoAcabou = false;
            CarregarMapa();
            PosicoesIniciais();
            lblMensagem.Text = "Aperte START para começar";
            AtualizarTextos();
            pdxTabuleiro.Invalidate();
        }

        private void CarregarMapa()
        {
            tacas = 0;
            for (int l = 0; l < 21; l++)
            {
                for (int c = 0; c < 28; c++)
                {
                    char letra = mapaTexto[l][c];
                    if (letra == '#')
                    {
                        mapa[l, c] = 1;
                    }
                    else if (letra == '.')
                    {
                        mapa[l, c] = 2;
                        tacas++;
                    }
                    else if (letra == 'A')
                    {
                        mapa[l, c] = 3;
                        tacas++;
                    }
                    else
                    {
                        mapa[l, c] = 0;
                    }
                }
            }

            //o Palmeiras começa aqui, então tira a taça desse lugar
            mapa[15, 13] = 0;
            tacas--;
        }

        private void PosicoesIniciais()
        {
            col = 13;
            lin = 15;
            direcao = 0;
            direcaoDesejada = 0;
            contadorTempo = 0;

            //os rivais começam na casinha do meio
            for (int i = 0; i < 3; i++)
            {
                rivalCol[i] = 12 + i;
                rivalLin[i] = 9;
                rivalDirecao[i] = 1;
            }
        }

        private bool EhParede(int c, int l)
        {
            return mapa[l, c] == 1;
        }

        private int ProximaCol(int c, int dir)
        {
            if (dir == 3) return c - 1;
            if (dir == 4) return c + 1;
            return c;
        }

        private int ProximaLin(int l, int dir)
        {
            if (dir == 1) return l - 1;
            if (dir == 2) return l + 1;
            return l;
        }

        private void MoverPalmeiras()
        {
            //se der pra virar para o lado do botão apertado, vira
            if (direcaoDesejada != 0 && !EhParede(ProximaCol(col, direcaoDesejada), ProximaLin(lin, direcaoDesejada)))
            {
                direcao = direcaoDesejada;
            }

            if (direcao == 0 || EhParede(ProximaCol(col, direcao), ProximaLin(lin, direcao)))
            {
                return; //parado ou bateu na parede
            }

            col = ProximaCol(col, direcao);
            lin = ProximaLin(lin, direcao);

            if (mapa[lin, col] == 2)
            {
                pontos += 10;
                tacas--;
                mapa[lin, col] = 0;
            }
            else if (mapa[lin, col] == 3)
            {
                pontos += 50;
                tacas--;
                mapa[lin, col] = 0;
                contadorTempo = 35; //mais ou menos 5 segundos
                lblMensagem.Text = "ABEL FERREIRA ENTROU EM CAMPO!";
            }
        }

        private int DirecaoContraria(int dir)
        {
            if (dir == 1) return 2;
            if (dir == 2) return 1;
            if (dir == 3) return 4;
            return 3;
        }

        private void MoverRival(int i)
        {
            int melhorDir = 0;
            int melhorDistancia = -1;
            int opcoes = 0;

            for (int dir = 1; dir <= 4; dir++)
            {
                int c = ProximaCol(rivalCol[i], dir);
                int l = ProximaLin(rivalLin[i], dir);

                //não entra na parede e não volta para trás
                if (EhParede(c, l) || dir == DirecaoContraria(rivalDirecao[i]))
                {
                    continue;
                }
                opcoes++;

                int distancia = Math.Abs(c - col) + Math.Abs(l - lin);

                bool melhor;
                if (contadorTempo > 0)
                    melhor = distancia > melhorDistancia;   //foge do Palmeiras
                else
                    melhor = melhorDistancia == -1 || distancia < melhorDistancia; //persegue

                //às vezes escolhe outro caminho sem pensar, pra não ficar impossível
                if (melhorDistancia != -1 && sorteio.Next(100) < 25)
                    melhor = !melhor;

                if (melhor)
                {
                    melhorDistancia = distancia;
                    melhorDir = dir;
                }
            }

            //se não tiver saída, volta
            if (opcoes == 0)
            {
                melhorDir = DirecaoContraria(rivalDirecao[i]);
            }

            rivalDirecao[i] = melhorDir;
            rivalCol[i] = ProximaCol(rivalCol[i], melhorDir);
            rivalLin[i] = ProximaLin(rivalLin[i], melhorDir);
        }

        private void VerificarColisao()
        {
            for (int i = 0; i < 3; i++)
            {
                if (rivalCol[i] == col && rivalLin[i] == lin)
                {
                    if (contadorTempo > 0)
                    {
                        //com o Abel o Palmeiras derrota o rival e ele volta pra casinha
                        pontos += 200;
                        lblMensagem.Text = "GOL DO VERDÃO! " + rivalNome[i] + " derrotado! +200";
                        rivalCol[i] = 13;
                        rivalLin[i] = 9;
                    }
                    else
                    {
                        vidas--;
                        if (vidas == 0)
                        {
                            timer1.Enabled = false;
                            btnStartStop.Text = "START";
                            jogoAcabou = true;
                            lblMensagem.Text = "ACABARAM AS VIDAS! O PALMEIRAS FOI REBAIXADO PARA A SÉRIE B!";
                        }
                        else
                        {
                            lblMensagem.Text = "O " + rivalNome[i] + " te pegou! Restam " + vidas + " vidas";
                            PosicoesIniciais();
                        }
                        return;
                    }
                }
            }
        }

        private void PassarDeFase()
        {
            if (fase == 5)
            {
                timer1.Enabled = false;
                btnStartStop.Text = "START";
                jogoAcabou = true;
                lblMensagem.Text = "CAMPEÃO DA LIBERTADORES! O PALMEIRAS É O MAIOR TIME DO UNIVERSO!";
                return;
            }

            fase++;
            CarregarMapa();
            PosicoesIniciais();
            timer1.Enabled = false;
            btnStartStop.Text = "START";
            lblMensagem.Text = "Classificado! Aperte START para jogar a " + NomeDaFase();
        }

        private string NomeDaFase()
        {
            if (fase == 1) return "FASE DE GRUPOS";
            if (fase == 2) return "OITAVAS DE FINAL";
            if (fase == 3) return "QUARTAS DE FINAL";
            if (fase == 4) return "SEMIFINAL";
            return "GRANDE FINAL";
        }

        private void AtualizarTextos()
        {
            lblPlacar.Text = "PONTOS: " + pontos.ToString() + "   VIDAS: " + vidas.ToString() + "   TAÇAS: " + tacas.ToString();
            lblFase.Text = "FASE " + fase.ToString() + " DE 5: " + NomeDaFase();
            lblTemporizador.Text = "CONTADOR ABEL: " + contadorTempo.ToString();
        }
    }
}
