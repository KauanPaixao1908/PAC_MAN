// Funções básicas do C#
using System;

// Listas e coleções
using System.Collections.Generic;

// Consultas em listas
using System.Linq;

// Tarefas
using System.Threading.Tasks;

// Janelas, botões e caixas
using System.Windows.Forms;

// Nome do projeto (agrupa as classes do jogo)
namespace PACMAN001
{
    // Classe que inicia o programa
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>

        // Exigido pelo Windows Forms
        [STAThread]

        // Primeiro método a rodar
        static void Main()
        {
            // Usa o visual moderno do Windows
            Application.EnableVisualStyles();

            // Configuração padrão de texto
            Application.SetCompatibleTextRenderingDefault(false);

            // Abre a janela do jogo
            Application.Run(new Form1());
        }
    }
}
