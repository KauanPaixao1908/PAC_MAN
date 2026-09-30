namespace PACMAN001M
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.pdxTabuleiro = new System.Windows.Forms.PictureBox();
            this.lblTemporizador = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblPlacar = new System.Windows.Forms.Label();
            this.lblFase = new System.Windows.Forms.Label();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.lblLegenda = new System.Windows.Forms.Label();
            this.btnCima = new System.Windows.Forms.Button();
            this.btnBaixo = new System.Windows.Forms.Button();
            this.btnEsquerda = new System.Windows.Forms.Button();
            this.btnDireita = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pdxTabuleiro)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(700, 402);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(75, 30);
            this.btnSair.TabIndex = 0;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(600, 402);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 30);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "START";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // pdxTabuleiro
            // 
            this.pdxTabuleiro.BackColor = System.Drawing.Color.Black;
            this.pdxTabuleiro.Location = new System.Drawing.Point(15, 12);
            this.pdxTabuleiro.Name = "pdxTabuleiro";
            this.pdxTabuleiro.Size = new System.Drawing.Size(560, 420);
            this.pdxTabuleiro.TabIndex = 2;
            this.pdxTabuleiro.TabStop = false;
            this.pdxTabuleiro.Paint += new System.Windows.Forms.PaintEventHandler(this.pdxTabuleiro_Paint);
            // 
            // lblTemporizador
            // 
            this.lblTemporizador.AutoSize = true;
            this.lblTemporizador.Location = new System.Drawing.Point(597, 72);
            this.lblTemporizador.Name = "lblTemporizador";
            this.lblTemporizador.Size = new System.Drawing.Size(71, 13);
            this.lblTemporizador.TabIndex = 3;
            this.lblTemporizador.Text = "CONTADOR:";
            // 
            // timer1
            // 
            this.timer1.Interval = 150;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblPlacar
            // 
            this.lblPlacar.AutoSize = true;
            this.lblPlacar.Location = new System.Drawing.Point(597, 16);
            this.lblPlacar.Name = "lblPlacar";
            this.lblPlacar.Size = new System.Drawing.Size(50, 13);
            this.lblPlacar.TabIndex = 4;
            this.lblPlacar.Text = "PONTOS:";
            // 
            // lblFase
            // 
            this.lblFase.AutoSize = true;
            this.lblFase.Location = new System.Drawing.Point(597, 44);
            this.lblFase.Name = "lblFase";
            this.lblFase.Size = new System.Drawing.Size(36, 13);
            this.lblFase.TabIndex = 5;
            this.lblFase.Text = "FASE:";
            // 
            // lblMensagem
            // 
            this.lblMensagem.Location = new System.Drawing.Point(597, 100);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(190, 70);
            this.lblMensagem.TabIndex = 6;
            this.lblMensagem.Text = "";
            // 
            // lblLegenda
            // 
            this.lblLegenda.Location = new System.Drawing.Point(597, 172);
            this.lblLegenda.Name = "lblLegenda";
            this.lblLegenda.Size = new System.Drawing.Size(190, 110);
            this.lblLegenda.TabIndex = 7;
            this.lblLegenda.Text = "Verde = Palmeiras (você)\r\nAmarelo = taça da Libertadores\r\nLaranja = Abel Ferreira\r\nPreto = Corinthians\r\nVermelho = Flamengo\r\nBranco = São Paulo\r\n\r\nMova com as setas: ^  ↓  <  >";
            // 
            // btnCima
            // 
            this.btnCima.Location = new System.Drawing.Point(660, 285);
            this.btnCima.Name = "btnCima";
            this.btnCima.Size = new System.Drawing.Size(60, 30);
            this.btnCima.TabIndex = 8;
            this.btnCima.Text = "^";
            this.btnCima.UseVisualStyleBackColor = true;
            this.btnCima.Click += new System.EventHandler(this.btnCima_Click);
            // 
            // btnBaixo
            // 
            this.btnBaixo.Location = new System.Drawing.Point(660, 351);
            this.btnBaixo.Name = "btnBaixo";
            this.btnBaixo.Size = new System.Drawing.Size(60, 30);
            this.btnBaixo.TabIndex = 9;
            this.btnBaixo.Text = "↓";
            this.btnBaixo.UseVisualStyleBackColor = true;
            this.btnBaixo.Click += new System.EventHandler(this.btnBaixo_Click);
            // 
            // btnEsquerda
            // 
            this.btnEsquerda.Location = new System.Drawing.Point(597, 318);
            this.btnEsquerda.Name = "btnEsquerda";
            this.btnEsquerda.Size = new System.Drawing.Size(60, 30);
            this.btnEsquerda.TabIndex = 10;
            this.btnEsquerda.Text = "<";
            this.btnEsquerda.UseVisualStyleBackColor = true;
            this.btnEsquerda.Click += new System.EventHandler(this.btnEsquerda_Click);
            // 
            // btnDireita
            // 
            this.btnDireita.Location = new System.Drawing.Point(723, 318);
            this.btnDireita.Name = "btnDireita";
            this.btnDireita.Size = new System.Drawing.Size(60, 30);
            this.btnDireita.TabIndex = 11;
            this.btnDireita.Text = ">";
            this.btnDireita.UseVisualStyleBackColor = true;
            this.btnDireita.Click += new System.EventHandler(this.btnDireita_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDireita);
            this.Controls.Add(this.btnEsquerda);
            this.Controls.Add(this.btnBaixo);
            this.Controls.Add(this.btnCima);
            this.Controls.Add(this.lblLegenda);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.lblFase);
            this.Controls.Add(this.lblPlacar);
            this.Controls.Add(this.lblTemporizador);
            this.Controls.Add(this.pdxTabuleiro);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnSair);
            this.Name = "Form1";
            this.Text = "PACMAN001 - PALMEIRAS";
            ((System.ComponentModel.ISupportInitialize)(this.pdxTabuleiro)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.PictureBox pdxTabuleiro;
        private System.Windows.Forms.Label lblTemporizador;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblPlacar;
        private System.Windows.Forms.Label lblFase;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.Label lblLegenda;
        private System.Windows.Forms.Button btnCima;
        private System.Windows.Forms.Button btnBaixo;
        private System.Windows.Forms.Button btnEsquerda;
        private System.Windows.Forms.Button btnDireita;
    }
}
