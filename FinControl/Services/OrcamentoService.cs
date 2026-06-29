using FinControl.Interfaces;
using FinControl.Models;

namespace FinControl.Services;

public class OrcamentoService
{
    private readonly IOrcamentoRepository _repository;
    private readonly TransacaoService _transacaoService;

    public OrcamentoService(
    IOrcamentoRepository repository,
    TransacaoService transacaoService)
    {
        _repository = repository;
        _transacaoService = transacaoService;
    }

    private OrcamentoCategoria? BuscarCategoria(
    List<OrcamentoCategoria> orcamentos,
    string categoria)
    {
        return orcamentos.FirstOrDefault(o =>
            o.Categoria.Equals(
                categoria,
                StringComparison.OrdinalIgnoreCase));
    }
    public List<OrcamentoCategoria> ListarOrcamentos()
    {
        return _repository
            .Carregar()
            .OrderBy(o => o.Categoria)
            .ToList();
    }

    public void DefinirOuAtualizarOrcamento(
        string categoria,
        decimal limite)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("A categoria é obrigatória.");

        if (limite <= 0)
            throw new ArgumentException("O limite mensal deve ser maior que zero.");

        var orcamentos = _repository.Carregar();

        var existente = BuscarCategoria(
    orcamentos,
    categoria);

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

        _repository.Salvar(orcamentos);
    }

    public bool RemoverOrcamento(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("A categoria é obrigatória.");

        var orcamentos = _repository.Carregar();

        var orcamento = BuscarCategoria(
    orcamentos,
    categoria);

        if (orcamento == null)
            return false;

        orcamentos.Remove(orcamento);

        _repository.Salvar(orcamentos);

        return true;
    }

    public List<string> VerificarAlertas()
    {
        var transacoes = _transacaoService.Listar();

        var orcamentos = _repository.Carregar();

        List<string> alertas = new();

        foreach (var orcamento in orcamentos)
        {
            decimal gasto = transacoes
                .Where(t =>
                    t.Tipo == TipoTransacao.Despesa &&
                    t.Categoria == orcamento.Categoria)
                .Sum(t => t.Valor);

            decimal percentual = orcamento.LimiteMensal == 0
                ? 0
                : gasto / orcamento.LimiteMensal;

            if (percentual >= 1)
            {
                alertas.Add(
                    $"Orçamento de {orcamento.Categoria} excedido: " +
                    $"R$ {gasto:F2} de R$ {orcamento.LimiteMensal:F2}.");
            }
            else if (percentual >= 0.8m)
            {
                alertas.Add(
                    $"{orcamento.Categoria}: " +
                    $"{percentual:P0} do limite utilizado " +
                    $"(R$ {gasto:F2} de R$ {orcamento.LimiteMensal:F2}).");
            }
        }

        return alertas;
    }

    public OrcamentoCategoria? ObterPorCategoria(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("A categoria é obrigatória.");

        var orcamentos = _repository.Carregar();

        return orcamentos.FirstOrDefault(o =>
            o.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
    }
}