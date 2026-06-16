using FinControl.Models;
using System.Linq;


namespace FinControl.Services;

/// <summary>
/// Provides financial reports and related helpers for console UI.
/// </summary>
public static class RelatorioService
{
    public static void RelatorioFinanceiro(List<Transacao> transacoes)
    {
        bool executandoRelatorio = true;

        while (executandoRelatorio)
        {
            Console.WriteLine("=== RELATÓRIO FINANCEIRO ===");
            Console.WriteLine("1 - Relatório Geral");
            Console.WriteLine("2 - Relatório Mensal");
            Console.WriteLine("3 - Relatório Anual");
            Console.WriteLine("4 - Relatório Personalizado");
            Console.WriteLine("5 - Busca Avançada");
            Console.WriteLine("6 - Voltar");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    RelatorioGeral(transacoes);
                    break;

                case "2":
                    RelatorioMensal(transacoes);
                    break;

                case "3":
                    RelatorioAnual(transacoes);
                    break;

                case "4":
                    RelatorioPersonalizado(transacoes);
                    break;

                case "5":
                    BuscaAvancada(transacoes);
                    break;

                case "6":
                    executandoRelatorio = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    public static void RelatorioMensal(List<Transacao> transacoes)
    {
        int mes = 0;
        bool mesValido = false;

        while (!mesValido)
        {
            Console.WriteLine("Digite o mês (1 a 12):");

            mesValido = int.TryParse(
                Console.ReadLine(),
                out mes
            );

            if (!mesValido || mes < 1 || mes > 12)
            {
                Console.WriteLine("Mês inválido.");
                mesValido = false;
            }
        }

        int ano = 0;
        bool anoValido = false;

        while (!anoValido)
        {
            Console.WriteLine("Digite o ano:");

            anoValido = int.TryParse(
                Console.ReadLine(),
                out ano
            );

            if (!anoValido || ano < 2000)
            {
                Console.WriteLine("Ano inválido.");
                anoValido = false;
            }
        }

        decimal totalReceitas = 0;
        decimal totalDespesas = 0;

        string[] meses =
            {
                "Janeiro", "Fevereiro", "Março", "Abril",
                "Maio", "Junho", "Julho", "Agosto",
                "Setembro", "Outubro", "Novembro", "Dezembro"
            };

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($"=== RELATÓRIO DE {meses[mes - 1].ToUpper()} DE {ano} ===");
        Console.WriteLine("====================================");
        Console.WriteLine();

        List<Transacao> transacoesDoMes = new List<Transacao>();

        foreach (var item in transacoes)
        {
            if (item.Data.Month == mes &&
                item.Data.Year == ano)
            {
                transacoesDoMes.Add(item);

                if (item.Tipo == TipoTransacao.Receita)
                {
                    totalReceitas += item.Valor;
                }
                else if (item.Tipo == TipoTransacao.Despesa)
                {
                    totalDespesas += item.Valor;
                }
            }
        }

        if (transacoesDoMes.Count == 0)
        {
            Console.WriteLine("Nenhuma transação encontrada para este período.");
            return;
        }
        foreach (var item in transacoesDoMes.OrderBy(t => t.Data))
        {
            Formatting.PrintTransacao(item);
        }

        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO MÊS ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");

        MostrarGastosPorCategoria(transacoesDoMes);

    }

    public static void RelatorioAnual(List<Transacao> transacoes)
    {
        int ano = 0;
        bool anoValido = false;

        while (!anoValido)
        {
            Console.WriteLine("Digite o ano:");

            anoValido = int.TryParse(
                Console.ReadLine(),
                out ano
            );

            if (!anoValido || ano < 2000)
            {
                Console.WriteLine("Ano inválido.");
                anoValido = false;
            }
        }

        decimal totalReceitas = 0;
        decimal totalDespesas = 0;

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($"RELATÓRIO ANUAL DE {ano}");
        Console.WriteLine("====================================");
        Console.WriteLine();

        List<Transacao> transacoesDoAno = new List<Transacao>();

        foreach (var item in transacoes)
        {
            if (item.Data.Year == ano)
            {
                transacoesDoAno.Add(item);

                if (item.Tipo == TipoTransacao.Receita)
                {
                    totalReceitas += item.Valor;
                }
                else if (item.Tipo == TipoTransacao.Despesa)
                {
                    totalDespesas += item.Valor;
                }
            }
        }

        if (transacoesDoAno.Count == 0)
        {
            Console.WriteLine("Nenhuma transação encontrada para este ano.");
            return;
        }

        foreach (var item in transacoesDoAno.OrderBy(t => t.Data))
        {
            Formatting.PrintTransacao(item);
        }


        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO ANO ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");

        MostrarGastosPorCategoria(transacoesDoAno);

    }

    /// <summary>
    /// Generates a custom report for a specified period. Prompts the user for start and end dates,
    /// validates them and then builds the report showing each transaction and a summary.
    /// </summary>
    /// <param name="transacoes">All available transactions to search within.</param>
    public static void RelatorioPersonalizado(List<Transacao> transacoes)
    {
        int diaInicial = 0;
        int mesInicial = 0;
        int anoInicial = 0;

        Console.WriteLine("=== DATA INICIAL ===");

        while (!int.TryParse(Console.ReadLine(), out diaInicial) ||
               diaInicial < 1 || diaInicial > 31)
        {
            Console.WriteLine("Digite um dia válido (1 a 31):");
        }

        Console.WriteLine("Digite o mês:");

        while (!int.TryParse(Console.ReadLine(), out mesInicial) ||
               mesInicial < 1 || mesInicial > 12)
        {
            Console.WriteLine("Digite um mês válido (1 a 12):");
        }

        Console.WriteLine("Digite o ano:");

        while (!int.TryParse(Console.ReadLine(), out anoInicial) ||
               anoInicial < 2000)
        {
            Console.WriteLine("Digite um ano válido:");
        }

        DateTime dataInicial = new DateTime(
            anoInicial,
            mesInicial,
            diaInicial
        );

        int diaFinal = 0;
        int mesFinal = 0;
        int anoFinal = 0;

        Console.WriteLine();
        Console.WriteLine("=== DATA FINAL ===");

        Console.WriteLine("Digite o dia:");

        while (!int.TryParse(Console.ReadLine(), out diaFinal) ||
               diaFinal < 1 || diaFinal > 31)
        {
            Console.WriteLine("Digite um dia válido (1 a 31):");
        }

        Console.WriteLine("Digite o mês:");

        while (!int.TryParse(Console.ReadLine(), out mesFinal) ||
               mesFinal < 1 || mesFinal > 12)
        {
            Console.WriteLine("Digite um mês válido (1 a 12):");
        }

        Console.WriteLine("Digite o ano:");

        while (!int.TryParse(Console.ReadLine(), out anoFinal) ||
               anoFinal < 2000)
        {
            Console.WriteLine("Digite um ano válido:");
        }

        DateTime dataFinal = new DateTime(
            anoFinal,
            mesFinal,
            diaFinal
        );

        if (dataFinal < dataInicial)
        {
            Console.WriteLine("A data final não pode ser menor que a data inicial.");
            return;
        }

        decimal totalReceitas = 0;
        decimal totalDespesas = 0;

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($"RELATÓRIO DE {dataInicial.ToString(Formatting.DateFormat)} ATÉ {dataFinal.ToString(Formatting.DateFormat)}");
        Console.WriteLine("====================================");
        Console.WriteLine();

        List<Transacao> transacoesDoPeriodo = new List<Transacao>();

        foreach (var item in transacoes)
        {
            if (item.Data.Date >= dataInicial.Date &&
                item.Data.Date <= dataFinal.Date)
            {
                transacoesDoPeriodo.Add(item);

                if (item.Tipo == TipoTransacao.Receita)
                {
                    totalReceitas += item.Valor;
                }
                else if (item.Tipo == TipoTransacao.Despesa)
                {
                    totalDespesas += item.Valor;
                }
            }
        }

        if (transacoesDoPeriodo.Count == 0)
        {
            Console.WriteLine("Nenhuma transação encontrada para este período.");
            return;
        }

        foreach (var item in transacoesDoPeriodo.OrderBy(t => t.Data))
        {
            Formatting.PrintTransacao(item);
        }


        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO PERÍODO ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");

        MostrarGastosPorCategoria(transacoesDoPeriodo);
    }

    public static void RelatorioGeral(List<Transacao> transacoes)
    {
        decimal totalReceitas = 0;
        decimal totalDespesas = 0;

        if (transacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma transação cadastrada.");
            return;
        }

        foreach (var item in transacoes)
        {
            if (item.Tipo == TipoTransacao.Receita)
                            {
                                totalReceitas += item.Valor;
                            }
                            else if (item.Tipo == TipoTransacao.Despesa)
                            {
                                totalDespesas += item.Valor;
                            }
        }

        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("RELATÓRIO GERAL");
        Console.WriteLine("====================================");
        Console.WriteLine();

        Console.WriteLine($"Quantidade de Transações: {transacoes.Count}");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");

        MostrarGastosPorCategoria(transacoes);
    }

    public static void MostrarGastosPorCategoria(List<Transacao> transacoesFiltradas)
    {
        Console.WriteLine();
        Console.WriteLine("=== GASTOS POR CATEGORIA ===");
        Console.WriteLine();

        foreach (var categoria in Categorias.Lista)
        {
            decimal totalCategoria = 0;

            foreach (var item in transacoesFiltradas)
            {
                if (item.Categoria == categoria &&
                    item.Tipo == TipoTransacao.Despesa)
                {
                    totalCategoria += item.Valor;
                }
            }

            if (totalCategoria > 0)
            {
                Console.WriteLine($"{categoria}: R$ {totalCategoria:F2}");
            }
        }
    }

    public static void BuscaAvancada(List<Transacao> transacoes)
    {
        var filtro = BuscaTransacaoService.CriarFiltroInterativo();
        var resultados = BuscaTransacaoService.BuscarComFiltros(transacoes, filtro);
        BuscaTransacaoService.ExibirTransacoes(resultados);
    }
}