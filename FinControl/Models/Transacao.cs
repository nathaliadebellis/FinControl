using System;

namespace FinControl.Models;

/// <summary>
/// Representa um registro de transação financeira.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Descrição ou observação da transação.
    /// </summary>
    public string Descricao { get; set; }

    /// <summary>
    /// Categoria da transação (ex.: Alimentação, Transporte).
    /// </summary>
    public string Categoria { get; set; }

    /// <summary>
    /// Tipo da transação: Receita ou Despesa.
    /// </summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Data e hora em que a transação ocorreu ou foi registrada.
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Valor monetário da transação.
    /// </summary>
    public decimal Valor { get; set; }
}
