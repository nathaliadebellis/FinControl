using System.Text.Json;
using FinControl.Models;
using FinControl.Services;

namespace FinControl.Repositories;

public static class MetaEconomiaRepository
{
    private static string Diretorio =>
    Path.GetDirectoryName(CaminhoArquivo)!;

    private static readonly string CaminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "metaEconomia.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static MetaEconomia Carregar()
    {
        return GerenciadorErros.ExecutarComTratamento(
            () =>
            {
                Directory.CreateDirectory(Diretorio);

                if (!File.Exists(CaminhoArquivo))
                    return new();

                string json = File.ReadAllText(CaminhoArquivo);

                return JsonSerializer.Deserialize<MetaEconomia>(json)
                       ?? new ();
            },
            "Carregar meta de economia",
            new MetaEconomia()
        ) ?? new MetaEconomia();
    }

    public static void Salvar(MetaEconomia meta)
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(CaminhoArquivo)!);

            string json = JsonSerializer.Serialize(meta, JsonOptions);

            File.WriteAllText(CaminhoArquivo, json);
        },
        "Salvar meta de economia");
    }

    public static void Remover()
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(Diretorio);

            if (File.Exists(CaminhoArquivo))
            {
                File.Delete(CaminhoArquivo);
            }
        },
        "Remover meta de economia");
    }
}