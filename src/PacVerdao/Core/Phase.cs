namespace PacVerdao.Core;

/// <summary>
/// Uma fase da Libertadores. Velocidades são "ticks por casa": quanto menor, mais rápido.
/// </summary>
public sealed record Phase(
    string Name,
    string Stadium,
    string Intro,
    string[] Layout,
    int PlayerStep,
    int RivalStep,
    int FrightenedStep,
    int FrightenedTicks,
    int[] ReleaseTicks);

public static class Phases
{
    private const int S = Game.TicksPerSecond;

    public static readonly IReadOnlyList<Phase> All = new[]
    {
        new Phase("FASE DE GRUPOS", "Allianz Parque, São Paulo",
            "A campanha começa em casa. Pegue todas as taças!",
            Layouts.Classico, PlayerStep: 7, RivalStep: 9, FrightenedStep: 14,
            FrightenedTicks: 8 * S, ReleaseTicks: new[] { 0, 3 * S, 6 * S }),
        new Phase("OITAVAS DE FINAL", "La Bombonera, Buenos Aires",
            "Mata-mata! Os rivais estão mais atentos.",
            Layouts.Colunas, PlayerStep: 7, RivalStep: 8, FrightenedStep: 14,
            FrightenedTicks: 7 * S, ReleaseTicks: new[] { 0, (int)(2.5 * S), 5 * S }),
        new Phase("QUARTAS DE FINAL", "Mineirão, Belo Horizonte",
            "Jogo pegado. O Abel tem menos tempo em campo.",
            Layouts.Arena, PlayerStep: 7, RivalStep: 8, FrightenedStep: 13,
            FrightenedTicks: 6 * S, ReleaseTicks: new[] { 0, 2 * S, 4 * S }),
        new Phase("SEMIFINAL", "Monumental de Núñez, Buenos Aires",
            "A vaga na final está logo ali. Rivais mais rápidos!",
            Layouts.Colunas, PlayerStep: 6, RivalStep: 7, FrightenedStep: 12,
            FrightenedTicks: 5 * S, ReleaseTicks: new[] { 0, (int)(1.5 * S), 3 * S }),
        new Phase("GRANDE FINAL", "Estádio Centenário, Montevidéu",
            "É a final! Vença e o Palmeiras vira o maior time do universo!",
            Layouts.Arena, PlayerStep: 6, RivalStep: 6, FrightenedStep: 12,
            FrightenedTicks: 4 * S, ReleaseTicks: new[] { 0, 1 * S, 2 * S }),
    };
}
