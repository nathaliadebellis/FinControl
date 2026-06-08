using FinControl.Models;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;


List<Transacao> transacoes = new List<Transacao>();

bool executarSistema = true;

while (executarSistema)
{
    Console.WriteLine("Bem vindo ao FinControl!");
    Console.WriteLine("Seu sistema de gestão financeira pessoal");
    Console.WriteLine();
    Console.WriteLine("Para continuar, escolha uma opção:");
    Console.WriteLine("1 - Adicionar Transação");
    Console.WriteLine("2 - Listar Transações");
    Console.WriteLine("3 - Ver Saldo Atual");
    Console.WriteLine("4 - Editar Transação");
    Console.WriteLine("5 - Excluir Transação");
    Console.WriteLine("6 - Relatório Financeiro");
    Console.WriteLine("7 - Sair");
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
            EditarTransacao();
            break;


        case "5":
            ExcluirTransacao();
            break;

        case "6":
            RelatorioFinanceiro();
            break;

        case "7":
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

        Console.WriteLine("Digite a descrição:");
        transacao.Description = Console.ReadLine();

        Console.WriteLine("Digite a categoria:");
        transacao.Category = Console.ReadLine();

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
        decimal totalReceitas = 0;
        decimal totalDespesas = 0;
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
        Console.WriteLine($"Total de Receitas: R$ {totalReceitas}");
        Console.WriteLine($"Total de Despesas: R$ {totalDespesas}");
        Console.WriteLine($"Saldo Final: R$ {saldo}");
    }


}