using FinControl.Models;

namespace FinControl.Services;

public static class RelatorioService
{
    public static List<Transacao> ObterRelatorioGeral(
        List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    public static List<Transacao> ObterRelatorioMensal(
        List<Transacao> transacoes,
        int mes,
        int ano)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Data.Month == mes &&
                        t.Data.Year == ano)
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    public static List<Transacao> ObterRelatorioAnual(
        List<Transacao> transacoes,
        int ano)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Data.Year == ano)
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    public static List<Transacao> ObterRelatorioPeriodo(
        List<Transacao> transacoes,
        DateTime dataInicial,
        DateTime dataFinal)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t =>
                t.Data.Date >= dataInicial.Date &&
                t.Data.Date <= dataFinal.Date)
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    public static List<GastoCategoriaResumo> ObterGastosPorCategoria(
        List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        var despesas = transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .ToList();

        decimal totalDespesas = despesas.Sum(t => t.Valor);

        return despesas
            .GroupBy(t => t.Categoria)
            .Select(g => new GastoCategoriaResumo
            {
                Categoria = g.Key,
                Total = g.Sum(t => t.Valor),
                Percentual = totalDespesas == 0
                    ? 0
                    : g.Sum(t => t.Valor) / totalDespesas
            })
            .OrderByDescending(g => g.Total)
            .ToList();
    }

    public static (decimal receitas, decimal despesas, decimal saldo)
        ObterResumo(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return FinanceiroService.CalcularResumo(transacoes);
    }
}