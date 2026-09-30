// Jogo estilo Pac-Man só que melhor
// Aluno: Kauan Paixão - RA: 26002387
//
// O Palmeiras (P) tem que pegar todas as taças da Libertadores (.)
// fugindo dos rivais: Corinthians (C), Flamengo (F) e São Paulo (S).
// Quando pega o Abel Ferreira (A) dá pra derrotar os rivais por um tempo.
// São 3 vidas, se acabar o Palmeiras é rebaixado pra Série B.
// Tem 5 fases: da fase de grupos até a grande final.

using System.Text;

namespace PacmanPalmeiras
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Pac-Man do Palmeiras";
            Console.CursorVisible = false;

            Jogo jogo = new Jogo();
            jogo.Iniciar();
        }
    }
}
