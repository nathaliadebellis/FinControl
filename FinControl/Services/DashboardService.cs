using System.Text;
using FinControl.Models;

namespace FinControl.Services;

public static class DashboardService
{
    public static string GerarTexto(DashboardResumo resumo)
    {
        var sb = new StringBuilder();

        if (resumo == null || resumo.QuantidadeTransacoes == 0)
        {
            sb.AppendLine("╔════════════════════════════════════╗");
            sb.AppendLine("║      DASHBOARD FINANCEIRO          ║");
            sb.AppendLine("╚════════════════════════════════════╝");
            sb.AppendLine();
            sb.AppendLine("Nenhuma transação cadastrada.");
            sb.AppendLine("Cadastre uma receita ou despesa para visualizar o dashboard.");

            return sb.ToString();
        }

        // =========================
        // HEADER
        // =========================
        sb.AppendLine("╔════════════════════════════════════════════╗");
        sb.AppendLine("║         DASHBOARD FINANCEIRO              ║");
        sb.AppendLine("╚════════════════════════════════════════════╝");
        sb.AppendLine();

        // =========================
        // RESUMO FINANCEIRO
        // =========================
        sb.AppendLine($"Saldo Atual:        R$ {resumo.Saldo:F2}");
        sb.AppendLine($"Receitas:           R$ {resumo.TotalReceitas:F2}");
        sb.AppendLine($"Despesas:           R$ {resumo.TotalDespesas:F2}");
        sb.AppendLine($"Economia:           {resumo.PercentualEconomia:F2}%");
        sb.AppendLine($"Transações:         {resumo.QuantidadeTransacoes}");

        // =========================
        // SAÚDE FINANCEIRA
        // =========================
        sb.AppendLine();
        sb.AppendLine("===== SAÚDE FINANCEIRA =====");
        sb.AppendLine($"Índice:             {resumo.IndiceSaudeFinanceira}/100");
        sb.AppendLine($"Status:             {resumo.StatusSaudeFinanceira}");

        // =========================
        // META
        // =========================
        if (resumo.MetaEconomia > 0)
        {
            sb.AppendLine();
            sb.AppendLine("===== META DE ECONOMIA =====");
            sb.AppendLine($"Meta:               R$ {resumo.MetaEconomia:F2}");
            sb.AppendLine($"Progresso:          {resumo.ProgressoMeta:F2}%");

            if (resumo.MetaAtingida)
            {
                sb.AppendLine("Status:             Meta atingida!");
            }
        }

        // =========================
        // INDICADORES
        // =========================
        sb.AppendLine();
        sb.AppendLine("===== PRINCIPAIS INDICADORES =====");

        if (!string.IsNullOrWhiteSpace(resumo.MaiorCategoria))
        {
            sb.AppendLine(
                $"Maior Categoria:    {resumo.MaiorCategoria} (R$ {resumo.ValorMaiorCategoria:F2})");
        }

        if (resumo.MaiorReceita != null)
        {
            sb.AppendLine(
                $"Maior Receita:      {resumo.MaiorReceita.Descricao} (R$ {resumo.MaiorReceita.Valor:F2})");
        }

        if (resumo.MaiorDespesa != null)
        {
            sb.AppendLine(
                $"Maior Despesa:      {resumo.MaiorDespesa.Descricao} (R$ {resumo.MaiorDespesa.Valor:F2})");
        }

        // =========================
        // GASTOS POR CATEGORIA
        // =========================
        sb.AppendLine();
        sb.AppendLine("===== GASTOS POR CATEGORIA =====");

        foreach (var categoria in resumo.GastosPorCategoria)
        {
            int blocos = (int)Math.Round(categoria.Percentual * 20);

            sb.AppendLine(
                $"{categoria.Categoria,-15} " +
                $"{new string('█', blocos),-20} " +
                $"R$ {categoria.Total,8:F2} ({categoria.Percentual:P0})");
        }

        // =========================
        // ANÁLISE
        // =========================
        sb.AppendLine();
        sb.AppendLine("===== ANÁLISE FINANCEIRA =====");
        sb.AppendLine(resumo.AnaliseFinanceira);

        // =========================
        // ÚLTIMAS TRANSAÇÕES
        // =========================
        sb.AppendLine();
        sb.AppendLine("===== ÚLTIMAS TRANSAÇÕES =====");

        foreach (var transacao in resumo.UltimasTransacoes)
        {
            string sinal = transacao.Tipo == TipoTransacao.Receita
                ? "+"
                : "-";

            sb.AppendLine(
                $"{transacao.Data:dd/MM/yyyy} | " +
                $"{transacao.Descricao,-20} | " +
                $"{sinal}R$ {transacao.Valor:F2}");
        }

        return sb.ToString();
    }
}