using FinControl.Models;

namespace FinControl.Services;

public class SistemaService
{
    private readonly string _caminhoArquivo;

    public SistemaService(string? caminhoArquivo = null)
    {
        _caminhoArquivo = caminhoArquivo ??
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "transacoes.json");
    }

    public bool ExisteBackup()
    {
        return ObterBackups().Quantidade > 0;
    }

    public bool CriarBackup(out string caminhoBackup)
    {
        caminhoBackup = string.Empty;

        try
        {
            if (!File.Exists(_caminhoArquivo))
                return false;

            string diretorioBackup = Path.Combine(
                Path.GetDirectoryName(_caminhoArquivo)!,
                "backup");

            Directory.CreateDirectory(diretorioBackup);

            caminhoBackup = Path.Combine(
                diretorioBackup,
                $"{Path.GetFileNameWithoutExtension(_caminhoArquivo)}_" +
                $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json");

            File.Copy(_caminhoArquivo, caminhoBackup, true);

            return true;
        }
        catch
        {
            caminhoBackup = string.Empty;
            return false;
        }
    }

    public BackupInfo ObterBackups()
    {
        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(_caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
            return new BackupInfo();

        return new BackupInfo
        {
            Backups = Directory
                .GetFiles(diretorioBackup, "*.json")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList()
        };
    }

    public bool RestaurarBackup(string caminhoBackup)
    {
        try
        {
            if (File.Exists(_caminhoArquivo))
            {
                CriarBackup(out _);
            }

            if (!File.Exists(caminhoBackup))
                return false;

            File.Copy(caminhoBackup, _caminhoArquivo, true);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void LimparBackupsAntigos(int diasRetencao = 30)
    {
        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(_caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
            return;

        DateTime dataLimite = DateTime.Now.AddDays(-diasRetencao);

        var backups = ObterBackups();

        foreach (var backup in backups.Backups)
        {
            if (backup.LastWriteTime < dataLimite)
            {
                backup.Delete();
            }
        }
    }

    public bool ExcluirTodosBackups()
    {
        var backups = ObterBackups();

        if (!backups.Backups.Any())
            return false;

        foreach (var backup in backups.Backups)
        {
            File.Delete(backup.FullName);
        }

        return true;
    }

    public void ExibirRelatorioErros()
    {
        GerenciadorErros.ExibirRelatorioDErros();
    }
}