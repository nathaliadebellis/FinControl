namespace FinControl.Models;

/// <summary>
/// Represents a combination of optional criteria used to filter transactions.
/// </summary>
public class FiltroTransacao
{
    /// <summary>
    /// Partial description to search for (case-insensitive).
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Specific category name to match.
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Transaction type to filter by.
    /// </summary>
    public TipoTransacao? Tipo { get; set; }

    /// <summary>
    /// Start date for the filtering period.
    /// </summary>
    public DateTime? DataInicial { get; set; }

    /// <summary>
    /// End date for the filtering period.
    /// </summary>
    public DateTime? DataFinal { get; set; }

    /// <summary>
    /// Minimum transaction value.
    /// </summary>
    public decimal? ValorMinimo { get; set; }

    /// <summary>
    /// Maximum transaction value.
    /// </summary>
    public decimal? ValorMaximo { get; set; }

    /// <summary>
    /// Returns true when at least one filter criterion has been set.
    /// </summary>
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