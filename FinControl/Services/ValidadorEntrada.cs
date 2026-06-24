using System;

namespace FinControl.Services;

/// <summary>
/// Métodos auxiliares para validação e leitura da entrada do usuário no console.
/// </summary>
public static class ValidadorEntrada
{
    public static string LerStringValida(string mensagem)
    {
        while (true)
        {
            Console.Write($"{mensagem} ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarErro("Campo obrigatório! Não pode estar vazio.");
                continue;
            }

            return entrada.Trim();
        }
    }

    public static int LerInteiro(
        string mensagem,
        int minimo = int.MinValue,
        int maximo = int.MaxValue)
    {
        while (true)
        {
            Console.Write($"{mensagem} ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarErro("Campo obrigatório! Digite um número.");
                continue;
            }

            if (!int.TryParse(entrada, out int valor))
            {
                MostrarErro("Entrada inválida! Digite um número inteiro.");
                continue;
            }

            if (valor < minimo || valor > maximo)
            {
                MostrarErro($"Valor fora do intervalo permitido ({minimo} a {maximo}).");
                continue;
            }

            return valor;
        }
    }

    public static decimal LerDecimal(
        string mensagem,
        decimal minimo = 0,
        decimal maximo = decimal.MaxValue)
    {
        while (true)
        {
            Console.Write($"{mensagem} ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarErro("Campo obrigatório! Digite um valor.");
                continue;
            }

            if (!decimal.TryParse(entrada, out decimal valor))
            {
                MostrarErro("Valor inválido! Use vírgula ou ponto como separador decimal.");
                continue;
            }

            if (valor < minimo)
            {
                MostrarErro($"O valor não pode ser menor que R$ {minimo}.");
                continue;
            }

            if (valor > maximo)
            {
                MostrarErro("O valor excede o limite permitido.");
                continue;
            }

            return valor;
        }
    }

    public static string LerOpcaoMenu(string[] opcoes)
    {
        while (true)
        {
            Console.Write("Digite a opção desejada: ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarErro("Digite uma opção válida.");
                continue;
            }

            if (Array.Exists(opcoes, o => o == entrada))
            {
                return entrada;
            }

            MostrarErro($"Opção inválida! Escolha entre: {string.Join(", ", opcoes)}");
        }
    }

    public static void MostrarSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ {mensagem}");
        Console.ResetColor();
    }

    public static void MostrarErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✖ {mensagem}");
        Console.ResetColor();
    }

    public static void MostrarInfo(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"ℹ {mensagem}");
        Console.ResetColor();
    }
}