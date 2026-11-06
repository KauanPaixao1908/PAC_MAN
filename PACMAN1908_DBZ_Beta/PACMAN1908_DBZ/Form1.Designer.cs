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
            this.pbPACMAN = new System.Windows.Forms.PictureBox();
            this.pbFantasma0 = new System.Windows.Forms.PictureBox();
            this.pbFantasma1 = new System.Windows.Forms.PictureBox();
            this.pbFantasma2 = new System.Windows.Forms.PictureBox();
            this.pbFantasma3 = new System.Windows.Forms.PictureBox();
            this.pbPower0 = new System.Windows.Forms.PictureBox();
            this.pbPower1 = new System.Windows.Forms.PictureBox();
            this.pbPower2 = new System.Windows.Forms.PictureBox();
            this.pbPower3 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txbTeclado = new System.Windows.Forms.TextBox();
            this.btnInicializar = new System.Windows.Forms.Button();
            this.lblTempo = new System.Windows.Forms.Label();
            this.lblPontos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPACMAN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower3)).BeginInit();
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
            this.pbTabuleiro.Controls.Add(this.pbPACMAN);
            this.pbTabuleiro.Controls.Add(this.pbFantasma0);
            this.pbTabuleiro.Controls.Add(this.pbFantasma1);
            this.pbTabuleiro.Controls.Add(this.pbFantasma2);
            this.pbTabuleiro.Controls.Add(this.pbFantasma3);
            this.pbTabuleiro.Controls.Add(this.pbPower0);
            this.pbTabuleiro.Controls.Add(this.pbPower1);
            this.pbTabuleiro.Controls.Add(this.pbPower2);
            this.pbTabuleiro.Controls.Add(this.pbPower3);
            this.pbTabuleiro.Location = new System.Drawing.Point(12, 12);
            this.pbTabuleiro.Name = "pbTabuleiro";
            this.pbTabuleiro.Size = new System.Drawing.Size(736, 672);
            this.pbTabuleiro.BackColor = System.Drawing.Color.Black;
            this.pbTabuleiro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbTabuleiro.TabIndex = 1;
            this.pbTabuleiro.TabStop = false;
            this.pbTabuleiro.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTabuleiro_Paint);
            // 
            // pbPACMAN
            // 
            this.pbPACMAN.BackColor = System.Drawing.Color.Transparent;
            this.pbPACMAN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPACMAN.Location = new System.Drawing.Point(0, 0);
            this.pbPACMAN.Name = "pbPACMAN";
            this.pbPACMAN.Size = new System.Drawing.Size(32, 32);
            this.pbPACMAN.TabIndex = 0;
            this.pbPACMAN.TabStop = false;
            this.pbPACMAN.Visible = false;
            // 
            // pbFantasma0
            // 
            this.pbFantasma0.BackColor = System.Drawing.Color.Transparent;
            this.pbFantasma0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFantasma0.Location = new System.Drawing.Point(0, 0);
            this.pbFantasma0.Name = "pbFantasma0";
            this.pbFantasma0.Size = new System.Drawing.Size(32, 32);
            this.pbFantasma0.TabIndex = 1;
            this.pbFantasma0.TabStop = false;
            this.pbFantasma0.Visible = false;
            // 
            // pbFantasma1
            // 
            this.pbFantasma1.BackColor = System.Drawing.Color.Transparent;
            this.pbFantasma1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFantasma1.Location = new System.Drawing.Point(0, 0);
            this.pbFantasma1.Name = "pbFantasma1";
            this.pbFantasma1.Size = new System.Drawing.Size(32, 32);
            this.pbFantasma1.TabIndex = 2;
            this.pbFantasma1.TabStop = false;
            this.pbFantasma1.Visible = false;
            // 
            // pbFantasma2
            // 
            this.pbFantasma2.BackColor = System.Drawing.Color.Transparent;
            this.pbFantasma2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFantasma2.Location = new System.Drawing.Point(0, 0);
            this.pbFantasma2.Name = "pbFantasma2";
            this.pbFantasma2.Size = new System.Drawing.Size(32, 32);
            this.pbFantasma2.TabIndex = 3;
            this.pbFantasma2.TabStop = false;
            this.pbFantasma2.Visible = false;
            // 
            // pbFantasma3
            // 
            this.pbFantasma3.BackColor = System.Drawing.Color.Transparent;
            this.pbFantasma3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFantasma3.Location = new System.Drawing.Point(0, 0);
            this.pbFantasma3.Name = "pbFantasma3";
            this.pbFantasma3.Size = new System.Drawing.Size(32, 32);
            this.pbFantasma3.TabIndex = 4;
            this.pbFantasma3.TabStop = false;
            this.pbFantasma3.Visible = false;
            // 
            // pbPower0
            // 
            this.pbPower0.BackColor = System.Drawing.Color.Transparent;
            this.pbPower0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPower0.Location = new System.Drawing.Point(0, 0);
            this.pbPower0.Name = "pbPower0";
            this.pbPower0.Size = new System.Drawing.Size(32, 32);
            this.pbPower0.TabIndex = 5;
            this.pbPower0.TabStop = false;
            this.pbPower0.Visible = false;
            // 
            // pbPower1
            // 
            this.pbPower1.BackColor = System.Drawing.Color.Transparent;
            this.pbPower1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPower1.Location = new System.Drawing.Point(0, 0);
            this.pbPower1.Name = "pbPower1";
            this.pbPower1.Size = new System.Drawing.Size(32, 32);
            this.pbPower1.TabIndex = 6;
            this.pbPower1.TabStop = false;
            this.pbPower1.Visible = false;
            // 
            // pbPower2
            // 
            this.pbPower2.BackColor = System.Drawing.Color.Transparent;
            this.pbPower2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPower2.Location = new System.Drawing.Point(0, 0);
            this.pbPower2.Name = "pbPower2";
            this.pbPower2.Size = new System.Drawing.Size(32, 32);
            this.pbPower2.TabIndex = 7;
            this.pbPower2.TabStop = false;
            this.pbPower2.Visible = false;
            // 
            // pbPower3
            // 
            this.pbPower3.BackColor = System.Drawing.Color.Transparent;
            this.pbPower3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPower3.Location = new System.Drawing.Point(0, 0);
            this.pbPower3.Name = "pbPower3";
            this.pbPower3.Size = new System.Drawing.Size(32, 32);
            this.pbPower3.TabIndex = 8;
            this.pbPower3.TabStop = false;
            this.pbPower3.Visible = false;
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
            this.ClientSize = new System.Drawing.Size(760, 770);
            this.Controls.Add(this.lblPontos);
            this.Controls.Add(this.lblTempo);
            this.Controls.Add(this.btnInicializar);
            this.Controls.Add(this.pbTabuleiro);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.txbTeclado);
            this.Name = "Form1";
            this.Text = "PAC MAN - KAUAN PAIXAO - RA 26002387";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPACMAN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPower3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.PictureBox pbTabuleiro;
        private System.Windows.Forms.PictureBox pbPACMAN;
        private System.Windows.Forms.PictureBox pbFantasma0;
        private System.Windows.Forms.PictureBox pbFantasma1;
        private System.Windows.Forms.PictureBox pbFantasma2;
        private System.Windows.Forms.PictureBox pbFantasma3;
        private System.Windows.Forms.PictureBox pbPower0;
        private System.Windows.Forms.PictureBox pbPower1;
        private System.Windows.Forms.PictureBox pbPower2;
        private System.Windows.Forms.PictureBox pbPower3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txbTeclado;
        private System.Windows.Forms.Button btnInicializar;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.Label lblPontos;
    }
}

