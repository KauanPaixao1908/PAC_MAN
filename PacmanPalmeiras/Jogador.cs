namespace PacmanPalmeiras
{
    // O Palmeiras, que é controlado pelo jogador
    internal class Jogador
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direcao Direcao { get; set; }

        public void VoltarParaInicio()
        {
            X = 13;
            Y = 15;
            Direcao = Direcao.Esquerda;
        }

        // anda uma casa e devolve o que pegou no caminho ('.', 'A' ou ' ')
        public char Mover(Mapa mapa)
        {
            int novoX = X;
            int novoY = Y;

            if (Direcao == Direcao.Cima) novoY--;
            if (Direcao == Direcao.Baixo) novoY++;
            if (Direcao == Direcao.Esquerda) novoX--;
            if (Direcao == Direcao.Direita) novoX++;

            if (mapa.EhParede(novoX, novoY))
            {
                return ' '; // bateu na parede, fica parado
            }

            X = novoX;
            Y = novoY;

            char item = mapa.Pegar(X, Y);
            mapa.Limpar(X, Y);
            return item;
        }

        public void Desenhar()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('P');
        }
    }
}
