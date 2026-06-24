using FinControl.Models;
using FinControl.Services;

namespace FinControl.Menus;

public static class MenuPlanejamentoFinanceiro
{
    public static void Exibir(List<Transacao> transacoes)
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
                        var orcamentos = OrcamentoService.ListarOrcamentos();

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

                        OrcamentoService.DefinirOuAtualizarOrcamento(
                            categoria,
                            limite);

                        ValidadorEntrada.MostrarSucesso("Orçamento salvo com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "3":
                    {
                        var orcamentos = OrcamentoService.ListarOrcamentos();

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

                        OrcamentoService.RemoverOrcamento(
                            orcamentos[opcaoRemover - 1].Categoria);

                        ValidadorEntrada.MostrarSucesso("Orçamento removido com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "4":
                    {
                        var alertas = OrcamentoService.VerificarAlertas(transacoes);

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

                        MetaEconomiaService.DefinirMeta(meta);

                        ValidadorEntrada.MostrarSucesso("Meta definida com sucesso!");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "6":
                    {
                        var meta = MetaEconomiaService.ObterMeta();

                        if (meta.ValorMeta <= 0)
                        {
                            ValidadorEntrada.MostrarInfo("Nenhuma meta de economia cadastrada.");
                            Formatting.AguardarRetorno();
                            break;
                        }

                        decimal economiaAtual = FinanceiroService.CalcularSaldo(transacoes);

                        decimal progresso = MetaEconomiaService.CalcularProgresso(
                            economiaAtual,
                            meta.ValorMeta);

                        decimal restante = MetaEconomiaService.CalcularValorRestante(
                            economiaAtual,
                            meta.ValorMeta);

                        Console.WriteLine("===== META DE ECONOMIA =====");
                        Console.WriteLine($"Meta definida:      R$ {meta.ValorMeta:F2}");
                        Console.WriteLine($"Economia atual:     R$ {economiaAtual:F2}");
                        Console.WriteLine($"Progresso:          {progresso:F2}%");
                        Console.WriteLine($"Status:             {MetaEconomiaService.ObterStatusMeta(economiaAtual, meta.ValorMeta)}");

                        if (!MetaEconomiaService.MetaFoiAtingida(economiaAtual, meta.ValorMeta))
                        {
                            Console.WriteLine($"Faltam:             R$ {restante:F2}");
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "7":
                    {
                        MetaEconomiaService.RemoverMeta();

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