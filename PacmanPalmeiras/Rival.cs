namespace PacmanPalmeiras
{
    // Um time rival (é o "fantasma" do Pac-Man)
    internal class Rival
    {
        private static Random sorteio = new Random();

        public string Nome { get; set; }
        public char Letra { get; set; }
        public ConsoleColor Cor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direcao Direcao { get; set; }

        private int inicioX;
        private int inicioY;

        public Rival(string nome, char letra, ConsoleColor cor, int x, int y)
        {
            Nome = nome;
            Letra = letra;
            Cor = cor;
            inicioX = x;
            inicioY = y;
            VoltarParaInicio();
        }

        public void VoltarParaInicio()
        {
            X = inicioX;
            Y = inicioY;
            Direcao = Direcao.Cima;
        }

        public void Mover(Mapa mapa, Jogador jogador, bool abelEmCampo)
        {
            // vê para quais lados o rival pode ir (sem voltar para trás)
            List<Direcao> opcoes = new List<Direcao>();
            foreach (Direcao d in Enum.GetValues<Direcao>())
            {
                if (!mapa.EhParede(ProximoX(d), ProximoY(d)) && d != Contraria(Direcao))
                {
                    opcoes.Add(d);
                }
            }

            // se não tiver saída, volta
            if (opcoes.Count == 0)
            {
                opcoes.Add(Contraria(Direcao));
            }

            Direcao escolhida = opcoes[0];

            if (sorteio.Next(100) < 25)
            {
                // às vezes o rival anda aleatório pra não ficar impossível
                escolhida = opcoes[sorteio.Next(opcoes.Count)];
            }
            else
            {
                // vai para o lado que chega mais perto do Palmeiras
                // (ou mais longe, se o Abel estiver em campo)
                int melhorDistancia = -1;
                foreach (Direcao d in opcoes)
                {
                    int distancia = Math.Abs(ProximoX(d) - jogador.X) + Math.Abs(ProximoY(d) - jogador.Y);

                    bool melhor;
                    if (abelEmCampo)
                        melhor = distancia > melhorDistancia;
                    else
                        melhor = melhorDistancia == -1 || distancia < melhorDistancia;

                    if (melhor)
                    {
                        melhorDistancia = distancia;
                        escolhida = d;
                    }
                }
            }

            Direcao = escolhida;
            X = ProximoX(escolhida);
            Y = ProximoY(escolhida);
        }

        private int ProximoX(Direcao d)
        {
            if (d == Direcao.Esquerda) return X - 1;
            if (d == Direcao.Direita) return X + 1;
            return X;
        }

        private int ProximoY(Direcao d)
        {
            if (d == Direcao.Cima) return Y - 1;
            if (d == Direcao.Baixo) return Y + 1;
            return Y;
        }

        private static Direcao Contraria(Direcao d)
        {
            if (d == Direcao.Cima) return Direcao.Baixo;
            if (d == Direcao.Baixo) return Direcao.Cima;
            if (d == Direcao.Esquerda) return Direcao.Direita;
            return Direcao.Esquerda;
        }

        public void Desenhar(bool abelEmCampo)
        {
            Console.SetCursorPosition(X, Y);
            if (abelEmCampo)
                Console.ForegroundColor = ConsoleColor.Blue; // com medo do Abel
            else
                Console.ForegroundColor = Cor;
            Console.Write(Letra);
        }
    }
}
