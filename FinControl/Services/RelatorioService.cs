using FinControl.Models;
using FinControl.Repositories.Interfaces;

namespace FinControl.Services;

public class RelatorioService
{
    private readonly ITransacaoRepository _repository;
    private readonly FinanceiroService _financeiroService;

    public RelatorioService(
        ITransacaoRepository repository,
        FinanceiroService financeiroService)
    {
        _repository = repository;
        _financeiroService = financeiroService;
    }

    // <summary>
    // RELATÓRIO GERAL
    // <summary>

    public List<Transacao> ObterRelatorioGeral()
    {
        return ObterRelatorioGeral(_repository.Carregar());
    }

    public List<Transacao> ObterRelatorioGeral(
        List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    // <summary>
    // RELATÓRIO MENSAL
    // <summary>

    public List<Transacao> ObterRelatorioMensal(
        int mes,
        int ano)
    {
        return ObterRelatorioMensal(
            _repository.Carregar(),
            mes,
            ano);
    }

    public List<Transacao> ObterRelatorioMensal(
        List<Transacao> transacoes,
        int mes,
        int ano)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t =>
                t.Data.Month == mes &&
                t.Data.Year == ano)
            .OrderBy(t => t.Data)
            .ThenBy(t => t.Id)
            .ToList();
    }

    // <summary>
    // RELATÓRIO ANUAL
    // <summary>

    public List<Transacao> ObterRelatorioAnual(
        int ano)
    {
        return ObterRelatorioAnual(
            _repository.Carregar(),
            ano);
    }

    public List<Transacao> ObterRelatorioAnual(
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

    // <summary>
    // PERÍODO
    // <summary>

    public List<Transacao> ObterRelatorioPeriodo(
        DateTime dataInicial,
        DateTime dataFinal)
    {
        return ObterRelatorioPeriodo(
            _repository.Carregar(),
            dataInicial,
            dataFinal);
    }

    public List<Transacao> ObterRelatorioPeriodo(
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

    // <summary>
    // GASTOS POR CATEGORIA
    // <summary>

    public List<GastoCategoriaResumo> ObterGastosPorCategoria()
    {
        return ObterGastosPorCategoria(
            _repository.Carregar());
    }

    public List<GastoCategoriaResumo> ObterGastosPorCategoria(
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

    // <summary>
    // RESUMO
    // <summary>

    public (decimal receitas, decimal despesas, decimal saldo)
        ObterResumo(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return _financeiroService.CalcularResumo(transacoes);
    }
}