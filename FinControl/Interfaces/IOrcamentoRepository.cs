using FinControl.Models;

namespace FinControl.Interfaces;

public interface IOrcamentoRepository
{
    List<OrcamentoCategoria> Carregar();

    void Salvar(List<OrcamentoCategoria> orcamentos);
}