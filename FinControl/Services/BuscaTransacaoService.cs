using FinControl.Models;

namespace FinControl.Services;

/// <summary>
/// Serviço responsável pela busca avançada de transações com filtros combinados
/// </summary>
public static class BuscaTransacaoService
{
    /// <summary>
    /// Busca transações aplicando todos os filtros ativos
    /// </summary>
    public static List<Transacao> BuscarComFiltros(
        List<Transacao> transacoes,
        FiltroTransacao filtro)
    {
        if (filtro == null || !filtro.TemFiltroAtivo())
        {
            return transacoes;
        }

        return transacoes
            .Where(t => FiltrarDescricao(t, filtro.Descricao))
            .Where(t => FiltrarCategoria(t, filtro.Categoria))
            .Where(t => FiltrarTipo(t, filtro.Tipo))
            .Where(t => FiltrarPeriodo(t, filtro.DataInicial, filtro.DataFinal))
            .Where(t => FiltrarFaixaValor(t, filtro.ValorMinimo, filtro.ValorMaximo))
            .OrderByDescending(t => t.Data)
            .ToList();
    }

    /// <summary>
    /// Filtra por descrição (parcial, case-insensitive)
    /// </summary>
    private static bool FiltrarDescricao(Transacao transacao, string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return true;

        return transacao.Descricao
            .Contains(descricao, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Filtra por categoria exata
    /// </summary>
    private static bool FiltrarCategoria(Transacao transacao, string? categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            return true;

        return transacao.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Filtra por tipo de transação (Receita/Despesa)
    /// </summary>
    private static bool FiltrarTipo(Transacao transacao, TipoTransacao? tipo)
    {
        if (!tipo.HasValue)
            return true;

        return transacao.Tipo == tipo.Value;
    }

    /// <summary>
    /// Filtra por período (data inicial e final)
    /// </summary>
    private static bool FiltrarPeriodo(Transacao transacao, DateTime? dataInicial, DateTime? dataFinal)
    {
        var data = transacao.Data.Date;

        if (dataInicial.HasValue && data < dataInicial.Value.Date)
            return false;

        if (dataFinal.HasValue && data > dataFinal.Value.Date)
            return false;

        return true;
    }

    /// <summary>
    /// Filtra por faixa de valor (mínimo e máximo)
    /// </summary>
    private static bool FiltrarFaixaValor(
        Transacao transacao,
        decimal? valorMinimo,
        decimal? valorMaximo)
    {
        if (valorMinimo.HasValue && transacao.Valor < valorMinimo.Value)
            return false;

        if (valorMaximo.HasValue && transacao.Valor > valorMaximo.Value)
            return false;

        return true;
    }

    /// <summary>
    /// Cria um filtro interativamente através do console
    /// </summary>
    public static FiltroTransacao CriarFiltroInterativo()
    {
        var filtro = new FiltroTransacao();

        Console.WriteLine("\n=== BUSCA AVANÇADA DE TRANSAÇÕES ===\n"); // interactive filter builder
        Console.WriteLine("(Deixe em branco para não aplicar o filtro)\n");

        // Descrição
        Console.Write("Descrição (parcial): ");
        var descricao = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(descricao))
            filtro.Descricao = descricao;

        // Categoria
        Console.WriteLine("\nCategorias disponíveis:");
        for (int i = 0; i < Categorias.Lista.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {Categorias.Lista[i]}");
        }
        Console.Write("\nCategoria (número ou nome): ");
        var categoriaInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(categoriaInput))
        {
            if (int.TryParse(categoriaInput, out int indexCategoria) &&
                indexCategoria > 0 && indexCategoria <= Categorias.Lista.Count)
            {
                filtro.Categoria = Categorias.Lista[indexCategoria - 1];
            }
            else if (Categorias.Lista.Contains(categoriaInput))
            {
                filtro.Categoria = categoriaInput;
            }
        }

        // Tipo
        Console.Write("\nTipo (1=Receita, 2=Despesa): ");
        if (int.TryParse(Console.ReadLine(), out int tipo) && (tipo == 1 || tipo == 2))
        {
            filtro.Tipo = tipo == 1 ? TipoTransacao.Receita : TipoTransacao.Despesa;
        }

        // Data inicial
        Console.Write("Data inicial (dd/MM/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime dataInicial))
            filtro.DataInicial = dataInicial;

        // Data final
        Console.Write("Data final (dd/MM/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime dataFinal))
            filtro.DataFinal = dataFinal;

        // Valor mínimo
        Console.Write("Valor mínimo (R$): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valorMin))
            filtro.ValorMinimo = valorMin;

        // Valor máximo
        Console.Write("Valor máximo (R$): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valorMax))
            filtro.ValorMaximo = valorMax;

        return filtro;
    }

    /// <summary>
    /// Exibe as transações encontradas com formatação
    /// </summary>
    public static void ExibirTransacoes(List<Transacao> transacoes)
    {
        if (transacoes.Count == 0)
        {
            Console.WriteLine("\n❌ Nenhuma transação encontrada com os filtros aplicados.\n");
            return;
        }

        Console.WriteLine($"\n✓ {transacoes.Count} transação(ções) encontrada(s):\n");
        Console.WriteLine("────────────────────────────────────────────────────");

        decimal totalReceitas = 0;
        decimal totalDespesas = 0;

        foreach (var transacao in transacoes)
        {
            Formatting.PrintTransacao(transacao);

            if (transacao.Tipo == TipoTransacao.Receita)
                totalReceitas += transacao.Valor;
            else
                totalDespesas += transacao.Valor;
        }

        Console.WriteLine("\n=== RESUMO DA BUSCA ===");
        Console.WriteLine($"Total Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo: R$ {(totalReceitas - totalDespesas):F2}\n");
    }
}