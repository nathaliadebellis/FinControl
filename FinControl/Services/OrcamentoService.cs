using FinControl.Models;
using System.Text.Json;

namespace FinControl.Services;

public static class OrcamentoService
{
    private static readonly string CaminhoArquivo = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Data",
        "orcamentos.json");

    /// <summary>
    /// Carrega os orçamentos salvos em arquivo JSON.
    /// </summary>
    public static List<OrcamentoCategoria> Carregar()
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(CaminhoArquivo)!);

        if (!File.Exists(CaminhoArquivo))
        {
            return new List<OrcamentoCategoria>();
        }

        string json = File.ReadAllText(CaminhoArquivo);

        return JsonSerializer.Deserialize<List<OrcamentoCategoria>>(json)
               ?? new List<OrcamentoCategoria>();
    }

    /// <summary>
    /// Salva os orçamentos em arquivo JSON.
    /// </summary>
    public static void Salvar(List<OrcamentoCategoria> orcamentos)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(CaminhoArquivo)!);

        string json = JsonSerializer.Serialize(
            orcamentos,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(CaminhoArquivo, json);
    }

    public static void DefinirOrcamento(List<OrcamentoCategoria> orcamentos)
    {
        Console.Clear();
        Console.WriteLine("===== DEFINIR ORÇAMENTO =====");
        Console.WriteLine();

        Console.WriteLine("Escolha a categoria:");

        for (int i = 0; i < Categorias.Lista.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {Categorias.Lista[i]}");
        }

        int opcao;

        while (!int.TryParse(Console.ReadLine(), out opcao) ||
               opcao < 1 ||
               opcao > Categorias.Lista.Count)
        {
            Console.WriteLine("Categoria inválida. Tente novamente.");
        }

        string categoria = Categorias.Lista[opcao - 1];

        decimal limite = 0;
        bool valido = false;

        while (!valido)
        {
            Console.Write($"\nDigite o limite mensal para {categoria}: ");

            valido = decimal.TryParse(Console.ReadLine(), out limite)
                     && limite > 0;

            if (!valido)
            {
                Console.WriteLine("Informe um valor maior que zero.");
            }
        }

        var existente = orcamentos
            .FirstOrDefault(o => o.Categoria == categoria);

        if (existente != null)
        {
            existente.LimiteMensal = limite;
        }
        else
        {
            orcamentos.Add(new OrcamentoCategoria
            {
                Categoria = categoria,
                LimiteMensal = limite
            });
        }

        Salvar(orcamentos);

        Console.WriteLine();
        Console.WriteLine("✅ Orçamento salvo com sucesso!");
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static void ListarOrcamentos(List<OrcamentoCategoria> orcamentos)
    {
        Console.Clear();

        Console.WriteLine("===== ORÇAMENTOS CADASTRADOS =====");
        Console.WriteLine();

        if (orcamentos.Count == 0)
        {
            Console.WriteLine("Nenhum orçamento cadastrado.");
        }
        else
        {
            foreach (var item in orcamentos)
            {
                Console.WriteLine(
                    $"{item.Categoria,-20} R$ {item.LimiteMensal:F2}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static void RemoverOrcamento(List<OrcamentoCategoria> orcamentos)
    {
        Console.Clear();

        if (orcamentos.Count == 0)
        {
            Console.WriteLine("Nenhum orçamento cadastrado.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Escolha o orçamento para remover:");

        for (int i = 0; i < orcamentos.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1} - {orcamentos[i].Categoria}");
        }

        int opcao;

        while (!int.TryParse(Console.ReadLine(), out opcao) ||
               opcao < 1 ||
               opcao > orcamentos.Count)
        {
            Console.WriteLine("Opção inválida.");
        }

        orcamentos.RemoveAt(opcao - 1);

        Salvar(orcamentos);

        Console.WriteLine();
        Console.WriteLine("Orçamento removido com sucesso!");
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}