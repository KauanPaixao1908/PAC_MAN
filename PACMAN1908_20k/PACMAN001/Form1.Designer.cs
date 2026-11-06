namespace PACMAN001
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
            this.pbTabuleiro = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txbTeclado = new System.Windows.Forms.TextBox();
            this.btnInicializar = new System.Windows.Forms.Button();
            this.lblTempo = new System.Windows.Forms.Label();
            this.lblPontos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(451, 624);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(121, 23);
            this.btnSair.TabIndex = 0;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // pbTabuleiro
            // 
            this.pbTabuleiro.Location = new System.Drawing.Point(12, 12);
            this.pbTabuleiro.Name = "pbTabuleiro";
            this.pbTabuleiro.Size = new System.Drawing.Size(560, 600);
            this.pbTabuleiro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbTabuleiro.TabIndex = 1;
            this.pbTabuleiro.TabStop = false;
            this.pbTabuleiro.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTabuleiro_Paint);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txbTeclado
            // 
            this.txbTeclado.Location = new System.Drawing.Point(22, 588);
            this.txbTeclado.Name = "txbTeclado";
            this.txbTeclado.Size = new System.Drawing.Size(100, 20);
            this.txbTeclado.TabIndex = 2;
            this.txbTeclado.TextChanged += new System.EventHandler(this.txbTeclado_TextChanged);
            this.txbTeclado.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbTeclado_KeyPress);
            this.txbTeclado.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbTeclado_KeyDown);
            // 
            // btnInicializar
            // 
            this.btnInicializar.Location = new System.Drawing.Point(12, 624);
            this.btnInicializar.Name = "btnInicializar";
            this.btnInicializar.Size = new System.Drawing.Size(121, 23);
            this.btnInicializar.TabIndex = 3;
            this.btnInicializar.Text = "INICIALIZAR";
            this.btnInicializar.UseVisualStyleBackColor = true;
            this.btnInicializar.Click += new System.EventHandler(this.btnInicializar_Click);
            // 
            // lblTempo
            // 
            this.lblTempo.AutoSize = true;
            this.lblTempo.Location = new System.Drawing.Point(12, 656);
            this.lblTempo.Name = "lblTempo";
            this.lblTempo.Size = new System.Drawing.Size(51, 13);
            this.lblTempo.TabIndex = 4;
            this.lblTempo.Text = "TEMPO: ";
            // 
            // lblPontos
            // 
            this.lblPontos.AutoSize = true;
            this.lblPontos.Location = new System.Drawing.Point(292, 656);
            this.lblPontos.Name = "lblPontos";
            this.lblPontos.Size = new System.Drawing.Size(55, 13);
            this.lblPontos.TabIndex = 5;
            this.lblPontos.Text = "PONTOS:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 690);
            this.Controls.Add(this.lblPontos);
            this.Controls.Add(this.lblTempo);
            this.Controls.Add(this.btnInicializar);
            this.Controls.Add(this.pbTabuleiro);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.txbTeclado);
            this.Name = "Form1";
            this.Text = "PAC MAN 001 [ ANDRE MENDELECK [EXEMPLO 001]";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.PictureBox pbTabuleiro;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txbTeclado;
        private System.Windows.Forms.Button btnInicializar;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.Label lblPontos;
    }
}

