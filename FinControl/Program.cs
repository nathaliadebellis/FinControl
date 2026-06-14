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

while (executarSistema)
{
    Console.Clear();
    Console.WriteLine("╔════════════════════════════════════╗");
    Console.WriteLine("║      BEM-VINDO AO FINCONTROL       ║");
    Console.WriteLine("║  Seu sistema de gestão financeira  ║");
    Console.WriteLine("╚════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine("Para continuar, escolha uma opção:");
    Console.WriteLine("1 - Adicionar Transação");
    Console.WriteLine("2 - Listar Transações");
    Console.WriteLine("3 - Ver Saldo Atual");
    Console.WriteLine("4 - Buscar Transação");
    Console.WriteLine("5 - Editar Transação");
    Console.WriteLine("6 - Excluir Transação");
    Console.WriteLine("7 - Relatório Financeiro");
    Console.WriteLine("8 - Backup e Recuperação");
    Console.WriteLine("9 - Ver Erros Recentes");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    string opcao = ValidadorEntrada.LerOpcaoMenu(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });

    switch (opcao)
    {
        case "1":
            CadastrarTransacao();
            break;

        case "2":
            ListarTransacoes();
            break;

        case "3":
            MostrarSaldo();
            break;

        case "4":
            BuscarTransacao();
            break;

        case "5":
            EditarTransacao();
            break;

        case "6":
            ExcluirTransacao();
            break;

        case "7":
            RelatorioService.RelatorioFinanceiro(transacoes);
            break;

        case "8":
            MenuBackupRecuperacao();
            break;

        case "9":
            GerenciadorErros.ExibirRelatorioDErros();
            break;

        case "0":
            Console.WriteLine("Obrigado por utilizar o FinControl!");
            executarSistema = false;
            break;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    void CadastrarTransacao()
    {
        Transacao transacao = new Transacao();

        transacao.Id = proximoId++;

        transacao.Date = DateTime.Now;

        Console.WriteLine("Digite a descrição:");
        transacao.Description = Console.ReadLine();

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
                transacao.Category = Categorias.Lista[opcaoCategoria - 1];
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

        if (tipoOpcao == "1")
        {
            transacao.Type = "Receita";
        }
        else
        {
            transacao.Type = "Despesa";
        }

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

        transacao.Value = valor;

        transacoes.Add(transacao);

        SalvarTransacoes();

        Console.WriteLine("Transação cadastrada com sucesso!");
    }

    void ListarTransacoes()
    {
        if (transacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma transação cadastrada.");
            return;
        }

        foreach (var item in transacoes)
        {
            Console.WriteLine($"ID: {item.Id}");
            Console.WriteLine($"Data: {item.Date:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Descrição: {item.Description}");
            Console.WriteLine($"Categoria: {item.Category}");
            Console.WriteLine($"Tipo: {item.Type}");
            Console.WriteLine($"Valor: R$ {item.Value}");
            Console.WriteLine("--------------------");
        }
    }

    void MostrarSaldo()
    {
        decimal saldo = 0;
        foreach (var item in transacoes)
        {
            if (item.Type == "Receita")
            {
                saldo += item.Value;
            }
            else if (item.Type == "Despesa")
            {
                saldo -= item.Value;
            }
        }

        Console.WriteLine($"Saldo atual: R$ {saldo}");
    }

    void BuscarTransacao()
    {
        Console.WriteLine("Digite a descrição da transação que deseja buscar:");
        string descricaoBuscar = Console.ReadLine();
        bool encontrou = false;
        foreach (var item in transacoes)
        {
            if (item.Description.ToUpper().Contains(descricaoBuscar.ToUpper()))
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Data: {item.Date:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Descrição: {item.Description}");
                Console.WriteLine($"Categoria: {item.Category}");
                Console.WriteLine($"Tipo: {item.Type}");
                Console.WriteLine($"Valor: R$ {item.Value}");
                Console.WriteLine("--------------------");
                encontrou = true;
            }
        }
        if (!encontrou)
        {
            Console.WriteLine("Transação não encontrada.");
        }
    }

    void EditarTransacao()
    {
        Console.WriteLine("Digite a descrição da transação que deseja editar:");
        string descricaoEditar = Console.ReadLine();
        bool encontrou = false;
        foreach (var item in transacoes)
        {
            if (item.Description.ToUpper() == descricaoEditar.ToUpper())
            {
                Console.WriteLine($"Descrição atual: {item.Description}");
                Console.WriteLine($"Categoria atual: {item.Category}");
                Console.WriteLine($"Tipo atual: {item.Type}");
                Console.WriteLine($"Valor atual: R$ {item.Value}");

                Console.WriteLine("Digite a nova descrição:");
                item.Description = Console.ReadLine();
                Console.WriteLine("Digite a nova categoria:");
                item.Category = Console.ReadLine();
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
                if (tipoOpcao == "1")
                {
                    item.Type = "Receita";
                }
                else
                {
                    item.Type = "Despesa";
                }
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
                item.Value = valor;
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
            if (item.Description == descricaoExcluir)
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

    void MenuBackupRecuperacao()
    {
        bool continuarMenu = true;

        while (continuarMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║    BACKUP E RECUPERAÇÃO            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine();
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
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("     CRIAR BACKUP MANUAL           ");
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine();

        bool sucesso = GerenciadorErros.CriarBackup(caminhoArquivo);

        if (sucesso)
        {
            ValidadorEntrada.MostrarSucesso("Backup criado com sucesso!");
        }
        else
        {
            ValidadorEntrada.MostrarErro("Falha ao criar backup.");
        }

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    void ListarBackups()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("     BACKUPS DISPONÍVEIS           ");
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine();

        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
            return;
        }

        var arquivos = Directory.GetFiles(diretorioBackup, "*.json")
            .OrderByDescending(f => File.GetLastWriteTime(f))
            .ToList();

        if (arquivos.Count == 0)
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
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

        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    void RestaurarBackup()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("     RESTAURAR DO BACKUP           ");
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine();

        string diretorioBackup = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorioBackup))
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
            return;
        }

        var arquivos = Directory.GetFiles(diretorioBackup, "*.json")
            .OrderByDescending(f => File.GetLastWriteTime(f))
            .ToList();

        if (arquivos.Count == 0)
        {
            ValidadorEntrada.MostrarInfo("Nenhum backup disponível.");
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
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
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
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

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    void ExibirInfoBackup()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("     INFORMAÇÕES DE BACKUP         ");
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine();

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

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}