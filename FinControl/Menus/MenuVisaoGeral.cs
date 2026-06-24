using FinControl.Models;
using FinControl.Services;

namespace FinControl.Menus;

public static class MenuVisaoGeral
{
    public static void Exibir(List<Transacao> transacoes)
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("VISÃO GERAL");

            Console.WriteLine("1 - Dashboard");
            Console.WriteLine("2 - Saldo");
            Console.WriteLine("3 - Relatórios");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "0" });

            switch (opcao)
            {
                case "1":
                    ExibirDashboard(transacoes);
                    break;

                case "2":
                    var (_, _, saldo) =
                        FinanceiroService.CalcularResumo(transacoes);

                    Console.WriteLine($"Saldo: R$ {saldo:F2}");
                    Formatting.AguardarRetorno();
                    break;

                case "3":
                    MenuRelatorios.Exibir(transacoes);
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    private static void ExibirDashboard(List<Transacao> transacoes)
    {
        DashboardResumo resumo =
            FinanceiroService.GerarDashboardResumo(transacoes);

        Console.Clear();
        Console.WriteLine(DashboardService.GerarTexto(resumo));
        Formatting.AguardarRetorno();
    }
}