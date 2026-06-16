using FinControl.Models;

namespace FinControl.Services;

public static class DashboardService
{
    public static void Exibir(List<Transacao> transacoes)
    {
        Console.Clear();

        if (!transacoes.Any())
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║      DASHBOARD FINANCEIRO          ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("Nenhuma transação cadastrada.");
            Console.WriteLine("Cadastre uma receita ou despesa para visualizar o dashboard.");

            Console.WriteLine("\nPressione ENTER para voltar...");
            Console.ReadLine();
            return;
        }

        decimal totalReceitas = transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .Sum(t => t.Valor);

        decimal totalDespesas = transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .Sum(t => t.Valor);

        decimal saldo = totalReceitas - totalDespesas;

        // Agrupa despesas por categoria
        var categorias = transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .GroupBy(t => t.Categoria)
            .Select(g => new
            {
                Categoria = g.Key,
                Total = g.Sum(t => t.Valor)
            })
            .OrderByDescending(g => g.Total)
            .ToList();

        var maiorCategoria = categorias.FirstOrDefault();

        // Maior receita
        var maiorReceita = transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .OrderByDescending(t => t.Valor)
            .FirstOrDefault();

        // Maior despesa
        var maiorDespesa = transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .OrderByDescending(t => t.Valor)
            .FirstOrDefault();

        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║         DASHBOARD FINANCEIRO               ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.WriteLine();

        Console.WriteLine($"Saldo Atual:        R$ {saldo:F2}");
        Console.WriteLine($"Receitas:           R$ {totalReceitas:F2}");
        Console.WriteLine($"Despesas:           R$ {totalDespesas:F2}");
        Console.WriteLine($"Transações:         {transacoes.Count}");

        if (maiorCategoria != null)
            Console.WriteLine($"Maior Categoria:    {maiorCategoria.Categoria} (R$ {maiorCategoria.Total:F2})");

        if (maiorReceita != null)
            Console.WriteLine($"Maior Receita:      {maiorReceita.Descricao} (R$ {maiorReceita.Valor:F2})");

        if (maiorDespesa != null)
            Console.WriteLine($"Maior Despesa:      {maiorDespesa.Descricao} (R$ {maiorDespesa.Valor:F2})");

        Console.WriteLine();
        Console.WriteLine("===== GASTOS POR CATEGORIA =====");

        foreach (var categoria in categorias)
        {
            double percentual = totalDespesas == 0
                ? 0
                : (double)(categoria.Total / totalDespesas);

            int blocos = (int)Math.Round(percentual * 20);

            Console.WriteLine(
                $"{categoria.Categoria,-15} " +
                $"{new string('█', blocos),-20} " +
                $"R$ {categoria.Total,8:F2} ({percentual:P0})");
        }

        Console.WriteLine();
        Console.WriteLine("===== ÚLTIMAS TRANSAÇÕES =====");

        foreach (var transacao in transacoes
                     .OrderByDescending(t => t.Data)
                     .Take(5))
        {
            string sinal = transacao.Tipo == TipoTransacao.Receita ? "+" : "-";

            Console.WriteLine(
                $"{transacao.Data:dd/MM/yyyy} | " +
                $"{transacao.Descricao,-20} | " +
                $"{sinal}R$ {transacao.Valor:F2}");
        }

        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para voltar...");
        Console.ReadLine();
    }
}