using FinControl.Interfaces;
using FinControl.Models;
using FinControl.Repositories;

namespace FinControl.Services;

public class MetaEconomiaService
{
    private readonly IMetaEconomiaRepository _repository;

    public MetaEconomiaService(IMetaEconomiaRepository repository)
    {
        _repository = repository;
    }

    public void DefinirMeta(decimal valorMeta)
    {
        if (valorMeta <= 0)
            throw new ArgumentException(
                "A meta deve ser maior que zero.");

        var meta = new MetaEconomia
        {
            ValorMeta = valorMeta
        };

        _repository.Salvar(meta);
    }

    public MetaEconomia ObterMeta()
    {
        return _repository.Carregar();
    }

    public void RemoverMeta()
    {
        _repository.Remover();
    }

    public decimal CalcularProgresso(
        decimal economiaAtual,
        decimal valorMeta)
    {
        if (valorMeta <= 0)
            return 0;

        return (economiaAtual / valorMeta) * 100;
    }

    public decimal CalcularValorRestante(
        decimal economiaAtual,
        decimal valorMeta)
    {
        return Math.Max(0, valorMeta - economiaAtual);
    }

    public string ObterStatusMeta(
        decimal economiaAtual,
        decimal valorMeta)
    {
        if (valorMeta <= 0)
            return "Nenhuma meta definida.";

        if (MetaFoiAtingida(economiaAtual, valorMeta))
            return "Meta atingida!";

        return "Meta em andamento.";
    }

    public bool MetaFoiAtingida(
        decimal economiaAtual,
        decimal valorMeta)
    {
        return economiaAtual >= valorMeta;
    }
}