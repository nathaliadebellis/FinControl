using FinControl.Models;
using System.Text.Json;

namespace FinControl.Repositories;

public static class TransacaoRepository
{
    private static readonly string CaminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "transacoes.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static List<Transacao> Carregar()
    {
        GarantirDiretorio();

        if (!File.Exists(CaminhoArquivo))
            return [];

        string json = File.ReadAllText(CaminhoArquivo);

        return JsonSerializer.Deserialize<List<Transacao>>(
            json,
            JsonOptions) ?? [];
    }

    public static void Salvar(List<Transacao> transacoes)
    {
        GarantirDiretorio();

        string json = JsonSerializer.Serialize(
            transacoes,
            JsonOptions);

        File.WriteAllText(CaminhoArquivo, json);
    }

    private static void GarantirDiretorio()
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(CaminhoArquivo)!);
    }
}