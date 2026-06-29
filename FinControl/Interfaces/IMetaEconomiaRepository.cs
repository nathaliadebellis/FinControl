using FinControl.Models;

namespace FinControl.Interfaces;

public interface IMetaEconomiaRepository
{
    MetaEconomia Carregar();

    void Salvar(MetaEconomia meta);

    void Remover();
}