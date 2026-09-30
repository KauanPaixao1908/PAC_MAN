namespace PacmanPalmeiras
{
    // Controla a partida: fases, pontos, vidas e o laço do jogo
    internal class Jogo
    {
        private string[] fases = { "FASE DE GRUPOS", "OITAVAS DE FINAL", "QUARTAS DE FINAL", "SEMIFINAL", "GRANDE FINAL" };

        // de quantos em quantos turnos os rivais andam (quanto menor, mais rápido)
        private int[] velocidadeRivais = { 3, 2, 2, 2, 1 };

        private Mapa mapa = new Mapa();
        private Jogador jogador = new Jogador();
        private List<Rival> rivais = new List<Rival>();

        private int pontos = 0;
        private int vidas = 3;
        private int fase = 0;
        private int tempoAbel = 0; // quando for maior que 0 o Abel está em campo
        private int turno = 0;
        private string mensagem = "";

        public Jogo()
        {
            // os rivais começam dentro da "casinha" no meio do mapa
            rivais.Add(new Rival("Corinthians", 'C', ConsoleColor.White, 12, 9));
            rivais.Add(new Rival("Flamengo", 'F', ConsoleColor.Red, 13, 9));
            rivais.Add(new Rival("São Paulo", 'S', ConsoleColor.Magenta, 14, 9));
        }

        public void Iniciar()
        {
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

            TelaFinal(ganhou);
        }

        private void TelaInicial()
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

        private void TelaDaFase()
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

        private void TelaFinal(bool ganhou)
        {
            Console.Clear();
            Console.WriteLine();
            if (ganhou)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ================================================");
                Console.WriteLine("    PALMEIRAS CAMPEÃO DA LIBERTADORES!!!");
                Console.WriteLine("    O PALMEIRAS É O MAIOR TIME DO UNIVERSO!");
                Console.WriteLine("  ================================================");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
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

        // joga uma fase inteira. Retorna true se pegou todas as taças
        private bool JogarFase()
        {
            mapa.Carregar();
            mapa.Limpar(13, 15); // o Palmeiras começa aqui, então tira a taça desse lugar
            PosicoesIniciais();
            Console.Clear();
            mapa.Desenhar();

            while (true)
            {
                LerTeclado();

                // apaga os personagens da posição antiga
                mapa.DesenharCasa(jogador.X, jogador.Y);
                foreach (Rival rival in rivais)
                {
                    mapa.DesenharCasa(rival.X, rival.Y);
                }

                char item = jogador.Mover(mapa);
                if (item == '.')
                {
                    pontos += 10;
                }
                else if (item == 'A')
                {
                    pontos += 50;
                    tempoAbel = 40; // mais ou menos 5 segundos
                    mensagem = "ABEL FERREIRA ENTROU EM CAMPO!";
                }
                VerificarColisao();

                turno++;
                if (turno % velocidadeRivais[fase] == 0)
                {
                    foreach (Rival rival in rivais)
                    {
                        rival.Mover(mapa, jogador, tempoAbel > 0);
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

                if (mapa.Tacas == 0)
                {
                    mensagem = fases[fase] + " CONCLUÍDA!";
                    DesenharPlacar();
                    Thread.Sleep(2000);
                    return true;
                }

                Thread.Sleep(130);
            }
        }

        private void PosicoesIniciais()
        {
            jogador.VoltarParaInicio();
            foreach (Rival rival in rivais)
            {
                rival.VoltarParaInicio();
            }
            tempoAbel = 0;
            mensagem = fases[fase];
        }

        private void LerTeclado()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey tecla = Console.ReadKey(true).Key;

                if (tecla == ConsoleKey.UpArrow || tecla == ConsoleKey.W) jogador.Direcao = Direcao.Cima;
                if (tecla == ConsoleKey.DownArrow || tecla == ConsoleKey.S) jogador.Direcao = Direcao.Baixo;
                if (tecla == ConsoleKey.LeftArrow || tecla == ConsoleKey.A) jogador.Direcao = Direcao.Esquerda;
                if (tecla == ConsoleKey.RightArrow || tecla == ConsoleKey.D) jogador.Direcao = Direcao.Direita;

                if (tecla == ConsoleKey.Escape)
                {
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                }
            }
        }

        private void VerificarColisao()
        {
            foreach (Rival rival in rivais)
            {
                if (rival.X != jogador.X || rival.Y != jogador.Y)
                {
                    continue;
                }

                if (tempoAbel > 0)
                {
                    // com o Abel o Palmeiras derrota o rival e ele volta pra casinha
                    pontos += 200;
                    mensagem = "GOL DO VERDÃO! " + rival.Nome + " derrotado! +200";
                    rival.VoltarParaInicio();
                }
                else
                {
                    vidas--;
                    if (vidas > 0)
                    {
                        mensagem = "O " + rival.Nome + " te pegou! Restam " + vidas + " vidas";
                        DesenharPersonagens();
                        DesenharPlacar();
                        Thread.Sleep(1500);

                        PosicoesIniciais();
                        mensagem = "Perdeu uma vida! Restam " + vidas;
                        Console.Clear();
                        mapa.Desenhar();
                    }
                    else
                    {
                        mensagem = "O " + rival.Nome + " te pegou! Fim de jogo...";
                    }
                    return;
                }
            }
        }

        private void DesenharPersonagens()
        {
            foreach (Rival rival in rivais)
            {
                rival.Desenhar(tempoAbel > 0);
            }
            jogador.Desenhar();
        }

        private void DesenharPlacar()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, mapa.Altura + 1);
            Console.Write(("Pontos: " + pontos + "   Vidas: " + vidas + "   Taças: " + mapa.Tacas + "   " + fases[fase]).PadRight(70));
            Console.SetCursorPosition(0, mapa.Altura + 2);
            Console.Write(mensagem.PadRight(70));
        }
    }
}
