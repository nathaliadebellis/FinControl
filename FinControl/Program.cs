using FinControl.Interfaces;
using FinControl.Menus;
using FinControl.Models;
using FinControl.Repositories;
using FinControl.Repositories.Interfaces;
using FinControl.Services;
using FinControl.Utils;

// Inicialização
GerenciadorErros.Inicializar();

// Repositórios
ITransacaoRepository transacaoRepository =
    new JsonTransacaoRepository();

IOrcamentoRepository orcamentoRepository =
    new JsonOrcamentoRepository();

IMetaEconomiaRepository metaRepository =
    new JsonMetaEconomiaRepository();

// Serviços
var transacaoService =
    new TransacaoService(transacaoRepository);

var orcamentoService =
    new OrcamentoService(
        orcamentoRepository,
        transacaoService);

var financeiroService =
    new FinanceiroService(transacaoRepository);

var relatorioService =
    new RelatorioService(
        transacaoRepository,
        financeiroService);

var menuRelatorios =
    new MenuRelatorios(relatorioService);

var menuVisaoGeral =
    new MenuVisaoGeral(
        financeiroService,
        menuRelatorios);

var metaService =
    new MetaEconomiaService(metaRepository);

var sistemaService = new SistemaService();

var menuSistema = new MenuSistema(sistemaService);

var menuTransacoes = new MenuTransacoes(transacaoService);

var menuPlanejamentoFinanceiro =
    new MenuPlanejamentoFinanceiro(
        orcamentoService,
        metaService,
        financeiroService);

// Carregar dados
List<Transacao> transacoes =
    transacaoRepository.Carregar();

List<OrcamentoCategoria> orcamentos =
    orcamentoRepository.Carregar();

// Limpeza automática de backups antigos
sistemaService.LimparBackupsAntigos(30);

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
            menuVisaoGeral.Exibir();
            break;

        case "2":
            menuTransacoes.Exibir();
            break;

        case "3":
            menuPlanejamentoFinanceiro.Exibir();
            break;

        case "4":
            menuSistema.Exibir();
            break;

        case "0":
            executarSistema = false;
            break;
    }
}

Console.WriteLine();
Console.WriteLine("Obrigado por utilizar o FinControl!");