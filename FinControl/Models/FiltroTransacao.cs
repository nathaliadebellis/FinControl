namespace FinControl.Models;

/// Filtro avançado para buscar transações com múltiplos critérios
public class FiltroTransacao
{
    /// Descrição parcial (case-insensitive)
    public string? Descricao { get; set; }

    /// Categoria específica
    public string? Categoria { get; set; }

    /// Tipo de transação (Receita/Despesa)
    public TipoTransacao? Tipo { get; set; }

    /// Data inicial do período
    public DateTime? DataInicial { get; set; }

    /// Data final do período
    public DateTime? DataFinal { get; set; }

    /// Valor mínimo da transação
    public decimal? ValorMinimo { get; set; }

    /// Valor máximo da transação
    public decimal? ValorMaximo { get; set; }

    /// Verifica se o filtro tem algum critério ativo
    public bool TemFiltroAtivo()
    {
        return !string.IsNullOrWhiteSpace(Descricao) ||
               !string.IsNullOrWhiteSpace(Categoria) ||
               Tipo.HasValue ||
               DataInicial.HasValue ||
               DataFinal.HasValue ||
               ValorMinimo.HasValue ||
               ValorMaximo.HasValue;
    }
}