using FinControl.Models;
using FinControl.Services;
using FinControl.Repositories;
using FinControl.Utils;
namespace FinControl.Menus;

public class MenuTransacoes
{
    private readonly TransacaoService _service;

    public MenuTransacoes(TransacaoService service)
    {
        _service = service;
    }
    public void Exibir()
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
                    Adicionar();
                    break;

                case "2":
                    Listar();
                    break;

                case "3":
                    Buscar();
                    break;

                case "4":
                    Editar();
                    break;

                case "5":
                    Excluir();
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
    private void Adicionar()
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

        Transacao? transacao = _service.Criar(
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
            _service.Adicionar(transacao);
            ValidadorEntrada.MostrarSucesso("Transação adicionada!");
        }

        Formatting.AguardarRetorno();
    }

    // =========================
    // LISTAR
    // =========================
    private void Listar()
    {
        var lista = _service.Listar();

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
    private void Buscar()
    {
        string termo =
            ValidadorEntrada.LerStringValida("Descrição:");

        var resultado =
            _service.BuscarPorDescricao(termo);

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
    private void Editar()
    {
        int id = ValidadorEntrada.LerInteiro("ID:");

        var existente =
    _service.BuscarPorId(id);

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

        bool atualizado = _service.Editar(
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
    private void Excluir()
    {
        int id = ValidadorEntrada.LerInteiro("ID:");

        var existente =
    _service.BuscarPorId(id);

        if (existente is null)
        {
            ValidadorEntrada.MostrarErro("Transação não encontrada.");
            Formatting.AguardarRetorno();
            return;
        }

        bool removido = _service.Excluir(id);

        if (removido)
            ValidadorEntrada.MostrarSucesso("Removido com sucesso!");
        else
            ValidadorEntrada.MostrarErro("Erro ao remover.");

        Formatting.AguardarRetorno();
    }

    // =========================
    // CATEGORIA (REFATORADA)
    // =========================
    private string SelecionarCategoria(TipoTransacao tipo)
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

}