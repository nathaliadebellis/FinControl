using FinControl.Models;

namespace FinControl.Services;

/// <summary>
/// Helper utilities for consistent formatting and output across reports.
/// </summary>
internal static class Formatting
{
    /// <summary>
    /// Standard date format used across the application.
    /// </summary>
    public const string DateFormat = "dd/MM/yyyy";

    /// <summary>
    /// Prints a single transaction to the Console using the application's standard layout.
    /// </summary>
    public static void PrintTransacao(Transacao t)
    {
        Console.WriteLine($"ID: {t.Id}");
        Console.WriteLine($"Data: {t.Data.ToString(DateFormat)}");
        Console.WriteLine($"Descrição: {t.Descricao}");
        Console.WriteLine($"Categoria: {t.Categoria}");
        Console.WriteLine($"Tipo: {t.Tipo}");
        Console.WriteLine($"Valor: R$ {t.Valor:F2}");
        Console.WriteLine("--------------------");
    }
}
