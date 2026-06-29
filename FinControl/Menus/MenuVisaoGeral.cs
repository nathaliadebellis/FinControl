using FinControl.Models;
using FinControl.Services;

namespace FinControl.Menus;

public class MenuVisaoGeral
{
    private readonly FinanceiroService _financeiroService;
    private readonly MenuRelatorios _menuRelatorios;

    public MenuVisaoGeral(
        FinanceiroService financeiroService,
        MenuRelatorios menuRelatorios)
    {
        _financeiroService = financeiroService;
        _menuRelatorios = menuRelatorios;
    }

    public void Exibir()
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
                    ExibirDashboard();
                    break;

                case "2":
                    var (_, _, saldo) =
                        _financeiroService.CalcularResumo();
                    break;

                case "3":
                    _menuRelatorios.Exibir();
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    private void ExibirDashboard()
    {
        DashboardResumo resumo =
            _financeiroService.GerarDashboardResumo();

        Console.Clear();
        Console.WriteLine(DashboardService.GerarTexto(resumo));
        Formatting.AguardarRetorno();
    }
}