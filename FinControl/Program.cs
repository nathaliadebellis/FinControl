using FinControl.Core;
using FinControl.Menus;
using FinControl.Models;
using FinControl.Repositories;
using FinControl.Services;

// Inicialização
GerenciadorErros.Inicializar();

// Carrega os dados
List<Transacao> transacoes = TransacaoRepository.Carregar();
List<OrcamentoCategoria> orcamentos = OrcamentoRepository.Carregar();

// Limpeza automática de backups antigos
GerenciadorErros.LimparBackupsAntigos(
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "transacoes.json"),
    30);

bool executarSistema = true;

while (executarSistema)
{
    Console.Clear();

    Formatting.ExibirCabecalho("FINCONTROL");

    Console.WriteLine("1 - Visão Geral");
    Console.WriteLine("2 - Transações");
    Console.WriteLine("3 - Planejamento Financeiro");
    Console.WriteLine("4 - Sistema");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    string opcao = ValidadorEntrada.LerOpcaoMenu(
        new[] { "1", "2", "3", "4", "0" });

    switch (opcao)
    {
        case "1":
            MenuVisaoGeral.Exibir(transacoes);
            break;

        case "2":
            MenuTransacoes.Exibir(transacoes);
            TransacaoRepository.Salvar(transacoes);
            break;

        case "3":
            MenuPlanejamentoFinanceiro.Exibir(transacoes);
            OrcamentoRepository.Salvar(orcamentos);
            break;

        case "4":
            MenuSistema.Exibir();
            break;

        case "0":
            executarSistema = false;
            break;
    }
}

Console.WriteLine();
Console.WriteLine("Obrigado por utilizar o FinControl!");