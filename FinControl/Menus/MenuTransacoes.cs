using FinControl.Models;
using FinControl.Services;
using FinControl.Repositories;

namespace FinControl.Menus;

public static class MenuTransacoes
{
    public static void Exibir(List<Transacao> transacoes)
    {
        bool continuar = true;

        while (continuar)
        {
            Formatting.ExibirCabecalho("TRANSAÇÕES");

            Console.WriteLine("1 - Adicionar");
            Console.WriteLine("2 - Listar");
            Console.WriteLine("3 - Buscar");
            Console.WriteLine("4 - Editar");
            Console.WriteLine("5 - Excluir");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            string opcao = ValidadorEntrada.LerOpcaoMenu(
                new[] { "1", "2", "3", "4", "5", "0" });

            Console.Clear();

            switch (opcao)
            {
                case "1":
                    Adicionar(transacoes);
                    Salvar(transacoes);
                    break;

                case "2":
                    Listar(transacoes);
                    break;

                case "3":
                    Buscar(transacoes);
                    break;

                case "4":
                    Editar(transacoes);
                    Salvar(transacoes);
                    break;

                case "5":
                    Excluir(transacoes);
                    Salvar(transacoes);
                    break;

                case "0":
                    continuar = false;
                    break;
            }
        }
    }

    // =========================
    // ADICIONAR
    // =========================
    private static void Adicionar(List<Transacao> transacoes)
    {
        string descricao =
            ValidadorEntrada.LerStringValida("Descrição:");

        int tipoOpcao = ValidadorEntrada.LerInteiro(
            "Tipo (1 = Receita, 2 = Despesa):",
            1,
            2);

        TipoTransacao tipo = tipoOpcao == 1
            ? TipoTransacao.Receita
            : TipoTransacao.Despesa;

        string categoria = SelecionarCategoria(tipo);

        decimal valor =
            ValidadorEntrada.LerDecimal("Valor:", 0.01m);

        Transacao? transacao = TransacaoService.Criar(
            transacoes,
            descricao,
            categoria,
            tipo,
            valor,
            out string erro);

        if (transacao is null)
        {
            ValidadorEntrada.MostrarErro(erro);
        }
        else
        {
            TransacaoService.Adicionar(transacoes, transacao);
            ValidadorEntrada.MostrarSucesso("Transação adicionada!");
        }

        Formatting.AguardarRetorno();
    }

    // =========================
    // LISTAR
    // =========================
    private static void Listar(List<Transacao> transacoes)
    {
        var lista = TransacaoService.Listar(transacoes);

        if (!lista.Any())
        {
            ValidadorEntrada.MostrarInfo("Nenhuma transação encontrada.");
        }
        else
        {
            foreach (var t in lista)
                Formatting.PrintTransacao(t);
        }

        Formatting.AguardarRetorno();
    }

    // =========================
    // BUSCAR
    // =========================
    private static void Buscar(List<Transacao> transacoes)
    {
        string termo =
            ValidadorEntrada.LerStringValida("Descrição:");

        var resultado =
            TransacaoService.BuscarPorDescricao(transacoes, termo);

        if (!resultado.Any())
        {
            ValidadorEntrada.MostrarInfo("Nenhuma transação encontrada.");
        }
        else
        {
            foreach (var t in resultado)
                Formatting.PrintTransacao(t);
        }

        Formatting.AguardarRetorno();
    }

    // =========================
    // EDITAR
    // =========================
    private static void Editar(List<Transacao> transacoes)
    {
        int id = ValidadorEntrada.LerInteiro("ID:");

        var existente = transacoes.FirstOrDefault(t => t.Id == id);

        if (existente is null)
        {
            ValidadorEntrada.MostrarErro("Transação não encontrada.");
            Formatting.AguardarRetorno();
            return;
        }

        string descricao =
            ValidadorEntrada.LerStringValida("Nova descrição:");

        int tipoOpcao = ValidadorEntrada.LerInteiro(
            "Tipo (1 = Receita, 2 = Despesa):",
            1,
            2);

        TipoTransacao tipo = tipoOpcao == 1
            ? TipoTransacao.Receita
            : TipoTransacao.Despesa;

        string categoria = SelecionarCategoria(tipo);

        decimal valor =
            ValidadorEntrada.LerDecimal("Novo valor:", 0.01m);

        bool atualizado = TransacaoService.Editar(
            transacoes,
            id,
            descricao,
            categoria,
            tipo,
            valor,
            out string erro);

        if (atualizado)
            ValidadorEntrada.MostrarSucesso("Atualizado com sucesso!");
        else
            ValidadorEntrada.MostrarErro(erro);

        Formatting.AguardarRetorno();
    }

    // =========================
    // EXCLUIR
    // =========================
    private static void Excluir(List<Transacao> transacoes)
    {
        int id = ValidadorEntrada.LerInteiro("ID:");

        var existente = transacoes.FirstOrDefault(t => t.Id == id);

        if (existente is null)
        {
            ValidadorEntrada.MostrarErro("Transação não encontrada.");
            Formatting.AguardarRetorno();
            return;
        }

        bool removido = TransacaoService.Excluir(transacoes, id);

        if (removido)
            ValidadorEntrada.MostrarSucesso("Removido com sucesso!");
        else
            ValidadorEntrada.MostrarErro("Erro ao remover.");

        Formatting.AguardarRetorno();
    }

    // =========================
    // CATEGORIA (REFATORADA)
    // =========================
    private static string SelecionarCategoria(TipoTransacao tipo)
    {
        var categorias = tipo == TipoTransacao.Receita
            ? Categorias.Receitas
            : Categorias.Despesas;

        Console.WriteLine();
        Console.WriteLine("Selecione uma categoria:");

        for (int i = 0; i < categorias.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {categorias[i]}");
        }

        int opcao = ValidadorEntrada.LerInteiro(
            "Opção:",
            1,
            categorias.Count);

        return categorias[opcao - 1];
    }

    // =========================
    // SALVAR CENTRALIZADO
    // =========================
    private static void Salvar(List<Transacao> transacoes)
    {
        TransacaoRepository.Salvar(transacoes);
    }
}