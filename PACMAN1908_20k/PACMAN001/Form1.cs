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
        int lintabuleiro = 15;
        int coltabuleiro = 14;
        int largura = 20;
        int altura = 20;
        int cursorcol = 1;
        int cursorlin = 1;
        char tecla = ' ';
        int tempo = 0;
        int pontos = 0;
        int flagimagempacman = 0;

        int tempoinicial = 150;
        int poder = 0;
        int[] fantasmalininicial = new int[4];
        int[] fantasmacolinicial = new int[4];
        Image[] imgfantasmas = new Image[4];
        Image imgpowerup;

        //DEFINIR 20 FANTASMANS
        FANTASMA[] fantasmas=new FANTASMA[20];
            
        public Form1()
        {
            InitializeComponent();
            Form1_Resize(this, EventArgs.Empty);

            pbTabuleiro.BackgroundImage = Image.FromFile("abertura.jpg");
            pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;

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

            fantasmalininicial[0] = 6;
            fantasmacolinicial[0] = 6;
            fantasmalininicial[1] = 6;
            fantasmacolinicial[1] = 7;
            fantasmalininicial[2] = 3;
            fantasmacolinicial[2] = 3;
            fantasmalininicial[3] = 3;
            fantasmacolinicial[3] = 10;

            imgfantasmas[0] = Image.FromFile("Fantasma_Fluminense.png");
            imgfantasmas[1] = Image.FromFile("fantasma_corinthians.png");
            imgfantasmas[2] = Image.FromFile("fantasma_flamengo.png");
            imgfantasmas[3] = Image.FromFile("fantasma_sao_paulo.png");
            imgpowerup = Image.FromFile("power_up_Abel_Ferreira.png");

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempo--;
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
            for (int i = 0; i < 4; i++)
            {
                if ((cursorcol == fantasmas[i].getFantasmaCol()) && (cursorlin == fantasmas[i].getFantasmaLin()))
                {
                    if (poder > 0)
                    {
                        fantasmas[i].setPosicaoInicial(fantasmalininicial[i], fantasmacolinicial[i]);
                        pontos = pontos + 50;
                    }
                    else
                    {
                        pego = true;
                    }
                }
            }
            lblTempo.Text ="TEMPO:  "+tempo.ToString() + "[" + fantasmas[0].getFantasmaCol().ToString()+"  "+ fantasmas[0].getFantasmaLin()+"]";
            lblPontos.Text="PONTOS: "+ pontos.ToString();
            txbTeclado.Focus();
            pbTabuleiro.Invalidate();
            if ( (tempo == 0)|| (pego == true) )
            {
                timer1.Enabled = false;
                lblTempo.Text = "TEMPO:  GAME OVER";
                pbTabuleiro.BackgroundImage = Image.FromFile("gameover.jpg");
                pbTabuleiro.SizeMode = PictureBoxSizeMode.StretchImage;
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
                int esp = largura / 8;
                Pen poderpen = new Pen(Color.Blue, esp);
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
                        if (tabuleiro[lin][col] == 'P')
                        {
                            e.Graphics.DrawImage(imgpowerup, col * altura, lin * largura, altura, largura);
                        }
                        if (tabuleiro[lin][col] == ' ')
                        {
                            e.Graphics.DrawRectangle(brancopen, col * altura, lin * largura, altura, largura);
                            e.Graphics.FillRectangle(brancobs, col * altura, lin * largura, altura, largura);

                        }
                    }
                }

                //DESENHAR PACMAN
                int dir=0;
                switch(tecla)
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

                e.Graphics.DrawImage(Image.FromFile(pacman[dir, flagimagempacman + 1]), cursorcol * largura, cursorlin * altura, largura, altura);
                flagimagempacman = Math.Abs(flagimagempacman - 1);

                //DESENHAR FANTASMA
                for (int i = 0; i < 4; i++)
                {
                    fantasmas[i].setMover(tabuleiro, cursorlin, cursorcol);
                    e.Graphics.DrawImage(imgfantasmas[i], fantasmas[i].getFantasmaCol() * largura, fantasmas[i].getFantasmaLin() * altura, largura, altura);
                    if (poder > 0)
                    {
                        e.Graphics.DrawRectangle(poderpen, fantasmas[i].getFantasmaCol() * largura + esp / 2, fantasmas[i].getFantasmaLin() * altura + esp / 2, largura - esp, altura - esp);
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
            tabuleiro[0] = "**************";
            tabuleiro[1] = "*P..........P*";
            tabuleiro[2] = "*.****..****.*";
            tabuleiro[3] = "*............*";
            tabuleiro[4] = "*.**.*..*.**.*";
            tabuleiro[5] = "*.**.*..*.**.*";
            tabuleiro[6] = "*............*";
            tabuleiro[7] = "*..*.*..*.*..*";
            tabuleiro[8] = "*............*";
            tabuleiro[9] = "*.**.*..*.**.*";
            tabuleiro[10] = "*.**.*..*.**.*";
            tabuleiro[11] = "*............*";
            tabuleiro[12] = "*.****..****.*";
            tabuleiro[13] = "*P..........P*";
            tabuleiro[14] = "**************";
            timer1.Enabled = true;
            timer1.Interval = 200;
            pbTabuleiro.Invalidate();
            tempo = tempoinicial;
            pontos = 0;
            poder = 0;
            cursorcol = 6;
            cursorlin = 13;
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
