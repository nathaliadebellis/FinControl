using System;
using System.Collections.Generic;
using System.Text;

namespace FinControl.Models;

/// <summary>
/// Representa um orçamento mensal definido para uma categoria.
/// </summary>
public class OrcamentoCategoria
{
    /// <summary>
    /// Nome da categoria.
    /// </summary>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Limite máximo permitido para gastos na categoria.
    /// </summary>
    public decimal LimiteMensal { get; set; }
}