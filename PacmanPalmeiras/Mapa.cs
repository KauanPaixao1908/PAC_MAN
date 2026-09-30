namespace PacmanPalmeiras
{
    // Guarda o labirinto e desenha ele na tela
    internal class Mapa
    {
        // # = parede, . = taça da Libertadores, A = Abel Ferreira
        private readonly string[] mapaOriginal =
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

        private char[,] casas = new char[21, 28];

        public int Largura { get; } = 28;
        public int Altura { get; } = 21;
        public int Tacas { get; set; }

        public void Carregar()
        {
            Tacas = 0;
            for (int y = 0; y < Altura; y++)
            {
                for (int x = 0; x < Largura; x++)
                {
                    casas[y, x] = mapaOriginal[y][x];
                    if (casas[y, x] == '.' || casas[y, x] == 'A')
                    {
                        Tacas++;
                    }
                }
            }
        }

        public char Pegar(int x, int y)
        {
            return casas[y, x];
        }

        public void Limpar(int x, int y)
        {
            if (casas[y, x] == '.' || casas[y, x] == 'A')
            {
                Tacas--;
            }
            casas[y, x] = ' ';
        }

        public bool EhParede(int x, int y)
        {
            return casas[y, x] == '#';
        }

        public void Desenhar()
        {
            for (int y = 0; y < Altura; y++)
            {
                for (int x = 0; x < Largura; x++)
                {
                    DesenharCasa(x, y);
                }
            }
        }

        // desenha o que tem no mapa em uma posição (usado também para "apagar" os personagens)
        public void DesenharCasa(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            char c = casas[y, x];

            if (c == '#')
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (c == 'A')
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(c);
        }
    }
}
