using FinControl.Models;

namespace FinControl.Repositories.Interfaces;

public interface ITransacaoRepository
{
    List<Transacao> Carregar();

    void Salvar(List<Transacao> transacoes);
}