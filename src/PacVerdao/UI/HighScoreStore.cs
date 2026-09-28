namespace PacVerdao.UI;

/// <summary>Guarda o recorde num arquivo de texto na pasta de dados do usuário.</summary>
public static class HighScoreStore
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PacVerdao",
        "recorde.txt");

    public static int Load()
    {
        try
        {
            return File.Exists(FilePath) && int.TryParse(File.ReadAllText(FilePath).Trim(), out int value) ? value : 0;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    public static void Save(int score)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, score.ToString());
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            // Sem recorde salvo não é motivo para derrubar o jogo.
        }
    }
}
