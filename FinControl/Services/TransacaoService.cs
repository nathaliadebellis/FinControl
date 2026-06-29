using FinControl.Models;
using FinControl.Repositories.Interfaces;

namespace FinControl.Services;

public class TransacaoService
{
    private readonly ITransacaoRepository _repository;

    public TransacaoService(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public int ObterProximoId(List<Transacao> transacoes)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes.Count == 0
            ? 1
            : transacoes.Max(t => t.Id) + 1;
    }

    public bool Validar(
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

    public Transacao? Criar(
    string descricao,
    string categoria,
    TipoTransacao tipo,
    decimal valor,
    out string mensagemErro)
    {
        var transacoes = _repository.Carregar();

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

    public bool Adicionar(Transacao transacao)
    {
        var transacoes = _repository.Carregar();
        ArgumentNullException.ThrowIfNull(transacoes);

        if (transacao is null)
            return false;

        transacoes.Add(transacao);
        _repository.Salvar(transacoes);
        return true;
    }

    public List<Transacao> Listar()
    {
        return _repository
            .Carregar()
            .OrderByDescending(t => t.Data)
            .ThenByDescending(t => t.Id)
            .ToList();
    }

    public Transacao? BuscarPorId(int id)
    {
        return _repository
            .Carregar()
            .FirstOrDefault(t => t.Id == id);
    }

    public List<Transacao> BuscarPorDescricao(string descricao)
    {
        return _repository
            .Carregar()
            .Where(t => t.Descricao.Contains(
                descricao.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public bool Editar(
        int id,
        string descricao,
        string categoria,
        TipoTransacao tipo,
        decimal valor,
        out string mensagemErro)
    {
        var transacoes = _repository.Carregar();
        ArgumentNullException.ThrowIfNull(transacoes);

        descricao = descricao.Trim();
        categoria = categoria.Trim();

        var transacao = BuscarPorId(id);

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
        _repository.Salvar(transacoes);

        mensagemErro = string.Empty;
        return true;
    }

public bool Excluir(int id)
{
    var transacoes = _repository.Carregar();

    var transacao = transacoes.FirstOrDefault(t => t.Id == id);

    if (transacao is null)
        return false;

    transacoes.Remove(transacao);

    _repository.Salvar(transacoes);

    return true;
}

    public List<Transacao> BuscarPorCategoria(
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

    public List<Transacao> BuscarPorTipo(
        List<Transacao> transacoes,
        TipoTransacao tipo)
    {
        ArgumentNullException.ThrowIfNull(transacoes);

        return transacoes
            .Where(t => t.Tipo == tipo)
            .ToList();
    }
}