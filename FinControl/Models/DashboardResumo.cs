namespace FinControl.Models;

public class DashboardResumo
{
    public decimal Saldo { get; set; }

    public decimal TotalReceitas { get; set; }

    public decimal TotalDespesas { get; set; }

    public decimal PercentualEconomia { get; set; }

    public int IndiceSaudeFinanceira { get; set; }

    public string StatusSaudeFinanceira { get; set; } = string.Empty;

    public string AnaliseFinanceira { get; set; } = string.Empty;

    public int QuantidadeTransacoes { get; set; }

    public string? MaiorCategoria { get; set; }

    public decimal ValorMaiorCategoria { get; set; }

    public Transacao? MaiorReceita { get; set; }

    public Transacao? MaiorDespesa { get; set; }

    public decimal MetaEconomia { get; set; }

    public decimal ProgressoMeta { get; set; }

    public bool MetaAtingida { get; set; }

    public List<Transacao> UltimasTransacoes { get; set; } = [];

    public List<GastoCategoriaResumo> GastosPorCategoria { get; set; } = [];
}