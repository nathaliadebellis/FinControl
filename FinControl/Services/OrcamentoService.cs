using FinControl.Models;
using FinControl.Repositories;

namespace FinControl.Services;

public static class OrcamentoService
{
    public static List<OrcamentoCategoria> ListarOrcamentos()
    {
        return OrcamentoRepository
            .Carregar()
            .OrderBy(o => o.Categoria)
            .ToList();
    }

    public static void DefinirOuAtualizarOrcamento(
        string categoria,
        decimal limite)
    {
        var orcamentos = OrcamentoRepository.Carregar();

        var existente = orcamentos.FirstOrDefault(o =>
            o.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("A categoria é obrigatória.");

        if (limite <= 0)
            throw new ArgumentException("O limite mensal deve ser maior que zero.");

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

        OrcamentoRepository.Salvar(orcamentos);
    }

    public static bool RemoverOrcamento(string categoria)
    {
        var orcamentos = OrcamentoRepository.Carregar();

        var orcamento = orcamentos.FirstOrDefault(o =>
            o.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

        if (orcamento == null)
            return false;

        orcamentos.Remove(orcamento);
        OrcamentoRepository.Salvar(orcamentos);

        return true;
    }

    public static List<string> VerificarAlertas(
        List<Transacao> transacoes)
    {
        var orcamentos = OrcamentoRepository.Carregar();
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
                alertas.Add($"Orçamento de {orcamento.Categoria} excedido: " +
                    $"R$ {gasto:F2} de R$ {orcamento.LimiteMensal:F2}.");

            else if (percentual >= 0.8m)
                alertas.Add($"{orcamento.Categoria}: " +
                    $"{percentual:P0} do limite utilizado " +
                    $"(R$ {gasto:F2} de R$ {orcamento.LimiteMensal:F2}).");
        }

        return alertas;
    }

    public static OrcamentoCategoria? ObterPorCategoria(string categoria)
    {
        var orcamentos = OrcamentoRepository.Carregar();

        return orcamentos.FirstOrDefault(o =>
    o.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
    }
}