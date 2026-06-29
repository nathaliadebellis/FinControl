using FinControl.Models;

namespace FinControl.Utils;

/// <summary>
/// Funções auxiliares para manter a formatação e a saída consistentes entre as telas da aplicação.
/// </summary>
public static class Formatting
{
    /// <summary>
    /// Formato de data padrão utilizado em toda a aplicação.
    /// </summary>
    public const string DateFormat = "dd/MM/yyyy";

    /// <summary>
    /// Exibe um cabeçalho padronizado no console.
    /// </summary>
    public static void ExibirCabecalho(string titulo)
    {
        Console.Clear();

        int largura = Math.Max(36, titulo.Length + 10);
        titulo = titulo.ToUpper();

        int espacosEsquerda = (largura - titulo.Length) / 2;
        int espacosDireita = largura - titulo.Length - espacosEsquerda;

        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine($"║{new string(' ', espacosEsquerda)}{titulo}{new string(' ', espacosDireita)}║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.WriteLine();
    }

    /// <summary>
    /// Exibe uma única transação no console usando o layout padrão da aplicação.
    /// </summary>
    public static void PrintTransacao(Transacao t)
    {
        Console.WriteLine($"ID        : {t.Id}");
        Console.WriteLine($"Data      : {t.Data:dd/MM/yyyy}");
        Console.WriteLine($"Descrição : {t.Descricao}");
        Console.WriteLine($"Categoria : {t.Categoria}");
        Console.WriteLine($"Tipo      : {t.Tipo}");
        Console.WriteLine($"Valor     : R$ {t.Valor:F2}");
    }

    public static void AguardarRetorno()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}