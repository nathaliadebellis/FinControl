using FinControl.Models;
using FinControl.Repositories;

namespace FinControl.Services;

public static class MetaEconomiaService
{
    public static void DefinirMeta(decimal valorMeta)
    {
        var meta = new MetaEconomia
        {
            ValorMeta = valorMeta
        };

        MetaEconomiaRepository.Salvar(meta);
    }

    public static MetaEconomia ObterMeta()
    {
        return MetaEconomiaRepository.Carregar();
    }

    public static void RemoverMeta()
    {
        MetaEconomiaRepository.Remover();
    }

    public static decimal CalcularProgresso(
        decimal economiaAtual,
        decimal valorMeta)
    {
        if (valorMeta <= 0)
            return 0;

        return (economiaAtual / valorMeta) * 100;
    }

    public static decimal CalcularValorRestante(
    decimal economiaAtual,
    decimal valorMeta)
    {
        return Math.Max(0, valorMeta - economiaAtual);
    }

    public static string ObterStatusMeta(
    decimal economiaAtual,
    decimal valorMeta)
    {
        if (valorMeta <= 0)
            return "Nenhuma meta definida.";

        if (MetaFoiAtingida(economiaAtual, valorMeta))
            return "Meta atingida!";

        return "Meta em andamento.";
    }

    public static bool MetaFoiAtingida(
        decimal economiaAtual,
        decimal valorMeta)
    {
        return economiaAtual >= valorMeta;
    }
}