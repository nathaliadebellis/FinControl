using System.Text.Json;
using FinControl.Models;
using FinControl.Services;

namespace FinControl.Repositories;

public static class OrcamentoRepository
{
    private static string Diretorio =>
    Path.GetDirectoryName(CaminhoArquivo)!;

    private static readonly string CaminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "orcamentos.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static List<OrcamentoCategoria> Carregar()
    {
        return GerenciadorErros.ExecutarComTratamento(
            () =>
            {
                Directory.CreateDirectory(Diretorio);

                if (!File.Exists(CaminhoArquivo))
                    return [];

                string json = File.ReadAllText(CaminhoArquivo);

                return JsonSerializer.Deserialize<List<OrcamentoCategoria>>(json)
                       ?? [];
            },
            "Carregar orçamentos",
            []
        ) ?? [];
    }

    public static void Salvar(List<OrcamentoCategoria> orcamentos)
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(Diretorio);

            string json = JsonSerializer.Serialize(
                orcamentos,
                JsonOptions);

            File.WriteAllText(CaminhoArquivo, json);
        },
        "Salvar orçamentos");
    }
}