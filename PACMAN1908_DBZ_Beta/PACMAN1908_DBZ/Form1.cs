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


namespace PACMAN001
{
    public partial class Form1 : Form
    {
        string[,] pacman = new string[20, 10];
                
        string[] tabuleiro = new string[100];
        int lintabuleiro = 21;
        int coltabuleiro = 23;
        int largura = 20;
        int altura = 20;
        int cursorcol = 1;
        int cursorlin = 1;
        char tecla = ' ';
        int tempo = 0;
        int tempolimite = 900;
        int tempoliberar = 10;
        int raiopixels = 250;
        int pontos = 0;
        int flagimagempacman = 0;

        int poder = 0;
        int[] fantasmalininicial = new int[4];
        int[] fantasmacolinicial = new int[4];
        int[] powerlin = new int[4];
        int[] powercol = new int[4];
        PictureBox[] pbFantasmas = new PictureBox[4];
        PictureBox[] pbPowers = new PictureBox[4];

        //DEFINIR 20 FANTASMANS
        FANTASMA[] fantasmas=new FANTASMA[20];
            
        public Form1()
        {
            InitializeComponent();
            Form1_Resize(this, EventArgs.Empty);

            pbTabuleiro.BackgroundImage = Image.FromFile("abertura.png");

            pacman[0, 0] = "w";
            pacman[0, 1] = "pacman_cima.png";
            pacman[0, 2] = "pacman_cima_2.png";

            pacman[1, 0] = "s";
            pacman[1, 1] = "pacman_baixo.png";
            pacman[1, 2] = "pacman_baixo_2.png";

            pacman[2, 0] = "a";
            pacman[2, 1] = "pacman_Esquerda.png";
            pacman[2, 2] = "pacman_Esquerda_2.png";

            pacman[3, 0] = "d";
            pacman[3, 1] = "pacman_Direita.png";
            pacman[3, 2] = "pacman_Direita_2.png";

            fantasmalininicial[0] = 9;
            fantasmacolinicial[0] = 11;
            fantasmalininicial[1] = 10;
            fantasmacolinicial[1] = 11;
            fantasmalininicial[2] = 11;
            fantasmacolinicial[2] = 11;
            fantasmalininicial[3] = 12;
            fantasmacolinicial[3] = 11;

            pbFantasmas[0] = pbFantasma0;
            pbFantasmas[1] = pbFantasma1;
            pbFantasmas[2] = pbFantasma2;
            pbFantasmas[3] = pbFantasma3;
            pbPowers[0] = pbPower0;
            pbPowers[1] = pbPower1;
            pbPowers[2] = pbPower2;
            pbPowers[3] = pbPower3;

            pbFantasmas[0].BackgroundImage = Image.FromFile("Fantasma_Fluminense.png");
            pbFantasmas[1].BackgroundImage = Image.FromFile("fantasma_corinthians.png");
            pbFantasmas[2].BackgroundImage = Image.FromFile("fantasma_flamengo.png");
            pbFantasmas[3].BackgroundImage = Image.FromFile("fantasma_sao_paulo.png");
            for (int k = 0; k < 4; k++)
            {
                pbPowers[k].BackgroundImage = Image.FromFile("power_up_Abel_Ferreira.png");
            }

            for (int i = 0; i < 4; i++)
            {
                fantasmas[i] = new FANTASMA(fantasmalininicial[i], fantasmacolinicial[i]);
            }

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if ((ClientSize.Width < 100) || (ClientSize.Height < 200)) return;

            int tam = (ClientSize.Height - 90) / lintabuleiro;
            if ((ClientSize.Width - 24) / coltabuleiro < tam)
            {
                tam = (ClientSize.Width - 24) / coltabuleiro;
            }
            if (tam < 10)
            {
                tam = 10;
            }
            largura = tam;
            altura = tam;

            int esq = (ClientSize.Width - coltabuleiro * tam) / 2;
            pbTabuleiro.Location = new Point(esq, 12);
            pbTabuleiro.Size = new Size(coltabuleiro * tam, lintabuleiro * tam);
            btnInicializar.Location = new Point(esq, 24 + lintabuleiro * tam);
            btnSair.Location = new Point(esq + coltabuleiro * tam - btnSair.Width, 24 + lintabuleiro * tam);
            lblTempo.Location = new Point(esq, 56 + lintabuleiro * tam);
            lblPontos.Location = new Point(esq + coltabuleiro * tam / 2, 56 + lintabuleiro * tam);
            txbTeclado.Location = new Point(esq + 10, 12 + lintabuleiro * tam - 24);
        }

        private string formatarTempo()
        {
            int segundos = (tempolimite - tempo) / 5;
            int minutos = segundos / 60;
            segundos = segundos % 60;
            return minutos.ToString("00") + ":" + segundos.ToString("00");
        }

        private bool posicaoOcupada(int lin, int col, int quem)
        {
            for (int j = 0; j < 4; j++)
            {
                if ((j != quem) && (fantasmas[j].getFantasmaLin() == lin) && (fantasmas[j].getFantasmaCol() == col))
                {
                    return true;
                }
            }
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempo++;
            if (poder > 0)
            {
                poder--;
            }
            if (tecla == 'w')
            {
                if ((tabuleiro[cursorlin - 1][cursorcol] == '.')||(tabuleiro[cursorlin - 1][cursorcol] == ' ') || (tabuleiro[cursorlin - 1][cursorcol] == 'P'))
                {
                    cursorlin--;
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        pontos++;
                    }
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        poder = 25;
                        pontos = pontos + 10;
                    }
                    var sb = new StringBuilder(tabuleiro[cursorlin]);
                    sb[cursorcol] = ' ';
                    tabuleiro[cursorlin]=sb.ToString();
                }
            }
            if (tecla == 's')
            {
                if ((tabuleiro[cursorlin + 1][cursorcol] == '.')|| (tabuleiro[cursorlin + 1][cursorcol] == ' ') || (tabuleiro[cursorlin + 1][cursorcol] == 'P'))
                {
                    cursorlin++;
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        pontos++;
                    }
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        poder = 25;
                        pontos = pontos + 10;
                    }
                    var sb = new StringBuilder(tabuleiro[cursorlin]);
                    sb[cursorcol] = ' ';
                    tabuleiro[cursorlin] = sb.ToString();  
                }
            }
            if (tecla == 'a')
            {
                if ((tabuleiro[cursorlin][cursorcol-1] == '.') || (tabuleiro[cursorlin][cursorcol-1] == ' ') || (tabuleiro[cursorlin][cursorcol-1] == 'P'))
                {
                    cursorcol--;
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        pontos++;
                    }
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        poder = 25;
                        pontos = pontos + 10;
                    }
                    var sb = new StringBuilder(tabuleiro[cursorlin]);
                    sb[cursorcol] = ' ';
                    tabuleiro[cursorlin] = sb.ToString();
                    
                }
            }
            if (tecla == 'd')
            {
                if ((tabuleiro[cursorlin][cursorcol + 1] == '.')|| (tabuleiro[cursorlin][cursorcol+1] == ' ') || (tabuleiro[cursorlin][cursorcol+1] == 'P'))
                {
                    cursorcol++;
                    if (tabuleiro[cursorlin][cursorcol] == '.')
                    {
                        pontos++;
                    }
                    if (tabuleiro[cursorlin][cursorcol] == 'P')
                    {
                        poder = 25;
                        pontos = pontos + 10;
                    }
                    var sb = new StringBuilder(tabuleiro[cursorlin]);
                    sb[cursorcol] = ' ';
                    tabuleiro[cursorlin] = sb.ToString();
                }
            }
            bool pego = false;
            for (int passo = 0; passo < 2; passo++)
            {
                if (passo == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (tempo >= tempoliberar)
                        {
                            int linantes = fantasmas[i].getFantasmaLin();
                            int colantes = fantasmas[i].getFantasmaCol();
                            double dx = (cursorcol - fantasmas[i].getFantasmaCol()) * largura;
                            double dy = (cursorlin - fantasmas[i].getFantasmaLin()) * altura;
                            if (poder > 0)
                            {
                                fantasmas[i].setFugir(tabuleiro, cursorlin, cursorcol);
                            }
                            else
                            if (Math.Sqrt(dx * dx + dy * dy) <= raiopixels)
                            {
                                fantasmas[i].setMover(tabuleiro, cursorlin, cursorcol);
                            }
                            else
                            {
                                fantasmas[i].setMoverLivre(tabuleiro);
                            }
                            if (posicaoOcupada(fantasmas[i].getFantasmaLin(), fantasmas[i].getFantasmaCol(), i) == true)
                            {
                                fantasmas[i].setPosicaoInicial(linantes, colantes);
                                fantasmas[i].zerarDirecao();
                            }
                        }
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if ((cursorcol == fantasmas[i].getFantasmaCol()) && (cursorlin == fantasmas[i].getFantasmaLin()))
                    {
                        if (poder > 0)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                int idx = (i + k) % 4;
                                if (posicaoOcupada(fantasmalininicial[idx], fantasmacolinicial[idx], i) == false)
                                {
                                    fantasmas[i].setPosicaoInicial(fantasmalininicial[idx], fantasmacolinicial[idx]);
                                    break;
                                }
                            }
                            pontos = pontos + 15;
                        }
                        else
                        {
                            pego = true;
                        }
                    }
                }
            }
            int dir = 0;
            switch (tecla)
            {
                case 'w':
                    dir = 0;
                    break;
                case 's':
                    dir = 1;
                    break;
                case 'a':
                    dir = 2;
                    break;
                case 'd':
                    dir = 3;
                    break;
            }
            pbPACMAN.BackgroundImage = Image.FromFile(pacman[dir, flagimagempacman + 1]);
            flagimagempacman = Math.Abs(flagimagempacman - 1);
            pbPACMAN.Size = new Size(largura, altura);
            pbPACMAN.Location = new Point(cursorcol * largura, cursorlin * altura);
            pbPACMAN.Visible = true;
            for (int i = 0; i < 4; i++)
            {
                pbFantasmas[i].Size = new Size(largura, altura);
                pbFantasmas[i].Location = new Point(fantasmas[i].getFantasmaCol() * largura, fantasmas[i].getFantasmaLin() * altura);
                pbFantasmas[i].Visible = true;
                if (poder > 0)
                {
                    pbFantasmas[i].BackColor = Color.Blue;
                }
                else
                {
                    pbFantasmas[i].BackColor = Color.Transparent;
                }
                pbPowers[i].Size = new Size(largura, altura);
                pbPowers[i].Location = new Point(powercol[i] * largura, powerlin[i] * altura);
                if (tabuleiro[powerlin[i]][powercol[i]] == 'P')
                {
                    pbPowers[i].Visible = true;
                }
                else
                {
                    pbPowers[i].Visible = false;
                }
            }
            int restantes = 0;
            for (int lin = 0; lin < lintabuleiro; lin++)
            {
                for (int col = 0; col < coltabuleiro; col++)
                {
                    if ((tabuleiro[lin][col] == '.') || (tabuleiro[lin][col] == 'P'))
                    {
                        restantes++;
                    }
                }
            }
            lblTempo.Text = "TEMPO:  " + formatarTempo();
            lblPontos.Text="PONTOS: "+ pontos.ToString();
            txbTeclado.Focus();
            pbTabuleiro.Invalidate();
            if ((restantes == 0) && (pego == false))
            {
                timer1.Enabled = false;
                lblTempo.Text = "TEMPO:  " + formatarTempo() + "  -  VITÓRIA!";
                pbTabuleiro.BackgroundImage = Image.FromFile("vitoria.jpg");
                pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;
                pbPACMAN.Visible = false;
                for (int i = 0; i < 4; i++)
                {
                    pbFantasmas[i].Visible = false;
                    pbPowers[i].Visible = false;
                }
            }
            else
            if ((tempo >= tempolimite) || (pego == true))
            {
                timer1.Enabled = false;
                lblTempo.Text = "TEMPO:  " + formatarTempo() + "  -  GAME OVER";
                pbTabuleiro.BackgroundImage = Image.FromFile("gameover.jpg");
                pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;
                pbPACMAN.Visible = false;
                for (int i = 0; i < 4; i++)
                {
                    pbFantasmas[i].Visible = false;
                    pbPowers[i].Visible = false;
                }
            }

            
        }

        private void pbTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            if (timer1.Enabled == true)
            {
                Pen pen = new Pen(Color.Black, 1);
                SolidBrush bs = new SolidBrush(Color.Black);
                Pen caminhopen = new Pen(Color.Yellow, 1);
                SolidBrush caminhobs = new SolidBrush(Color.Yellow);
                Pen brancopen = new Pen(Color.White, 1);
                SolidBrush brancobs = new SolidBrush(Color.White);
                for (int lin = 0; lin < lintabuleiro; lin++)
                {
                    for (int col = 0; col < coltabuleiro; col++)
                    {
                        if (tabuleiro[lin][col] == '*')
                        {
                            e.Graphics.DrawRectangle(pen, col * altura, lin * largura, altura, largura);
                            e.Graphics.FillRectangle(bs, col * altura, lin * largura, altura, largura);

                        }
                        if ((tabuleiro[lin][col] == '.') || (tabuleiro[lin][col] == 'P'))
                        {
                            e.Graphics.DrawRectangle(caminhopen, col * altura, lin * largura, altura, largura);
                            e.Graphics.FillRectangle(caminhobs, col * altura, lin * largura, altura, largura);

                        }
                        if (tabuleiro[lin][col] == ' ')
                        {
                            e.Graphics.DrawRectangle(brancopen, col * altura, lin * largura, altura, largura);
                            e.Graphics.FillRectangle(brancobs, col * altura, lin * largura, altura, largura);

                        }
                    }
                }

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txbTeclado_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbTeclado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (timer1.Enabled == false) return;

            if ((e.KeyChar == 'w') || (e.KeyChar == 's') || (e.KeyChar == 'a') || (e.KeyChar == 'd'))
            {
                tecla = e.KeyChar;
            }
        }

        private void txbTeclado_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer1.Enabled == false) return;

            if (e.KeyCode == Keys.Up)
            {
                tecla = 'w';
            }
            if (e.KeyCode == Keys.Down)
            {
                tecla = 's';
            }
            if (e.KeyCode == Keys.Left)
            {
                tecla = 'a';
            }
            if (e.KeyCode == Keys.Right)
            {
                tecla = 'd';
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnInicializar_Click(object sender, EventArgs e)
        {
            tabuleiro[0] = "***********************";
            tabuleiro[1] = "*P...................P*";
            tabuleiro[2] = "*.*********.*********.*";
            tabuleiro[3] = "*.....................*";
            tabuleiro[4] = "*.****.**.*.*.**.****.*";
            tabuleiro[5] = "*.*.**.****.****.**.*.*";
            tabuleiro[6] = "*.*.**.*..*.*..*.**.*.*";
            tabuleiro[7] = "*...**.*.......*.**...*";
            tabuleiro[8] = "*.*.*....**.**....*.*.*";
            tabuleiro[9] = "*.....**.*...*.**.....*";
            tabuleiro[10] = "*.***....*...*....***.*";
            tabuleiro[11] = "*.....**.*...*.**.....*";
            tabuleiro[12] = "*.*.*....**.**....*.*.*";
            tabuleiro[13] = "*...**.*.......*.**...*";
            tabuleiro[14] = "*.*.**.*..*.*..*.**.*.*";
            tabuleiro[15] = "*.*.**.****.****.**.*.*";
            tabuleiro[16] = "*.*.**.**.*.*.**.****.*";
            tabuleiro[17] = "*.....................*";
            tabuleiro[18] = "*.*********.*********.*";
            tabuleiro[19] = "*P......... .........P*";
            tabuleiro[20] = "***********************";
            int qtd = 0;
            for (int lin = 0; lin < lintabuleiro; lin++)
            {
                for (int col = 0; col < coltabuleiro; col++)
                {
                    if (tabuleiro[lin][col] == 'P')
                    {
                        powerlin[qtd] = lin;
                        powercol[qtd] = col;
                        qtd++;
                    }
                }
            }
            timer1.Enabled = true;
            timer1.Interval = 200;
            pbTabuleiro.Invalidate();
            tempo = 0;
            lblTempo.Text = "TEMPO:  " + formatarTempo();
            pontos = 0;
            poder = 0;
            cursorcol = 11;
            cursorlin = 19;
            tecla = ' ';
            for (int i = 0; i < 4; i++)
            {
                fantasmas[i].setPosicaoInicial(fantasmalininicial[i], fantasmacolinicial[i]);
            }
        }
    }

    public class FANTASMA
    {
        private int fantasmacol, fantasmalin;
        private int largura = 20;
        private int altura = 20;
        private int MAXATRASO = 2;
        private int atraso;
        private Random rnd = new Random();
        private int ultimadir = -1;

        public FANTASMA()
        {
            fantasmacol = 20;
            fantasmalin = 5;
            atraso = MAXATRASO;
        }
        public FANTASMA(int lin, int col)
        {
            fantasmacol = col;
            fantasmalin = lin;
            atraso = MAXATRASO;
        }

        public void setPosicaoInicial(int lin, int col)
        {
            fantasmacol = col;
            fantasmalin = lin;
        }

        public void setFugir(string[] tb, int ln, int cl)
        {
            atraso--;
            if (atraso <= 0)
            {
                double melhor = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin) * (ln - fantasmalin));
                int escolha = -1;
                double d;
                if (tb[fantasmalin - 1][fantasmacol] != '*')
                {
                    d = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin + 1) * (ln - fantasmalin + 1));
                    if (d > melhor)
                    {
                        melhor = d;
                        escolha = 0;
                    }
                }
                if (tb[fantasmalin + 1][fantasmacol] != '*')
                {
                    d = Math.Sqrt((cl - fantasmacol) * (cl - fantasmacol) + (ln - fantasmalin - 1) * (ln - fantasmalin - 1));
                    if (d > melhor)
                    {
                        melhor = d;
                        escolha = 1;
                    }
                }
                if (tb[fantasmalin][fantasmacol - 1] != '*')
                {
                    d = Math.Sqrt((cl - fantasmacol + 1) * (cl - fantasmacol + 1) + (ln - fantasmalin) * (ln - fantasmalin));
                    if (d > melhor)
                    {
                        melhor = d;
                        escolha = 2;
                    }
                }
                if (tb[fantasmalin][fantasmacol + 1] != '*')
                {
                    d = Math.Sqrt((cl - fantasmacol - 1) * (cl - fantasmacol - 1) + (ln - fantasmalin) * (ln - fantasmalin));
                    if (d > melhor)
                    {
                        melhor = d;
                        escolha = 3;
                    }
                }
                if (escolha == 0)
                {
                    fantasmalin--;
                }
                if (escolha == 1)
                {
                    fantasmalin++;
                }
                if (escolha == 2)
                {
                    fantasmacol--;
                }
                if (escolha == 3)
                {
                    fantasmacol++;
                }
                ultimadir = -1;
                atraso = MAXATRASO;
            }
        }

        public void zerarDirecao()
        {
            ultimadir = -1;
        }

        public void setMoverLivre(string[] tb)
        {
            atraso--;
            if (atraso <= 0)
            {
                int[] opcoes = new int[4];
                int quantas = 0;
                int inversa = -1;
                if (ultimadir == 0)
                {
                    inversa = 1;
                }
                if (ultimadir == 1)
                {
                    inversa = 0;
                }
                if (ultimadir == 2)
                {
                    inversa = 3;
                }
                if (ultimadir == 3)
                {
                    inversa = 2;
                }
                for (int tentativa = 0; tentativa < 2; tentativa++)
                {
                    if (quantas == 0)
                    {
                        if (tentativa == 1)
                        {
                            inversa = -1;
                        }
                        if ((tb[fantasmalin - 1][fantasmacol] != '*') && (inversa != 0))
                        {
                            opcoes[quantas] = 0;
                            quantas++;
                        }
                        if ((tb[fantasmalin + 1][fantasmacol] != '*') && (inversa != 1))
                        {
                            opcoes[quantas] = 1;
                            quantas++;
                        }
                        if ((tb[fantasmalin][fantasmacol - 1] != '*') && (inversa != 2))
                        {
                            opcoes[quantas] = 2;
                            quantas++;
                        }
                        if ((tb[fantasmalin][fantasmacol + 1] != '*') && (inversa != 3))
                        {
                            opcoes[quantas] = 3;
                            quantas++;
                        }
                    }
                }
                if (quantas > 0)
                {
                    ultimadir = opcoes[rnd.Next(quantas)];
                    if (ultimadir == 0)
                    {
                        fantasmalin--;
                    }
                    if (ultimadir == 1)
                    {
                        fantasmalin++;
                    }
                    if (ultimadir == 2)
                    {
                        fantasmacol--;
                    }
                    if (ultimadir == 3)
                    {
                        fantasmacol++;
                    }
                }
                atraso = MAXATRASO;
            }
        }

        public void setMover(string[] tb, int ln, int cl)
        {
            int deltacol, deltalin;
            double d0, d1, d2, d3;

            
            deltacol = cl - fantasmacol;
            deltalin = ln - fantasmalin;
            atraso--;
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
                    d0 = Math.Sqrt( (cl - fantasmacol+1)*(cl - fantasmacol + 1)+ (ln - fantasmalin)* (ln - fantasmalin) );
                    d1 = Math.Sqrt((cl - fantasmacol - 1) * (cl - fantasmacol - 1) + (ln - fantasmalin) * (ln - fantasmalin));
                    d2 = Math.Sqrt((cl - fantasmacol ) * (cl - fantasmacol) + (ln - fantasmalin+1) * (ln - fantasmalin+1));
                    d3 = Math.Sqrt((cl - fantasmacol ) * (cl - fantasmacol) + (ln - fantasmalin-1) * (ln - fantasmalin-1));
                    if ((d0 >= d1) && (d0 >= d2) && (d0 >= d3) && (tb[fantasmalin][fantasmacol + 1] != '*'))
                    {
                        fantasmacol++;
                    }
                    else
                    if ((d1 >= d0) && (d1 >= d2) && (d1 >= d3) && (tb[fantasmalin][fantasmacol - 1] != '*'))
                    {
                        fantasmacol--;
                    }
                    else
                    if ((d2 >= d0) && (d2 >= d1) && (d2 >= d3) && (tb[fantasmalin + 1][fantasmacol] != '*'))
                    {
                        fantasmalin++;
                    }
                    else
                    if ((d3 >= d0) && (d3 >= d1) && (d3 >= d2) && (tb[fantasmalin - 1][fantasmacol] != '*'))
                    {
                        fantasmalin--;
                    }
                    else
                    {
                        switch (rnd.Next(1000) % 125)
                        {
                            case 4:
                            case 0:
                                if (tb[fantasmalin][fantasmacol + 1] != '*')
                                {
                                    fantasmacol++;
                                }
                                break;
                            case 5:
                            case 1:
                                if (tb[fantasmalin][fantasmacol - 1] != '*')
                                {
                                    fantasmacol--;
                                }
                                break;
                            case 6:
                            case 2:
                                if (tb[fantasmalin + 1][fantasmacol] != '*')
                                {
                                    fantasmalin++;
                                }
                                break;
                            default:
                                if (tb[fantasmalin - 1][fantasmacol] != '*')
                                {
                                    fantasmalin--;
                                }
                                break;

                        }
                    //}
                }
                atraso = MAXATRASO;
            }
        }

        public int getFantasmaCol()
        {
            return fantasmacol;
        }

        public int getFantasmaLin()
        {
            return fantasmalin;
        }
    }

}
