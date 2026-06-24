using FinControl.Models;

namespace FinControl.Services;

public static class TransacaoService
{
    public static int ObterProximoId(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes.Count == 0
            ? 1
            : transacoes.Max(t => t.Id) + 1;
    }

    public static bool Validar(
        string descricao,
        string categoria,
        decimal valor,
        out string mensagemErro)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            mensagemErro = "A descrição é obrigatória.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(categoria))
        {
            mensagemErro = "A categoria é obrigatória.";
            return false;
        }

        if (valor <= 0)
        {
            mensagemErro = "O valor deve ser maior que zero.";
            return false;
        }

        mensagemErro = string.Empty;
        return true;
    }

    public static Transacao? Criar(
        List<Transacao> transacoes,
        string descricao,
        string categoria,
        TipoTransacao tipo,
        decimal valor,
        out string mensagemErro)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        descricao = descricao.Trim();
        categoria = categoria.Trim();

        if (!Validar(descricao, categoria, valor, out mensagemErro))
            return null;

        return new Transacao
        {
            Id = ObterProximoId(transacoes),
            Data = DateTime.Now,
            Descricao = descricao,
            Categoria = categoria,
            Tipo = tipo,
            Valor = valor
        };
    }

    public static bool Adicionar(
        List<Transacao> transacoes,
        Transacao? transacao)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        if (transacao is null)
            return false;

        transacoes.Add(transacao);
        return true;
    }

    public static List<Transacao> Listar(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .OrderByDescending(t => t.Data)
            .ThenByDescending(t => t.Id)
            .ToList();
    }

    public static Transacao? BuscarPorId(
        List<Transacao> transacoes,
        int id)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes.FirstOrDefault(t => t.Id == id);
    }

    public static List<Transacao> BuscarPorDescricao(
    List<Transacao> transacoes,
    string descricao)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Descricao.Contains(
                descricao.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static bool Editar(
        List<Transacao> transacoes,
        int id,
        string descricao,
        string categoria,
        TipoTransacao tipo,
        decimal valor,
        out string mensagemErro)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        descricao = descricao.Trim();
        categoria = categoria.Trim();

        var transacao = BuscarPorId(transacoes, id);

        if (transacao is null)
        {
            mensagemErro = "Transação não encontrada.";
            return false;
        }

        if (!Validar(descricao, categoria, valor, out mensagemErro))
            return false;

        transacao.Descricao = descricao;
        transacao.Categoria = categoria;
        transacao.Tipo = tipo;
        transacao.Valor = valor;

        mensagemErro = string.Empty;
        return true;
    }

    public static bool Excluir(
        List<Transacao> transacoes,
        int id)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        var transacao = BuscarPorId(transacoes, id);

        if (transacao is null)
            return false;

        return transacoes.Remove(transacao);
    }

    public static List<Transacao> BuscarPorCategoria(
        List<Transacao> transacoes,
        string categoria)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Categoria.Equals(
                categoria.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static List<Transacao> BuscarPorTipo(
        List<Transacao> transacoes,
        TipoTransacao tipo)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Tipo == tipo)
            .ToList();
    }
}