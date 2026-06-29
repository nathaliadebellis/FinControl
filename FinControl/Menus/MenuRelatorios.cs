using FinControl.Models;
using FinControl.Services;
using FinControl.Utils;

namespace FinControl.Menus;

public class MenuRelatorios
{
    private readonly RelatorioService _service;

    public MenuRelatorios(RelatorioService service)
    {
        _service = service;
    }
    public void Exibir()
    {

        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("RELATÓRIOS");

            Console.WriteLine("1 - Relatório Geral");
            Console.WriteLine("2 - Relatório Mensal");
            Console.WriteLine("3 - Relatório Anual");
            Console.WriteLine("4 - Gastos por Categoria");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "4", "0" });

            Console.Clear();

            switch (opcao)
            {
                case "1":
                    ExibirRelatorio(
                        _service.ObterRelatorioGeral());
                    break;

                case "2":
                    {
                        int mes = ValidadorEntrada.LerInteiro(
                            "Informe o mês:",
                            1,
                            12);

                        int ano = ValidadorEntrada.LerInteiro(
                            "Informe o ano:",
                            1900,
                            DateTime.Now.Year);

                        ExibirRelatorio(
                            _service.ObterRelatorioMensal(
                                mes,
                                ano));
                        break;
                    }

                case "3":
                    {
                        int ano = ValidadorEntrada.LerInteiro(
                            "Informe o ano:",
                            1900,
                            DateTime.Now.Year);

                        ExibirRelatorio(
                            _service.ObterRelatorioAnual(
                                ano));
                        break;
                    }

                case "4":
                    ExibirCategorias(
                        _service.ObterGastosPorCategoria());
                    break;

                case "0":
                    continuar = false;
                    break;
            }

            if (continuar)
            {
                Formatting.AguardarRetorno();
            }
        }
    }

    private void ExibirRelatorio(
        List<Transacao> transacoes)
    {
        if (!transacoes.Any())
        {
            Console.WriteLine("Nenhuma transação encontrada.");
            return;
        }

        Console.WriteLine(
            $"{"Data",-12} {"Tipo",-10} {"Categoria",-15} {"Descrição",-25} {"Valor",12}");
        Console.WriteLine(new string('-', 80));

        foreach (var transacao in transacoes)
        {
            Console.WriteLine(
                $"{transacao.Data:dd/MM/yyyy,-12} " +
                $"{transacao.Tipo,-10} " +
                $"{transacao.Categoria,-15} " +
                $"{transacao.Descricao,-25} " +
                $"R$ {transacao.Valor,8:F2}");
        }

        Console.WriteLine();

        var (receitas, despesas, saldo) =
            _service.ObterResumo(transacoes);

        Console.WriteLine($"Total de Receitas : R$ {receitas:F2}");
        Console.WriteLine($"Total de Despesas : R$ {despesas:F2}");
        Console.WriteLine($"Saldo             : R$ {saldo:F2}");
    }

    private void ExibirCategorias(
        List<GastoCategoriaResumo> categorias)
    {
        if (!categorias.Any())
        {
            Console.WriteLine("Nenhuma despesa cadastrada.");
            return;
        }

        Console.WriteLine(
            $"{"Categoria",-20} {"Total",15} {"Percentual",15}");
        Console.WriteLine(new string('-', 55));

        foreach (var categoria in categorias)
        {
            Console.WriteLine(
                $"{categoria.Categoria,-20}" +
                $"R$ {categoria.Total,12:F2} " +
                $"{categoria.Percentual:P1}");
        }
    }
}