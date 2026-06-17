using FinControl.Models;
using FinControl.Services;
using System.Text.Json;

// Inicializa gerenciador de erros
GerenciadorErros.Inicializar();

string caminhoArquivo = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "Data",
    "transacoes.json");

List<Transacao> transacoes = new List<Transacao>();

int proximoId = 1;

bool executarSistema = true;

void SalvarTransacoes()
{
    bool sucesso = GerenciadorErros.TratarSalvamentoDados(() =>
    {
        // Cria backup antes de salvar
        GerenciadorErros.CriarBackup(caminhoArquivo);

        Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo)!);

        string json = JsonSerializer.Serialize(
            transacoes,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(caminhoArquivo, json);
    },
    "Transações");
}

void CarregarTransacoes()
{
    transacoes = GerenciadorErros.TratarCarregamentoDados(() =>
    {
        string pasta = Path.GetDirectoryName(caminhoArquivo)!;
        Directory.CreateDirectory(pasta);

        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            return JsonSerializer.Deserialize<List<Transacao>>(json)
                   ?? new List<Transacao>();
        }

        return new List<Transacao>();
    },
    "Transações",
    new List<Transacao>()) ?? new List<Transacao>();
}

// Inicializa limpeza de backups antigos
GerenciadorErros.LimparBackupsAntigos(caminhoArquivo, 30);

CarregarTransacoes();
if (transacoes.Count > 0)
{
    proximoId = transacoes.Max(t => t.Id) + 1;
}

List<OrcamentoCategoria> orcamentos = OrcamentoService.Carregar();

Console.WriteLine($"Orçamentos carregados: {orcamentos.Count}");

while (executarSistema)
{
    Console.Clear();
    Console.WriteLine("╔════════════════════════════════════╗");
    Console.WriteLine("║      BEM-VINDO AO FINCONTROL       ║");
    Console.WriteLine("║  Seu sistema de gestão financeira  ║");
    Console.WriteLine("╚════════════════════════════════════╝");
    Console.WriteLine();

    Console.WriteLine("1 - Visão Geral");
    Console.WriteLine("2 - Transações");
    Console.WriteLine("3 - Planejamento Financeiro");
    Console.WriteLine("4 - Sistema");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    string opcao = ValidadorEntrada.LerOpcaoMenu(new[] { "1", "2", "3", "4", "0" });

    switch (opcao)
    {
        case "1":
            MenuVisaoGeral();
            break;

        case "2":
            MenuTransacoes();
            break;

        case "3":
            MenuPlanejamentoFinanceiro();
            break;

        case "4":
            MenuSistema();
            break;

        case "0":
            Console.WriteLine("Obrigado por utilizar o FinControl!");
            executarSistema = false;
            break;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    void MenuVisaoGeral()
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("VISÃO GERAL");
            Console.WriteLine("1 - Dashboard Financeiro");
            Console.WriteLine("2 - Ver Saldo Atual");
            Console.WriteLine("3 - Relatório Financeiro");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "0" });

            switch (opcao)
            {
                case "1":
                    DashboardService.Exibir(transacoes);
                    break;

                case "2":
                    MostrarSaldo();
                    break;

                case "3":
                    RelatorioService.RelatorioFinanceiro(transacoes);
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    void MenuTransacoes()
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("TRANSAÇÕES");
            Console.WriteLine("1 - Adicionar Transação");
            Console.WriteLine("2 - Listar Transações");
            Console.WriteLine("3 - Buscar Transação");
            Console.WriteLine("4 - Editar Transação");
            Console.WriteLine("5 - Excluir Transação");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "4", "5", "0" });

            switch (opcao)
            {
                case "1":
                    CadastrarTransacao();
                    break;

                case "2":
                    ListarTransacoes();
                    break;

                case "3":
                    BuscarTransacao();
                    break;

                case "4":
                    EditarTransacao();
                    break;

                case "5":
                    ExcluirTransacao();
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    void MenuPlanejamentoFinanceiro()
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("PLANEJAMENTO FINANCEIRO");
            Console.WriteLine("1 - Gerenciar Orçamentos");
            Console.WriteLine("2 - Meta de Economia");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "0" });

            switch (opcao)
            {
                case "1":
                    MenuGerenciarOrcamentos();
                    break;

                case "2":
                    MenuMetaEconomia();
                    break;

                case "0":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    void MenuSistema()
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("SISTEMA");
            Console.WriteLine("1 - Backup e Recuperação");
            Console.WriteLine("2 - Ver Erros Recentes");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "0" });

            switch (opcao)
            {
                case "1":
                    MenuBackupRecuperacao();
                    break;

                case "2":
                    GerenciadorErros.ExibirRelatorioDErros();
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    void CadastrarTransacao()
    {
        Transacao transacao = new Transacao();

        transacao.Id = proximoId++;

        transacao.Data = DateTime.Now; // Timestamp de criação da transação.

        Console.WriteLine("Digite a descrição:");
        transacao.Descricao = Console.ReadLine();

        Console.WriteLine("Escolha a categoria:");

        for (int i = 0; i < Categorias.Lista.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {Categorias.Lista[i]}");
        }

        int opcaoCategoria;
        bool categoriaValida = false;

        while (!categoriaValida)
        {
            categoriaValida = int.TryParse(
                Console.ReadLine(),
                out opcaoCategoria
            );

            if (categoriaValida &&
                opcaoCategoria >= 1 &&
                opcaoCategoria <= Categorias.Lista.Count)
            {
                transacao.Categoria = Categorias.Lista[opcaoCategoria - 1];
            }
            else
            {
                categoriaValida = false;
                Console.WriteLine("Categoria inválida. Tente novamente.");
            }
        }

        Console.WriteLine("Digite o tipo:");
        Console.WriteLine("1 - Receita");
        Console.WriteLine("2 - Despesa");

        string tipoOpcao = Console.ReadLine();

        while (tipoOpcao != "1" && tipoOpcao != "2")
        {
            Console.WriteLine("Tipo inválido. Tente novamente.");

            Console.WriteLine("1 - Receita");
            Console.WriteLine("2 - Despesa");

            tipoOpcao = Console.ReadLine();
        }

        transacao.Tipo = tipoOpcao == "1" ? TipoTransacao.Receita : TipoTransacao.Despesa;

        decimal valor = 0;
        bool valorValido = false;

        while (!valorValido)
        {
            Console.WriteLine("Digite o valor:");

            valorValido = decimal.TryParse(
                Console.ReadLine(),
                out valor
            );

            if (!valorValido)
            {
                Console.WriteLine("Valor inválido. Tente novamente.");
            }
        }

        transacao.Valor = valor;

        transacoes.Add(transacao);

        SalvarTransacoes();

        Console.WriteLine("Transação cadastrada com sucesso!");
    }

    /// <summary>
    /// Lista todas as transações no console usando a formatação da aplicação.
    /// </summary>
    void ListarTransacoes()
    {
        if (transacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma transação cadastrada.");
            return;
        }

        foreach (var item in transacoes)
        {
            Formatting.PrintTransacao(item);
        }
    }

    void MostrarSaldo()
    {
        decimal saldo = 0;

        foreach (var item in transacoes)
        {
            if (item.Tipo == TipoTransacao.Receita)
            {
                saldo += item.Valor;
            }
            else if (item.Tipo == TipoTransacao.Despesa)
            {
                saldo -= item.Valor;
            }
        }

        Formatting.ExibirCabecalho("SALDO ATUAL");
        Console.WriteLine($"Saldo atual: R$ {saldo:F2}");

        Formatting.AguardarRetorno();
    }

    /// <summary>
    /// Realiza uma busca interativa por transações usando parte da descrição e exibe as correspondências.
    /// </summary>
    void BuscarTransacao()
    {
        Console.WriteLine("Digite a descrição da transação que deseja buscar:");
        string descricaoBuscar = Console.ReadLine();
        bool encontrou = false;
        foreach (var item in transacoes)
        {
            if (item.Descricao.ToUpper().Contains(descricaoBuscar.ToUpper()))
            {
                Formatting.PrintTransacao(item);
                encontrou = true;
            }
        }
        if (!encontrou)
        {
            Console.WriteLine("Transação não encontrada.");
        }
    }

    /// <summary>
    /// Edita uma transação localizada por correspondência exata da descrição e solicita novos valores.
    /// </summary>
    void EditarTransacao()
    {
        Console.WriteLine("Digite a descrição da transação que deseja editar:");
        string descricaoEditar = Console.ReadLine();
        bool encontrou = false;
        foreach (var item in transacoes)
        {
            if (item.Descricao.ToUpper() == descricaoEditar.ToUpper())
            {
                Console.WriteLine($"Descrição atual: {item.Descricao}");
                Console.WriteLine($"Categoria atual: {item.Categoria}");
                Console.WriteLine($"Tipo atual: {item.Tipo}");
                Console.WriteLine($"Valor atual: R$ {item.Valor:F2}");

                Console.WriteLine("Digite a nova descrição:");
                item.Descricao = Console.ReadLine();
                Console.WriteLine("Digite a nova categoria:");
                item.Categoria = Console.ReadLine();
                Console.WriteLine("Digite o novo tipo:");
                Console.WriteLine("1 - Receita");
                Console.WriteLine("2 - Despesa");
                string tipoOpcao = Console.ReadLine();
                while (tipoOpcao != "1" && tipoOpcao != "2")
                {
                    Console.WriteLine("Tipo inválido. Tente novamente.");
                    Console.WriteLine("1 - Receita");
                    Console.WriteLine("2 - Despesa");
                    tipoOpcao = Console.ReadLine();
                }
                item.Tipo = tipoOpcao == "1" ? TipoTransacao.Receita : TipoTransacao.Despesa;
                decimal valor = 0;
                bool valorValido = false;
                while (!valorValido)
                {
                    Console.WriteLine("Digite o novo valor:");
                    valorValido = decimal.TryParse(
                        Console.ReadLine(),
                        out valor
                    );
                    if (!valorValido)
                    {
                        Console.WriteLine("Valor inválido. Tente novamente.");
                    }
                }
                item.Valor = valor;
                encontrou = true;
                SalvarTransacoes();
                Console.WriteLine("Transação editada com sucesso!");
                break;
            }
        }
        if (!encontrou)
        {
            Console.WriteLine("Transação não encontrada.");
        }
    }

    void ExcluirTransacao()
    {
        Console.WriteLine("Digite a descrição da transação que deseja excluir:");
        string descricaoExcluir = Console.ReadLine();

        bool encontrou = false;

        foreach (var item in transacoes.ToList())
        {
            if (item.Descricao == descricaoExcluir)
            {
                transacoes.Remove(item);
                encontrou = true;
                SalvarTransacoes();

                Console.WriteLine("Transação removida com sucesso!");
                break;
            }
        }

        if (!encontrou)
        {
            Console.WriteLine("Transação não encontrada.");
        }
    }


    void MenuGerenciarOrcamentos()
    {
        bool continuar = true;

        while (continuar)
        {

            Formatting.ExibirCabecalho("GERENCIAR ORÇAMENTOS ");

            Console.WriteLine("1 - Definir orçamento");
            Console.WriteLine("2 - Listar orçamentos");
            Console.WriteLine("3 - Remover orçamento");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = Console.ReadLine() ?? "";

            switch (opcao)
            {
                case "1":
                    OrcamentoService.DefinirOrcamento(orcamentos);
                    break;

                case "2":
                    OrcamentoService.ListarOrcamentos(orcamentos);
                    break;

                case "3":
                    OrcamentoService.RemoverOrcamento(orcamentos);
                    break;

                case "0":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    void MenuMetaEconomia()
    {
        bool continuar = true;

        while (continuar)
        {

            Formatting.ExibirCabecalho("META DE ECONOMIA");
            Console.WriteLine("1 - Definir meta");
            Console.WriteLine("2 - Visualizar meta");
            Console.WriteLine("3 - Remover meta");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = Console.ReadLine() ?? "";

            switch (opcao)
            {
                case "1":
                    DefinirMetaEconomia();
                    break;

                case "2":
                    VisualizarMetaEconomia();
                    break;

                case "3":
                    RemoverMetaEconomia();
                    break;

                case "0":
                    continuar = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    void DefinirMetaEconomia()
    {
        Console.Clear();

        Console.WriteLine("===== DEFINIR META DE ECONOMIA =====");
        Console.WriteLine();

        decimal valorMeta = 0;
        bool valido = false;

        while (!valido)
        {
            Console.Write("Digite sua meta mensal de economia: R$ ");

            valido = decimal.TryParse(Console.ReadLine(), out valorMeta)
                      && valorMeta > 0;

            if (!valido)
            {
                Console.WriteLine("Informe um valor válido maior que zero.");
            }
        }

        MetaEconomia meta = new()
        {
            ValorMeta = valorMeta
        };

        MetaEconomiaService.Salvar(meta);

        Console.WriteLine();
        Console.WriteLine("Meta salva com sucesso!");

        Formatting.AguardarRetorno();
    }

    void VisualizarMetaEconomia()
    {
        Console.Clear();

        var meta = MetaEconomiaService.Carregar();

        Console.WriteLine("===== META DE ECONOMIA =====");
        Console.WriteLine();

        if (meta.ValorMeta <= 0)
        {
            Console.WriteLine("Nenhuma meta cadastrada.");
        }
        else
        {
            Console.WriteLine($"Meta atual: R$ {meta.ValorMeta:F2}");
        }

        Formatting.AguardarRetorno();
    }

    void RemoverMetaEconomia()
    {
        Console.Clear();

        MetaEconomiaService.Salvar(new MetaEconomia());

        Console.WriteLine("Meta removida com sucesso!");

        Formatting.AguardarRetorno();
    }

    void MenuBackupRecuperacao()
    {
        bool continuarMenu = true;

        while (continuarMenu)
        {

            Formatting.ExibirCabecalho("BACKUP E RECUPERAÇÃO");
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Criar Backup Manual");
            Console.WriteLine("2 - Listar Backups");
            Console.WriteLine("3 - Restaurar do Backup");
            Console.WriteLine("4 - Informações de Backup");
            Console.WriteLine("0 - Voltar ao Menu Principal");
            Console.WriteLine();

            string opcaoBackup = ValidadorEntrada.LerOpcaoMenu(new[] { "1", "2", "3", "4", "0" });

            switch (opcaoBackup)
            {
                case "1":
                    CriarBackupManual();
                    break;
                case "2":
                    ListarBackups();
                    break;
                case "3":
                    RestaurarBackup();
                    break;
                case "4":
                    ExibirInfoBackup();
                    break;
                case "0":
                    continuarMenu = false;
                    break;
            }
        }
    }

    void CriarBackupManual()
    {

        Formatting.ExibirCabecalho("CRIAR BACKUP MANUAL");

        bool sucesso = GerenciadorErros.CriarBackup(caminhoArquivo);

        if (sucesso)
        {
            ValidadorEntrada.MostrarSucesso("Backup criado com sucesso!");
        }
        else
        {
            ValidadorEntrada.MostrarErro("Falha ao criar backup.");
        }

        Formatting.AguardarRetorno();
    }

    void ListarBackups()
    {

        Formatting.ExibirCabecalho("BACKUPS DISPONÍVEIS");

        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Formatting.AguardarRetorno();
            return;
        }

        var arquivos = Directory.GetFiles(diretorioBackup, "*.json")
            .OrderByDescending(f => File.GetLastWriteTime(f))
            .ToList();

        if (arquivos.Count == 0)
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Formatting.AguardarRetorno();
            return;
        }

        for (int i = 0; i < arquivos.Count; i++)
        {
            FileInfo info = new FileInfo(arquivos[i]);
            Console.WriteLine($"{i + 1}. {info.Name}");
            Console.WriteLine($"   Data: {info.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine($"   Tamanho: {(info.Length / 1024.0):F2} KB");
            Console.WriteLine();
        }

        Formatting.AguardarRetorno();
    }

    void RestaurarBackup()
    {

        Formatting.ExibirCabecalho("RESTAURAR BACKUP");

        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Formatting.AguardarRetorno();
            return;
        }

        var arquivos = Directory.GetFiles(diretorioBackup, "*.json")
            .OrderByDescending(f => File.GetLastWriteTime(f))
            .ToList();

        if (arquivos.Count == 0)
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Formatting.AguardarRetorno();
            return;
        }

        Console.WriteLine("Backups disponíveis:");
        for (int i = 0; i < arquivos.Count; i++)
        {
            FileInfo info = new FileInfo(arquivos[i]);
            Console.WriteLine($"{i + 1}. {info.Name} ({info.LastWriteTime:dd/MM/yyyy HH:mm:ss})");
        }

        int escolha = ValidadorEntrada.LerInteiro("\nEscolha o número do backup:", 1, arquivos.Count);
        string backupSelecionado = arquivos[escolha - 1];

        Console.Write("\nDeseja realmente restaurar este backup? Isso sobrescreverá os dados atuais. (S/N): ");
        string confirmacao = Console.ReadLine()?.ToUpper() ?? "N";

        if (confirmacao != "S")
        {
            ValidadorEntrada.MostrarInfo("Restauração cancelada.");
            Formatting.AguardarRetorno();
            return;
        }

        try
        {
            // Cria backup dos dados atuais antes de restaurar
            GerenciadorErros.CriarBackup(caminhoArquivo);

            // Restaura do backup
            File.Copy(backupSelecionado, caminhoArquivo, true);

            // Recarrega transações
            CarregarTransacoes();

            ValidadorEntrada.MostrarSucesso("Backup restaurado com sucesso!");
        }
        catch (Exception ex)
        {
            ValidadorEntrada.MostrarErro($"Erro ao restaurar backup: {ex.Message}");
        }

        Formatting.AguardarRetorno();
    }

    void ExibirInfoBackup()
    {

        Formatting.ExibirCabecalho("INFORMAÇÕES DE BACKUP");

        // Informações do arquivo principal
        if (File.Exists(caminhoArquivo))
        {
            FileInfo info = new FileInfo(caminhoArquivo);
            Console.WriteLine("Arquivo Principal:");
            Console.WriteLine($"  Caminho: {caminhoArquivo}");
            Console.WriteLine($"  Tamanho: {(info.Length / 1024.0):F2} KB");
            Console.WriteLine($"  Última modificação: {info.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine($"  Transações: {transacoes.Count}");
        }

        // Informações de backups
        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (Directory.Exists(diretorioBackup))
        {
            var backups = Directory.GetFiles(diretorioBackup, "*.json");
            Console.WriteLine();
            Console.WriteLine($"Backups Disponíveis: {backups.Length}");

            if (backups.Length > 0)
            {
                long tamanhoTotal = backups.Sum(f => new FileInfo(f).Length);
                Console.WriteLine($"Tamanho Total: {(tamanhoTotal / 1024.0):F2} KB");

                var mais_recente = backups
                    .Select(f => new { Path = f, Info = new FileInfo(f) })
                    .OrderByDescending(x => x.Info.LastWriteTime)
                    .First();

                Console.WriteLine($"Backup Mais Recente: {Path.GetFileName(mais_recente.Path)}");
                Console.WriteLine($"  Data: {mais_recente.Info.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Backups Disponíveis: 0");
        }

        Formatting.AguardarRetorno();
    }
}