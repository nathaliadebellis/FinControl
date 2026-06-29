using FinControl.Interfaces;
using FinControl.Models;
using FinControl.Services;
using System.Text.Json;

namespace FinControl.Repositories;

public class JsonMetaEconomiaRepository : IMetaEconomiaRepository
{
    private string Diretorio =>
    Path.GetDirectoryName(_caminhoArquivo)!;

    private readonly string _caminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "metaEconomia.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public MetaEconomia Carregar()
    {
        return GerenciadorErros.ExecutarComTratamento(
            () =>
            {
                Directory.CreateDirectory(Diretorio);

                if (!File.Exists(_caminhoArquivo))
                    return new();

                string json = File.ReadAllText(_caminhoArquivo);

                return JsonSerializer.Deserialize<MetaEconomia>(
    json,
    _jsonOptions)
    ?? new();
            },
            "Carregar meta de economia",
            new()
        ) ?? new();
    }

    public void Salvar(MetaEconomia meta)
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(Diretorio);

            string json = JsonSerializer.Serialize(meta, _jsonOptions);

            File.WriteAllText(_caminhoArquivo, json);
        },
        "Salvar meta de economia");
    }

    public void Remover()
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(Diretorio);

            if (File.Exists(_caminhoArquivo))
            {
                File.Delete(_caminhoArquivo);
            }
        },
        "Remover meta de economia");
    }
}