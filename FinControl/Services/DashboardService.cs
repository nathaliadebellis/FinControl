using FinControl.Models;
using System.Net.NetworkInformation;

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

        decimal percentualEconomia = totalReceitas == 0
            ? 0
            : (saldo / totalReceitas) * 100;


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

        int indiceSaudeFinanceira = 100;

        // Gastou mais do que recebeu
        if (saldo < 0)
        {
            indiceSaudeFinanceira -= 50;
        }

        // Economia inferior a 10%
        else if (totalReceitas > 0 &&
                 (saldo / totalReceitas) < 0.10m)
        {
            indiceSaudeFinanceira -= 20;
        }

        // Mais de 70% das despesas concentradas em uma categoria
        if (maiorCategoria != null &&
            totalDespesas > 0 &&
            (maiorCategoria.Total / totalDespesas) > 0.70m)
        {
            indiceSaudeFinanceira -= 15;
        }

        // Muitas despesas em relação às receitas
        if (totalReceitas > 0 &&
            totalDespesas / totalReceitas > 0.90m)
        {
            indiceSaudeFinanceira -= 15;
        }

        // Garante que o índice fique entre 0 e 100
        indiceSaudeFinanceira = Math.Clamp(indiceSaudeFinanceira, 0, 100);

        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║         DASHBOARD FINANCEIRO               ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.WriteLine();

        Console.WriteLine($"Saldo Atual:        R$ {saldo:F2}");
        Console.WriteLine($"Receitas:           R$ {totalReceitas:F2}");
        Console.WriteLine($"Despesas:           R$ {totalDespesas:F2}");
        Console.WriteLine($"Economia:              {percentualEconomia:F2}%");
        Console.WriteLine($"Transações:            {transacoes.Count}");

        string status;

        if (indiceSaudeFinanceira >= 90)
        {
            status = "Excelente";
        }
        else if (indiceSaudeFinanceira >= 70)
        {
            status = "Boa";
        }
        else if (indiceSaudeFinanceira >= 50)
        {
            status = "Regular";
        }
        else
        {
            status = "Crítica";
        }

        Console.WriteLine();
        Console.WriteLine("===== SAÚDE FINANCEIRA =====");
        Console.WriteLine($"Índice:             {indiceSaudeFinanceira}/100");
        Console.WriteLine($"Status:             {status}");

        Console.WriteLine();
        Console.WriteLine("===== PRINCIPAIS INDICADORES =====");

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
        Console.WriteLine("===== ANÁLISE FINANCEIRA =====");

        if (saldo < 0)
        {
            Console.WriteLine("Atenção: suas despesas ultrapassaram as receitas.");
        }
        else if (percentualEconomia >= 20)
        {
            Console.WriteLine("Excelente! Você está economizando uma boa parte da sua renda.");
        }
        else if (percentualEconomia >= 10)
        {
            Console.WriteLine("Bom controle financeiro, mas ainda há espaço para economizar mais.");
        }
        else
        {
            Console.WriteLine("Sua margem de economia está baixa. Revise seus gastos.");
        }

        if (maiorCategoria != null && totalDespesas > 0)
        {
            decimal percentualCategoria =
                (maiorCategoria.Total / totalDespesas) * 100;

            Console.WriteLine(
                $"{maiorCategoria.Categoria} representa {percentualCategoria:F0}% das despesas.");
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