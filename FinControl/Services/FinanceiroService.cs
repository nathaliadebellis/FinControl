using FinControl.Models;
using FinControl.Repositories.Interfaces;

namespace FinControl.Services;

public class FinanceiroService
{
    private readonly ITransacaoRepository _repository;

    public FinanceiroService(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public (decimal receitas, decimal despesas, decimal saldo)
    CalcularResumo()
    {
        var transacoes = _repository.Carregar();

        return CalcularResumo(transacoes);
    }

    public (decimal receitas, decimal despesas, decimal saldo)
        CalcularResumo(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        decimal receitas = transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .Sum(t => t.Valor);

        decimal despesas = transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .Sum(t => t.Valor);

        return (receitas, despesas, receitas - despesas);
    }

    public decimal CalcularSaldo()
    {
        return CalcularResumo().saldo;
    }

    public decimal CalcularPercentualEconomia()
    {
        return CalcularPercentualEconomia(_repository.Carregar());
    }

    public decimal CalcularPercentualEconomia(
        List<Transacao> transacoes)
    {
        var (receitas, _, saldo) = CalcularResumo(transacoes);

        return receitas == 0
            ? 0
            : (saldo / receitas) * 100;
    }

    public int CalcularIndiceSaudeFinanceira()
    {
        return CalcularIndiceSaudeFinanceira(_repository.Carregar());
    }

    public int CalcularIndiceSaudeFinanceira(
        List<Transacao> transacoes)
    {
        var (receitas, despesas, saldo) =
            CalcularResumo(transacoes);

        int indice = 100;

        if (saldo < 0)
            indice -= 50;
        else if (receitas > 0 &&
                 (saldo / receitas) < 0.10m)
            indice -= 20;

        if (receitas > 0 &&
            (despesas / receitas) > 0.90m)
            indice -= 15;

        return Math.Clamp(indice, 0, 100);
    }

    public string ObterStatusSaudeFinanceira(
        int indice)
    {
        if (indice >= 90)
            return "Excelente";

        if (indice >= 70)
            return "Boa";

        if (indice >= 50)
            return "Regular";

        return "Crítica";
    }

    public string GerarAnaliseFinanceira()
    {
        return GerarAnaliseFinanceira(_repository.Carregar());
    }

    public string GerarAnaliseFinanceira(
        List<Transacao> transacoes)
    {
        var (receitas, _, saldo) =
            CalcularResumo(transacoes);

        if (receitas == 0)
            return "Nenhuma receita cadastrada.";

        decimal percentual =
            CalcularPercentualEconomia(transacoes);

        if (saldo < 0)
            return "Atenção: suas despesas ultrapassaram as receitas.";

        if (percentual >= 20)
            return "Excelente! Você está economizando uma boa parte da sua renda.";

        if (percentual >= 10)
            return "Bom controle financeiro, mas ainda há espaço para economizar mais.";

        return "Sua margem de economia está baixa. Revise seus gastos.";
    }

    private List<GastoCategoriaResumo>
        CalcularGastosPorCategoria(
            List<Transacao> transacoes,
            decimal totalDespesas)
    {
        return transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
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

    private Transacao? ObterMaiorReceita(
        List<Transacao> transacoes)
    {
        return transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .OrderByDescending(t => t.Valor)
            .FirstOrDefault();
    }

    private Transacao? ObterMaiorDespesa(
        List<Transacao> transacoes)
    {
        return transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .OrderByDescending(t => t.Valor)
            .FirstOrDefault();
    }

    public DashboardResumo GerarDashboardResumo()
    {
        return GerarDashboardResumo(_repository.Carregar());
    }

    public DashboardResumo GerarDashboardResumo(
        List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        var (receitas, despesas, saldo) =
            CalcularResumo(transacoes);

        var gastosPorCategoria =
            CalcularGastosPorCategoria(
                transacoes,
                despesas);

        var maiorCategoria =
            gastosPorCategoria.FirstOrDefault();

        int indice =
            CalcularIndiceSaudeFinanceira(transacoes);

        return new DashboardResumo
        {
            Saldo = saldo,
            TotalReceitas = receitas,
            TotalDespesas = despesas,

            PercentualEconomia =
                CalcularPercentualEconomia(transacoes),

            IndiceSaudeFinanceira = indice,

            StatusSaudeFinanceira =
                ObterStatusSaudeFinanceira(indice),

            AnaliseFinanceira =
                GerarAnaliseFinanceira(transacoes),

            QuantidadeTransacoes =
                transacoes.Count,

            MaiorCategoria =
                maiorCategoria?.Categoria,

            ValorMaiorCategoria =
                maiorCategoria?.Total ?? 0,

            MaiorReceita =
                ObterMaiorReceita(transacoes),

            MaiorDespesa =
                ObterMaiorDespesa(transacoes),

            UltimasTransacoes = transacoes
                .OrderByDescending(t => t.Data)
                .ThenByDescending(t => t.Id)
                .Take(5)
                .ToList(),

            GastosPorCategoria =
                gastosPorCategoria
        };
    }
}