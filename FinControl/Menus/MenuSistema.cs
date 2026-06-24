using FinControl.Services;

namespace FinControl.Menus;

public static class MenuSistema
{
    public static void Exibir()
    {
        string caminhoArquivo = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "Data",
    "transacoes.json");

        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("SISTEMA");

            Console.WriteLine("1 - Criar backup");
            Console.WriteLine("2 - Informações dos backups");
            Console.WriteLine("3 - Restaurar backup");
            Console.WriteLine("4 - Limpar backups antigos");
            Console.WriteLine("5 - Excluir todos os backups");
            Console.WriteLine("6 - Relatório de erros");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "4", "5", "6", "0" });

            Console.Clear();

            switch (opcao)
            {
                case "1":
                    {
                        bool sucesso = GerenciadorErros.CriarBackup(
                            caminhoArquivo,
                            out string backup);

                        if (sucesso)
                        {
                            ValidadorEntrada.MostrarSucesso(
                                $"Backup criado:\n{backup}");
                        }
                        else
                        {
                            ValidadorEntrada.MostrarErro(
                                "Não foi possível criar o backup.");
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "2":
                    {
                        var info = GerenciadorErros.ObterBackups(caminhoArquivo);

                        Console.WriteLine("===== BACKUPS =====");
                        Console.WriteLine($"Quantidade: {info.Quantidade}");
                        Console.WriteLine($"Espaço utilizado: {info.TamanhoTotalBytes / 1024.0:F2} KB");

                        if (info.UltimoBackup.HasValue)
                        {
                            Console.WriteLine(
                                $"Último backup: {info.UltimoBackup:dd/MM/yyyy HH:mm:ss}");
                        }
                        else
                        {
                            Console.WriteLine("Nenhum backup encontrado.");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Arquivos:");

                        foreach (var backup in info.Backups)
                        {
                            Console.WriteLine(
                                $"- {backup.Name}");
                            Console.WriteLine(
                                $"  Data: {backup.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
                            Console.WriteLine(
                                $"  Tamanho: {backup.Length / 1024.0:F2} KB");
                            Console.WriteLine();
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "3":
                    {
                        var info = GerenciadorErros.ObterBackups(caminhoArquivo);

                        if (!info.Backups.Any())
                        {
                            ValidadorEntrada.MostrarInfo("Nenhum backup encontrado.");
                            Formatting.AguardarRetorno();
                            break;
                        }

                        Console.WriteLine("Selecione o backup:");

                        for (int i = 0; i < info.Backups.Count; i++)
                        {
                            Console.WriteLine(
                                $"{i + 1} - {info.Backups[i].Name} ({info.Backups[i].LastWriteTime:dd/MM/yyyy HH:mm:ss})");
                        }

                        int opcaoBackup = ValidadorEntrada.LerInteiro(
                            "Opção:",
                            1,
                            info.Backups.Count);

                        bool sucesso = GerenciadorErros.RestaurarBackup(
                            info.Backups[opcaoBackup - 1].FullName,
                            caminhoArquivo);

                        if (sucesso)
                        {
                            ValidadorEntrada.MostrarSucesso("Backup restaurado com sucesso!");
                        }
                        else
                        {
                            ValidadorEntrada.MostrarErro("Falha ao restaurar o backup.");
                        }

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "4":
                    {
                        int dias = ValidadorEntrada.LerInteiro(
                            "Remover backups com mais de quantos dias?",
                            1,
                            365);

                        GerenciadorErros.LimparBackupsAntigos(
                            caminhoArquivo,
                            dias);

                        ValidadorEntrada.MostrarInfo(
                            "Limpeza de backups concluída.");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "5":
                    {
                        Console.Write("Deseja realmente excluir todos os backups? (S/N): ");

                        string resposta = Console.ReadLine()?.Trim().ToUpper() ?? "N";

                        if (resposta != "S")
                        {
                            ValidadorEntrada.MostrarInfo("Operação cancelada.");
                            Formatting.AguardarRetorno();
                            break;
                        }

                        bool sucesso = GerenciadorErros.ExcluirTodosBackups(
                            caminhoArquivo);

                        if (sucesso)
                            ValidadorEntrada.MostrarSucesso(
                                "Todos os backups foram removidos.");
                        else
                            ValidadorEntrada.MostrarErro(
                                "Nenhum backup encontrado.");

                        Formatting.AguardarRetorno();
                        break;
                    }

                case "6":
                    {
                        GerenciadorErros.ExibirRelatorioDErros();
                        break;
                    }

                case "0":
                    continuar = false;
                    break;
            }
        }
    }
}