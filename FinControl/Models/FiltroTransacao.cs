namespace FinControl.Models;

/// <summary>
/// Representa uma combinação de critérios opcionais usados para filtrar transações.
/// </summary>
public class FiltroTransacao
{
    /// <summary>
    /// Descrição parcial a ser pesquisada (não diferencia maiúsculas de minúsculas).
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Nome da categoria a ser correspondido exatamente.
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Tipo de transação usado para filtragem.
    /// </summary>
    public TipoTransacao? Tipo { get; set; }

    /// <summary>
    /// Data de início do período de filtragem.
    /// </summary>
    public DateTime? DataInicial { get; set; }

    /// <summary>
    /// Data de fim do período de filtragem.
    /// </summary>
    public DateTime? DataFinal { get; set; }

    /// <summary>
    /// Valor mínimo da transação.
    /// </summary>
    public decimal? ValorMinimo { get; set; }

    /// <summary>
    /// Valor máximo da transação.
    /// </summary>
    public decimal? ValorMaximo { get; set; }

    /// <summary>
    /// Retorna verdadeiro quando pelo menos um critério de filtragem foi definido.
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