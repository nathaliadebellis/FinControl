using System;

namespace FinControl.Services;

/// <summary>
/// Métodos auxiliares para validação e leitura da entrada do usuário no console.
/// </summary>
public static class ValidadorEntrada
{

    /// <summary>
    /// Valida e retorna uma string não vazia
    /// </summary>
    public static string LerStringValida(string mensagem)
    {
        while (true)
        {
            Console.WriteLine(mensagem);
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Campo obrigatório! Não pode estar vazio.");
                Console.ResetColor();
                continue;
            }

            return entrada.Trim();
        }
    }

    /// <summary>
    /// Valida e retorna um inteiro dentro de um intervalo
    /// </summary>
    public static int LerInteiro(string mensagem, int minimo = int.MinValue, int maximo = int.MaxValue)
    {
        while (true)
        {
            Console.WriteLine(mensagem);
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Campo obrigatório! Digite um número.");
                Console.ResetColor();
                continue;
            }

            if (!int.TryParse(entrada, out int valor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Entrada inválida! Digite um número inteiro.");
                Console.ResetColor();
                continue;
            }

            if (valor < minimo || valor > maximo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Valor fora do intervalo permitido ({minimo} a {maximo}).");
                Console.ResetColor();
                continue;
            }

            return valor;
        }
    }

    /// <summary>
    /// Valida e retorna um decimal (valor monetário)
    /// </summary>
    public static decimal LerDecimal(string mensagem, decimal minimo = 0, decimal maximo = decimal.MaxValue)
    {
        while (true)
        {
            Console.WriteLine(mensagem);
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Campo obrigatório! Digite um valor.");
                Console.ResetColor();
                continue;
            }

            if (!decimal.TryParse(entrada, out decimal valor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Valor inválido! Use ponto (.) ou vírgula (,) como separador decimal.");
                Console.ResetColor();
                continue;
            }

            if (valor < minimo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"O valor não pode ser menor que R$ {minimo}.");
                Console.ResetColor();
                continue;
            }

            if (valor > maximo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"O valor excede o limite permitido.");
                Console.ResetColor();
                continue;
            }

            return valor;
        }
    }

    /// <summary>
    /// Valida uma opção de menu
    /// </summary>
    public static string LerOpcaoMenu(string[] opcoes)
    {
        while (true)
        {
            Console.WriteLine("Digite a opção desejada:");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Digite uma opção válida.");
                Console.ResetColor();
                continue;
            }

            //Se existe o valor digitado dentro do array de opções, retorna ele
            if (Array.Exists(opcoes, element => element == entrada))
            {
                return entrada;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Opção inválida! Escolha entre: {string.Join(", ", opcoes)}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Exibe uma mensagem de sucesso
    /// </summary>
    public static void MostrarSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }

    /// <summary>
    /// Exibe uma mensagem de erro
    /// </summary>
    public static void MostrarErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }

    /// <summary>
    /// Exibe uma mensagem de informação
    /// </summary>
    public static void MostrarInfo(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }
}