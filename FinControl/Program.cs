using FinControl.Models;
using System.Text.Json;

string caminhoArquivo = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "Data",
    "transacoes.json");

List<Transacao> transacoes = new List<Transacao>();

int proximoId = 1;

bool executarSistema = true;

void SalvarTransacoes()
{
    Directory.CreateDirectory(
        Path.GetDirectoryName(caminhoArquivo)!
    );

    string json = JsonSerializer.Serialize(
        transacoes,
        new JsonSerializerOptions
        {
            WriteIndented = true
        });

    File.WriteAllText(caminhoArquivo, json);
}

void CarregarTransacoes()
{
    string pasta = Path.GetDirectoryName(caminhoArquivo)!;

    Directory.CreateDirectory(pasta);

    if (File.Exists(caminhoArquivo))
    {
        string json = File.ReadAllText(caminhoArquivo);

        transacoes = JsonSerializer.Deserialize<List<Transacao>>(json)
                     ?? new List<Transacao>();
    }
}

CarregarTransacoes();
    if (transacoes.Count > 0)
{
    proximoId = transacoes.Max(t => t.Id) + 1;
}

while (executarSistema)
{
    Console.WriteLine("Bem vindo ao FinControl!");
    Console.WriteLine("Seu sistema de gestão financeira pessoal");
    Console.WriteLine();
    Console.WriteLine("Para continuar, escolha uma opção:");
    Console.WriteLine("1 - Adicionar Transação");
    Console.WriteLine("2 - Listar Transações");
    Console.WriteLine("3 - Ver Saldo Atual");
    Console.WriteLine("4 - Buscar Transação");
    Console.WriteLine("5 - Editar Transação");
    Console.WriteLine("6 - Excluir Transação");
    Console.WriteLine("7 - Relatório Financeiro");
    Console.WriteLine("8 - Sair");
    string opcao = Console.ReadLine();

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
            RelatorioFinanceiro();
            break;

        case "8":
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

    void RelatorioFinanceiro()
    {
        bool executandoRelatorio = true;

        while (executandoRelatorio)
        {
            Console.WriteLine("=== RELATÓRIO FINANCEIRO ===");
            Console.WriteLine("1 - Relatório Geral");
            Console.WriteLine("2 - Relatório Mensal");
            Console.WriteLine("3 - Relatório Anual");
            Console.WriteLine("4 - Relatório Personalizado");
            Console.WriteLine("5 - Voltar");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    RelatorioGeral();
                    break;

                case "2":
                    RelatorioMensal();
                    break;

                case "3":
                    RelatorioAnual();
                    break;

                case "4":
                    RelatorioPersonalizado();
                    break;

                case "5":
                    executandoRelatorio = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    void RelatorioMensal()
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
        bool encontrou = false;

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

        foreach (var item in transacoes)
        {
            if (item.Date.Month == mes &&
                item.Date.Year == ano)
            {
                encontrou = true;

                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Data: {item.Date:dd/MM/yyyy}");
                Console.WriteLine($"Descrição: {item.Description}");
                Console.WriteLine($"Categoria: {item.Category}");
                Console.WriteLine($"Tipo: {item.Type}");
                Console.WriteLine($"Valor: R$ {item.Value:F2}");
                Console.WriteLine("--------------------");

                if (item.Type == "Receita")
                {
                    totalReceitas += item.Value;
                }
                else if (item.Type == "Despesa")
                {
                    totalDespesas += item.Value;
                }
            }
        }

        if (!encontrou)
        {
            Console.WriteLine("Nenhuma transação encontrada para este período.");
            return;
        }

        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO MÊS ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");
    }

    void RelatorioAnual()
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
        bool encontrou = false;

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($"RELATÓRIO ANUAL DE {ano}");
        Console.WriteLine("====================================");
        Console.WriteLine();

        foreach (var item in transacoes)
        {
            if (item.Date.Year == ano)
            {
                encontrou = true;

                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Data: {item.Date:dd/MM/yyyy}");
                Console.WriteLine($"Descrição: {item.Description}");
                Console.WriteLine($"Categoria: {item.Category}");
                Console.WriteLine($"Tipo: {item.Type}");
                Console.WriteLine($"Valor: R$ {item.Value:F2}");
                Console.WriteLine("--------------------");

                if (item.Type == "Receita")
                {
                    totalReceitas += item.Value;
                }
                else if (item.Type == "Despesa")
                {
                    totalDespesas += item.Value;
                }
            }
        }

        if (!encontrou)
        {
            Console.WriteLine("Nenhuma transação encontrada para este ano.");
            return;
        }

        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO ANO ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");
    }

    void RelatorioPersonalizado()
    {
        Console.WriteLine("Digite a data inicial (dd/MM/yyyy):");

        DateTime dataInicial;
        while (!DateTime.TryParse(Console.ReadLine(), out dataInicial))
        {
            Console.WriteLine("Data inválida. Tente novamente:");
        }

        Console.WriteLine("Digite a data final (dd/MM/yyyy):");

        DateTime dataFinal;
        while (!DateTime.TryParse(Console.ReadLine(), out dataFinal))
        {
            Console.WriteLine("Data inválida. Tente novamente:");
        }

        if (dataFinal < dataInicial)
        {
            Console.WriteLine("A data final não pode ser menor que a data inicial.");
            return;
        }

        decimal totalReceitas = 0;
        decimal totalDespesas = 0;
        bool encontrou = false;

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($"RELATÓRIO DE {dataInicial:dd/MM/yyyy} ATÉ {dataFinal:dd/MM/yyyy}");
        Console.WriteLine("====================================");
        Console.WriteLine();

        foreach (var item in transacoes)
        {
            if (item.Date.Date >= dataInicial.Date &&
                item.Date.Date <= dataFinal.Date)
            {
                encontrou = true;

                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Data: {item.Date:dd/MM/yyyy}");
                Console.WriteLine($"Descrição: {item.Description}");
                Console.WriteLine($"Categoria: {item.Category}");
                Console.WriteLine($"Tipo: {item.Type}");
                Console.WriteLine($"Valor: R$ {item.Value:F2}");
                Console.WriteLine("--------------------");

                if (item.Type == "Receita")
                {
                    totalReceitas += item.Value;
                }
                else if (item.Type == "Despesa")
                {
                    totalDespesas += item.Value;
                }
            }
        }

        if (!encontrou)
        {
            Console.WriteLine("Nenhuma transação encontrada para este período.");
            return;
        }

        decimal saldo = totalReceitas - totalDespesas;

        Console.WriteLine();
        Console.WriteLine("=== RESUMO DO PERÍODO ===");
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas:F2}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas:F2}");
        Console.WriteLine($"Saldo Final: R$ {saldo:F2}");
    }

    void RelatorioGeral()
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
            if (item.Type == "Receita")
            {
                totalReceitas += item.Value;
            }
            else if (item.Type == "Despesa")
            {
                totalDespesas += item.Value;
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
    }




}