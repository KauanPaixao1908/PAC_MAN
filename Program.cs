// Jogo estilo Pac-Man só que melhor
// Aluno: Kauan Paixão - RA: 26002387
//
// O Palmeiras (P) tem que pegar todas as taças da Libertadores (.)
// fugindo dos rivais: Corinthians (C), Flamengo (F) e São Paulo (S).
// Quando pega o Abel Ferreira (A) dá pra derrotar os rivais por um tempo.
// São 3 vidas, se acabar o Palmeiras é rebaixado pra Série B.
// Tem 5 fases: da fase de grupos até a grande final.

using System.Text;

class Program
{
    // mapa do jogo: # = parede, . = taça, A = Abel Ferreira
    static string[] mapaOriginal =
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

    static char[,] mapa;
    static int largura = 28;
    static int altura = 21;

    // jogador (Palmeiras)
    static int jogadorX;
    static int jogadorY;
    static int direcao; // 0 = cima, 1 = baixo, 2 = esquerda, 3 = direita

    // rivais
    static string[] rivalNome = { "Corinthians", "Flamengo", "São Paulo" };
    static char[] rivalLetra = { 'C', 'F', 'S' };
    static ConsoleColor[] rivalCor = { ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Magenta };
    static int[] rivalX = new int[3];
    static int[] rivalY = new int[3];
    static int[] rivalDirecao = new int[3];

    // fases da Libertadores
    static string[] fases = { "FASE DE GRUPOS", "OITAVAS DE FINAL", "QUARTAS DE FINAL", "SEMIFINAL", "GRANDE FINAL" };
    // de quantos em quantos turnos o rival anda (quanto menor, mais rápido)
    static int[] velocidadeRivais = { 3, 2, 2, 2, 1 };

    static int pontos = 0;
    static int vidas = 3;
    static int fase = 0;
    static int taças = 0;
    static int tempoAbel = 0; // quando for maior que 0 o Abel está em campo
    static int turno = 0;
    static string mensagem = "";

    static Random sorteio = new Random();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        TelaInicial();

        bool ganhou = false;
        while (vidas > 0)
        {
            TelaDaFase();
            bool passouDeFase = JogarFase();

            if (passouDeFase)
            {
                fase++;
                if (fase == fases.Length)
                {
                    ganhou = true;
                    break;
                }
            }
        }

        Console.Clear();
        if (ganhou)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("  ================================================");
            Console.WriteLine("    PALMEIRAS CAMPEÃO DA LIBERTADORES!!!");
            Console.WriteLine("    O PALMEIRAS É O MAIOR TIME DO UNIVERSO!");
            Console.WriteLine("  ================================================");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("  ================================================");
            Console.WriteLine("    ACABARAM AS VIDAS...");
            Console.WriteLine("    O PALMEIRAS FOI REBAIXADO PARA A SÉRIE B!");
            Console.WriteLine("  ================================================");
        }
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Pontuação final: " + pontos);
        Console.WriteLine();
        Console.WriteLine("  Aperte qualquer tecla para sair...");
        Console.CursorVisible = true;
        Console.ReadKey(true);
    }

    static void TelaInicial()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine("  ======================================");
        Console.WriteLine("       PAC-MAN DO PALMEIRAS");
        Console.WriteLine("  ======================================");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Pegue todas as taças da Libertadores (.)");
        Console.WriteLine("  e fuja dos rivais: C = Corinthians, F = Flamengo, S = São Paulo");
        Console.WriteLine();
        Console.WriteLine("  Pegue o Abel Ferreira (A) para poder derrotar os rivais!");
        Console.WriteLine("  Você tem 3 vidas, senão é Série B.");
        Console.WriteLine();
        Console.WriteLine("  Controles: setas ou W A S D   |   ESC para sair");
        Console.WriteLine();
        Console.WriteLine("  Aperte qualquer tecla para começar...");
        Console.ReadKey(true);
    }

    static void TelaDaFase()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine();
        Console.WriteLine("  COPA LIBERTADORES");
        Console.WriteLine();
        Console.WriteLine("  Fase " + (fase + 1) + " de " + fases.Length + ": " + fases[fase]);
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Vidas: " + vidas + "   Pontos: " + pontos);
        Console.WriteLine();
        Console.WriteLine("  Aperte qualquer tecla para começar...");
        Console.ReadKey(true);
    }

    // joga uma fase inteira. Retorna true se pegou todas as taças
    static bool JogarFase()
    {
        CarregarMapa();
        PosicoesIniciais();
        Console.Clear();
        DesenharMapa();

        while (true)
        {
            LerTeclado();

            // apaga os personagens da posição antiga
            DesenharCasa(jogadorX, jogadorY);
            for (int i = 0; i < 3; i++)
            {
                DesenharCasa(rivalX[i], rivalY[i]);
            }

            MoverJogador();
            VerificarColisao();

            turno++;
            if (turno % velocidadeRivais[fase] == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    MoverRival(i);
                }
                VerificarColisao();
            }

            if (tempoAbel > 0)
            {
                tempoAbel--;
                if (tempoAbel == 0)
                {
                    mensagem = "O Abel saiu de campo!";
                }
            }

            DesenharPersonagens();
            DesenharPlacar();

            if (vidas == 0)
            {
                Thread.Sleep(1500);
                return false;
            }

            if (taças == 0)
            {
                mensagem = fases[fase] + " CONCLUÍDA!";
                DesenharPlacar();
                Thread.Sleep(2000);
                return true;
            }

            Thread.Sleep(130);
        }
    }

    static void CarregarMapa()
    {
        mapa = new char[altura, largura];
        taças = 0;
        for (int y = 0; y < altura; y++)
        {
            for (int x = 0; x < largura; x++)
            {
                mapa[y, x] = mapaOriginal[y][x];
                if (mapa[y, x] == '.' || mapa[y, x] == 'A')
                {
                    taças++;
                }
            }
        }

        // o Palmeiras começa aqui, então tira a taça desse lugar
        mapa[15, 13] = ' ';
        taças--;
    }

    static void PosicoesIniciais()
    {
        jogadorX = 13;
        jogadorY = 15;
        direcao = 2;

        // os rivais começam dentro da "casinha" no meio do mapa
        for (int i = 0; i < 3; i++)
        {
            rivalX[i] = 12 + i;
            rivalY[i] = 9;
            rivalDirecao[i] = 0;
        }

        tempoAbel = 0;
        mensagem = fases[fase];
    }

    static void LerTeclado()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey tecla = Console.ReadKey(true).Key;

            if (tecla == ConsoleKey.UpArrow || tecla == ConsoleKey.W) direcao = 0;
            if (tecla == ConsoleKey.DownArrow || tecla == ConsoleKey.S) direcao = 1;
            if (tecla == ConsoleKey.LeftArrow || tecla == ConsoleKey.A) direcao = 2;
            if (tecla == ConsoleKey.RightArrow || tecla == ConsoleKey.D) direcao = 3;

            if (tecla == ConsoleKey.Escape)
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true;
                Environment.Exit(0);
            }
        }
    }

    static int ProximoX(int x, int dir)
    {
        if (dir == 2) return x - 1;
        if (dir == 3) return x + 1;
        return x;
    }

    static int ProximoY(int y, int dir)
    {
        if (dir == 0) return y - 1;
        if (dir == 1) return y + 1;
        return y;
    }

    static bool EhParede(int x, int y)
    {
        return mapa[y, x] == '#';
    }

    static void MoverJogador()
    {
        int novoX = ProximoX(jogadorX, direcao);
        int novoY = ProximoY(jogadorY, direcao);

        if (EhParede(novoX, novoY))
        {
            return; // bateu na parede, fica parado
        }

        jogadorX = novoX;
        jogadorY = novoY;

        if (mapa[jogadorY, jogadorX] == '.')
        {
            pontos += 10;
            taças--;
            mapa[jogadorY, jogadorX] = ' ';
        }
        else if (mapa[jogadorY, jogadorX] == 'A')
        {
            pontos += 50;
            taças--;
            mapa[jogadorY, jogadorX] = ' ';
            tempoAbel = 40; // mais ou menos 5 segundos
            mensagem = "ABEL FERREIRA ENTROU EM CAMPO!";
        }
    }

    static int DirecaoContraria(int dir)
    {
        if (dir == 0) return 1;
        if (dir == 1) return 0;
        if (dir == 2) return 3;
        return 2;
    }

    static void MoverRival(int i)
    {
        // vê para quais lados o rival pode ir (sem voltar para trás)
        List<int> opcoes = new List<int>();
        for (int dir = 0; dir < 4; dir++)
        {
            int x = ProximoX(rivalX[i], dir);
            int y = ProximoY(rivalY[i], dir);
            if (!EhParede(x, y) && dir != DirecaoContraria(rivalDirecao[i]))
            {
                opcoes.Add(dir);
            }
        }

        // se não tiver saída, volta
        if (opcoes.Count == 0)
        {
            opcoes.Add(DirecaoContraria(rivalDirecao[i]));
        }

        int escolhida = opcoes[0];

        if (sorteio.Next(100) < 25)
        {
            // às vezes o rival anda aleatório pra não ficar impossível
            escolhida = opcoes[sorteio.Next(opcoes.Count)];
        }
        else
        {
            // escolhe o caminho que chega mais perto do Palmeiras
            // (ou mais longe, se o Abel estiver em campo)
            int melhorDistancia = -1;
            foreach (int dir in opcoes)
            {
                int x = ProximoX(rivalX[i], dir);
                int y = ProximoY(rivalY[i], dir);
                int distancia = Math.Abs(x - jogadorX) + Math.Abs(y - jogadorY);

                bool melhor;
                if (tempoAbel > 0)
                    melhor = distancia > melhorDistancia;
                else
                    melhor = melhorDistancia == -1 || distancia < melhorDistancia;

                if (melhor)
                {
                    melhorDistancia = distancia;
                    escolhida = dir;
                }
            }
        }

        rivalDirecao[i] = escolhida;
        rivalX[i] = ProximoX(rivalX[i], escolhida);
        rivalY[i] = ProximoY(rivalY[i], escolhida);
    }

    static void VerificarColisao()
    {
        for (int i = 0; i < 3; i++)
        {
            if (rivalX[i] == jogadorX && rivalY[i] == jogadorY)
            {
                if (tempoAbel > 0)
                {
                    // com o Abel o Palmeiras derrota o rival e ele volta pra casinha
                    pontos += 200;
                    mensagem = "GOL DO VERDÃO! " + rivalNome[i] + " derrotado! +200";
                    rivalX[i] = 13;
                    rivalY[i] = 9;
                }
                else
                {
                    vidas--;
                    if (vidas > 0)
                    {
                        mensagem = "O " + rivalNome[i] + " te pegou! Restam " + vidas + " vidas";
                        DesenharPersonagens();
                        DesenharPlacar();
                        Thread.Sleep(1500);

                        PosicoesIniciais();
                        mensagem = "Perdeu uma vida! Restam " + vidas;
                        Console.Clear();
                        DesenharMapa();
                    }
                    else
                    {
                        mensagem = "O " + rivalNome[i] + " te pegou! Fim de jogo...";
                    }
                    return;
                }
            }
        }
    }

    static void DesenharMapa()
    {
        for (int y = 0; y < altura; y++)
        {
            for (int x = 0; x < largura; x++)
            {
                DesenharCasa(x, y);
            }
        }
    }

    // desenha o que tem no mapa em uma posição
    static void DesenharCasa(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        char c = mapa[y, x];

        if (c == '#')
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        else if (c == 'A')
            Console.ForegroundColor = ConsoleColor.Cyan;
        else
            Console.ForegroundColor = ConsoleColor.Yellow;

        Console.Write(c);
    }

    static void DesenharPersonagens()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition(rivalX[i], rivalY[i]);
            if (tempoAbel > 0)
                Console.ForegroundColor = ConsoleColor.Blue; // rival com medo do Abel
            else
                Console.ForegroundColor = rivalCor[i];
            Console.Write(rivalLetra[i]);
        }

        Console.SetCursorPosition(jogadorX, jogadorY);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write('P');
    }

    static void DesenharPlacar()
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, altura + 1);
        Console.Write(("Pontos: " + pontos + "   Vidas: " + vidas + "   Taças: " + taças + "   " + fases[fase]).PadRight(70));
        Console.SetCursorPosition(0, altura + 2);
        Console.Write(mensagem.PadRight(70));
    }
}
