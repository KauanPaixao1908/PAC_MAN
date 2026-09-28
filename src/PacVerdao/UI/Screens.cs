using PacVerdao.Core;

namespace PacVerdao.UI;

/// <summary>Telas fora da partida: título, abertura de fase, rebaixamento e título da Libertadores.</summary>
public static class Screens
{
    private static readonly string[] Logo =
    {
        @" ____   _    ____   __     _______ ____  ____    _    ___  ",
        @"|  _ \ / \  / ___|  \ \   / / ____|  _ \|  _ \  / \  / _ \ ",
        @"| |_) / _ \| |   ____\ \ / /|  _| | |_) | | | |/ _ \| | | |",
        @"|  __/ ___ \ |__|_____\ V / | |___|  _ <| |_| / ___ \ |_| |",
        @"|_| /_/   \_\____|     \_/  |_____|_| \_\____/_/   \_\___/ ",
    };

    private static readonly string[] SerieB =
    {
        @" ____  _____ ____  ___ _____   ____  ",
        @"/ ___|| ____|  _ \|_ _| ____| | __ ) ",
        @"\___ \|  _| | |_) || ||  _|   |  _ \ ",
        @" ___) | |___|  _ < | || |___  | |_) |",
        @"|____/|_____|_| \_\___|_____| |____/ ",
    };

    private static readonly string[] Trophy =
    {
        @"   ___________   ",
        @"  '._==_==_=_.'  ",
        @"  .-\:      /-.  ",
        @" | (|:.     |) | ",
        @"  '-|:.     |-'  ",
        @"    \::.    /    ",
        @"     '::. .'     ",
        @"       ) (       ",
        @"     _.' '._     ",
        "    `\"\"\"\"\"\"\"`    ",
    };

    public static void Title(Canvas c, long t, int highScore)
    {
        c.Clear();
        ConsoleColor logoColor = t / 15 % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkGreen;
        for (int i = 0; i < Logo.Length; i++) c.PutCentered(1 + i, Logo[i], logoColor);

        c.PutCentered(7, "A CAÇA ÀS TAÇAS DA LIBERTADORES", ConsoleColor.Yellow);

        c.PutCentered(9, "Guie o Palmeiras pelo gramado e colete todas as taças da Libertadores,", ConsoleColor.Gray);
        c.PutCentered(10, "fugindo dos rivais. Pegue o Abel Ferreira para virar o jogo e derrotá-los!", ConsoleColor.Gray);
        c.PutCentered(11, "Da fase de grupos até a grande final. São só 3 vidas... ou é Série B!", ConsoleColor.Gray);

        int x = 22;
        c.Put(x, 13, "PA", ConsoleColor.White, ConsoleColor.DarkGreen);
        c.Put(x + 3, 13, "Palmeiras (você)", ConsoleColor.Gray);
        c.Put(x, 14, "• ", ConsoleColor.Yellow);
        c.Put(x + 3, 14, "Taça da Libertadores ....... 10", ConsoleColor.Gray);
        c.Put(x, 15, "AB", ConsoleColor.Black, ConsoleColor.Yellow);
        c.Put(x + 3, 15, "Abel Ferreira (power-up) ... 50", ConsoleColor.Gray);
        int row = 16;
        foreach (RivalKind kind in new[] { RivalKind.Flamengo, RivalKind.Corinthians, RivalKind.SaoPaulo })
        {
            var rival = new Rival(kind, default, default, 0);
            (ConsoleColor fg, ConsoleColor bg) = GameView.RivalColors(kind);
            c.Put(x, row, rival.Tag, fg, bg);
            c.Put(x + 3, row, $"{rival.Name,-24} 200+", ConsoleColor.Gray);
            row++;
        }

        c.PutCentered(20, $"RECORDE: {highScore:D6}", ConsoleColor.White);
        if (t / 25 % 2 == 0)
            c.PutCentered(22, "Pressione ENTER para entrar em campo  -  ESC para sair", ConsoleColor.Yellow);
    }

    public static void PhaseIntro(Canvas c, Game game, long t)
    {
        c.Clear();
        Phase phase = game.Phase;
        int total = Phases.All.Count;

        c.PutCentered(3, "COPA LIBERTADORES", ConsoleColor.Yellow);
        c.PutCentered(5, $"FASE {game.PhaseIndex + 1} DE {total}", ConsoleColor.Gray);
        c.Box(18, 7, 44, 5, ConsoleColor.DarkGreen);
        c.PutCentered(9, phase.Name, t / 15 % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White);
        c.PutCentered(13, phase.Stadium, ConsoleColor.Gray);
        c.PutCentered(15, phase.Intro, ConsoleColor.White);

        // Chaveamento: onde o Palmeiras está na caminhada até a final.
        var path = new System.Text.StringBuilder();
        for (int i = 0; i < total; i++)
        {
            path.Append(i < game.PhaseIndex ? "(x)" : i == game.PhaseIndex ? "(>)" : "( )");
            if (i < total - 1) path.Append("---");
        }
        c.PutCentered(17, path.ToString(), ConsoleColor.DarkYellow);

        c.PutCentered(19, $"PLACAR {game.Score:D6}     VIDAS {game.Lives}", ConsoleColor.Gray);
        if (t / 25 % 2 == 0)
            c.PutCentered(22, "ENTER para começar", ConsoleColor.Yellow);
    }

    public static void Relegated(Canvas c, Game game, long t, bool newRecord)
    {
        c.Clear();
        for (int i = 0; i < SerieB.Length; i++)
            c.PutCentered(3 + i, SerieB[i], t / 20 % 2 == 0 ? ConsoleColor.Red : ConsoleColor.DarkRed);

        c.PutCentered(10, "ACABARAM AS VIDAS: O PALMEIRAS FOI REBAIXADO PARA A SÉRIE B!", ConsoleColor.White);
        c.PutCentered(12, $"Eliminado na {game.Phase.Name}", ConsoleColor.Gray);
        c.PutCentered(14, $"PLACAR FINAL: {game.Score:D6}", ConsoleColor.Yellow);
        if (newRecord) c.PutCentered(15, "NOVO RECORDE!", ConsoleColor.Green);
        c.PutCentered(18, "Bola pra frente: ano que vem a gente volta!", ConsoleColor.DarkGray);
        c.PutCentered(21, "ENTER para jogar de novo  -  ESC para sair", ConsoleColor.Yellow);
    }

    public static void Champion(Canvas c, Game game, long t, bool newRecord, int seed)
    {
        c.Clear();

        // Fogos de artifício: estrelas que mudam de lugar e cor.
        var rng = new Random(seed + (int)(t / 6));
        ConsoleColor[] colors = { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkGreen };
        for (int i = 0; i < 40; i++)
        {
            int x = rng.Next(c.Width - 1);
            int y = rng.Next(c.Height - 1);
            c.Put(x, y, rng.Next(2) == 0 ? "*" : "+", colors[rng.Next(colors.Length)]);
        }

        for (int i = 0; i < Trophy.Length; i++)
            c.PutCentered(1 + i, Trophy[i], ConsoleColor.Yellow);

        c.PutCentered(12, " PALMEIRAS CAMPEÃO DA LIBERTADORES! ", ConsoleColor.White, ConsoleColor.DarkGreen);
        c.PutCentered(14, " O MAIOR TIME DO UNIVERSO! ", t / 10 % 2 == 0 ? ConsoleColor.Yellow : ConsoleColor.Green, ConsoleColor.Black);
        c.PutCentered(16, "Todas as taças coletadas, todos os rivais superados.", ConsoleColor.Gray);
        c.PutCentered(18, $"PLACAR FINAL: {game.Score:D6}   VIDAS RESTANTES: {game.Lives}", ConsoleColor.Yellow);
        if (newRecord) c.PutCentered(19, "NOVO RECORDE!", ConsoleColor.Green);
        c.PutCentered(21, "ENTER para jogar de novo  -  ESC para sair", ConsoleColor.Yellow);
    }
}
