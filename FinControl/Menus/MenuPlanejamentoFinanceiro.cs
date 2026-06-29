using FinControl.Models;
using FinControl.Services;

namespace FinControl.Menus;

public class MenuPlanejamentoFinanceiro
{
    private readonly OrcamentoService _orcamentoService;
    private readonly MetaEconomiaService _metaEconomiaService;
    private readonly FinanceiroService _financeiroService;

    public MenuPlanejamentoFinanceiro(
        OrcamentoService orcamentoService,
        MetaEconomiaService metaEconomiaService,
        FinanceiroService financeiroService)
    {
        _orcamentoService = orcamentoService;
        _metaEconomiaService = metaEconomiaService;
        _financeiroService = financeiroService;
    }

    public void Exibir()
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("PLANEJAMENTO FINANCEIRO");

            Console.WriteLine("1 - Ver Orçamentos");
            Console.WriteLine("2 - Adicionar ou Atualizar Orçamento");
            Console.WriteLine("3 - Remover Orçamento");
            Console.WriteLine("4 - Alertas de orçamento");
            Console.WriteLine("5 - Meta de Economia");
            Console.WriteLine("6 - Ver Meta");
            Console.WriteLine("7 - Remover Meta");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "4", "5", "6", "7", "0" });

            Console.Clear();

            switch (opcao)
            {
                case "1":
                    {
                        var orcamentos = _orcamentoService.ListarOrcamentos();

                        if (!orcamentos.Any())
                        {
                            ValidadorEntrada.MostrarInfo("Nenhum orçamento cadastrado.");
                        }
                        else
                        {
                            foreach (var orcamento in orcamentos)
                            {
                                Console.WriteLine(
                                    $"{orcamento.Categoria} - Limite: R$ {orcamento.LimiteMensal:F2}");
                            }
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "2":
                    {
                        Console.WriteLine("Escolha a categoria:");

                        for (int i = 0; i < Categorias.Despesas.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {Categorias.Despesas[i]}");
                        }

                        int opcaoCategoria = ValidadorEntrada.LerInteiro(
                            "Opção:",
                            1,
                            Categorias.Despesas.Count);

                        string categoria = Categorias.Despesas[opcaoCategoria - 1];

                        decimal limite = ValidadorEntrada.LerDecimal(
                            "Limite mensal:",
                            0.01m);

                        _orcamentoService.DefinirOuAtualizarOrcamento(
                            categoria,
                            limite);

                        ValidadorEntrada.MostrarSucesso("Orçamento salvo com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "3":
                    {
                        var orcamentos = _orcamentoService.ListarOrcamentos();

                        if (!orcamentos.Any())
                        {
                            ValidadorEntrada.MostrarInfo("Nenhum orçamento cadastrado.");
                            Formatting.AguardarRetorno();
                            break;
                        }

                        Console.WriteLine("Escolha o orçamento para remover:");

                        for (int i = 0; i < orcamentos.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {orcamentos[i].Categoria}");
                        }

                        int opcaoRemover = ValidadorEntrada.LerInteiro(
                            "Opção:",
                            1,
                            orcamentos.Count);

                        _orcamentoService.RemoverOrcamento(
                            orcamentos[opcaoRemover - 1].Categoria);

                        ValidadorEntrada.MostrarSucesso("Orçamento removido com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "4":
                    {
                        var alertas = _orcamentoService.VerificarAlertas();

                        if (!alertas.Any())
                        {
                            ValidadorEntrada.MostrarInfo("Nenhum alerta encontrado.");
                        }
                        else
                        {
                            foreach (var alerta in alertas)
                            {
                                Console.WriteLine($"• {alerta}");
                            }
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "5":
                    {
                        decimal meta = ValidadorEntrada.LerDecimal(
                            "Meta de economia:",
                            0.01m);

                        _metaEconomiaService.DefinirMeta(meta);

                        ValidadorEntrada.MostrarSucesso("Meta definida com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "6":
                    {
                        var meta = _metaEconomiaService.ObterMeta();

                        if (meta.ValorMeta <= 0)
                        {
                            ValidadorEntrada.MostrarInfo("Nenhuma meta de economia cadastrada.");
                            Formatting.AguardarRetorno();
                            break;
                        }

                        decimal economiaAtual = _financeiroService.CalcularSaldo();

                        decimal progresso = _metaEconomiaService.CalcularProgresso(
                            economiaAtual,
                            meta.ValorMeta);

                        decimal restante = _metaEconomiaService.CalcularValorRestante(
                            economiaAtual,
                            meta.ValorMeta);

                        Console.WriteLine("===== META DE ECONOMIA =====");
                        Console.WriteLine($"Meta definida:      R$ {meta.ValorMeta:F2}");
                        Console.WriteLine($"Economia atual:     R$ {economiaAtual:F2}");
                        Console.WriteLine($"Progresso:          {progresso:F2}%");
                        Console.WriteLine($"Status:             {_metaEconomiaService.ObterStatusMeta(economiaAtual, meta.ValorMeta)}");

                        if (!_metaEconomiaService.MetaFoiAtingida(economiaAtual, meta.ValorMeta))
                        {
                            Console.WriteLine($"Faltam:             R$ {restante:F2}");
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "7":
                    {
                        _metaEconomiaService.RemoverMeta();

                        ValidadorEntrada.MostrarSucesso("Meta removida com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "0":
                    continuar = false;
                    break;
            }
        }
    }
}