using FinControl.Interfaces;
using FinControl.Models;
using System.Text.Json;

namespace FinControl.Repositories.Interfaces;
public class JsonTransacaoRepository : ITransacaoRepository
{
    private readonly string _caminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "transacoes.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public List<Transacao> Carregar()
    {
        GarantirDiretorio();

        if (!File.Exists(_caminhoArquivo))
            return [];

        string json = File.ReadAllText(_caminhoArquivo);

        return JsonSerializer.Deserialize<List<Transacao>>(
            json,
            _jsonOptions) ?? [];
    }

    public void Salvar(List<Transacao> transacoes)
    {
        GarantirDiretorio();

        string json = JsonSerializer.Serialize(
            transacoes,
            _jsonOptions);

        File.WriteAllText(_caminhoArquivo, json);
    }

    private void GarantirDiretorio()
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(_caminhoArquivo)!);
    }
}