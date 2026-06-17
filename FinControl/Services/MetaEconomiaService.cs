using System.Text.Json;
using FinControl.Models;

namespace FinControl.Services;

public static class MetaEconomiaService
{
    private static readonly string CaminhoArquivo =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Data",
            "metaEconomia.json");

    /// <summary>
    /// Carrega a meta de economia do arquivo.
    /// </summary>
    public static MetaEconomia Carregar()
    {
        return GerenciadorErros.TratarCarregamentoDados(() =>
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(CaminhoArquivo)!);

            if (!File.Exists(CaminhoArquivo))
            {
                return new MetaEconomia();
            }

            string json = File.ReadAllText(CaminhoArquivo);

            return JsonSerializer.Deserialize<MetaEconomia>(json)
                   ?? new MetaEconomia();

        },
        "Meta de Economia",
        new MetaEconomia()) ?? new MetaEconomia();
    }

    /// <summary>
    /// Salva a meta de economia.
    /// </summary>
    public static void Salvar(MetaEconomia meta)
    {
        GerenciadorErros.TratarSalvamentoDados(() =>
        {
            // Cria backup antes de salvar
            GerenciadorErros.CriarBackup(CaminhoArquivo);

            Directory.CreateDirectory(
                Path.GetDirectoryName(CaminhoArquivo)!);

            string json = JsonSerializer.Serialize(
                meta,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(CaminhoArquivo, json);

        }, "Meta de Economia");
    }
}