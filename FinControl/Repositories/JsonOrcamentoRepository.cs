using FinControl.Interfaces;
using FinControl.Models;
using FinControl.Services;
using System.Text.Json;

namespace FinControl.Repositories;

public class JsonOrcamentoRepository : IOrcamentoRepository
{
    private string Diretorio =>
    Path.GetDirectoryName(_caminhoArquivo)!;

    private readonly string _caminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "orcamentos.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public List<OrcamentoCategoria> Carregar()
    {
        return GerenciadorErros.ExecutarComTratamento(
            () =>
            {
                Directory.CreateDirectory(Diretorio);

                if (!File.Exists(_caminhoArquivo))
                    return [];

                string json = File.ReadAllText(_caminhoArquivo);

                return JsonSerializer.Deserialize<List<OrcamentoCategoria>>(
                    json,
                    _jsonOptions) ?? [];
            },
            "Carregar orçamentos",
            []
        ) ?? [];
    }

    public void Salvar(List<OrcamentoCategoria> orcamentos)
    {
        GerenciadorErros.ExecutarComTratamento(() =>
        {
            Directory.CreateDirectory(Diretorio);

            string json = JsonSerializer.Serialize(
                orcamentos,
                _jsonOptions);

            File.WriteAllText(_caminhoArquivo, json);
        },
        "Salvar orçamentos");
    }
}