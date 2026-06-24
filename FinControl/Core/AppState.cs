using FinControl.Models;
using FinControl.Repositories;

namespace FinControl.Core;

public static class AppState
{
    public static List<Transacao> Transacoes { get; private set; } = new();

    public static void Inicializar()
    {
        Transacoes = TransacaoRepository.Carregar();
    }

    public static void AdicionarTransacao(Transacao transacao)
    {
        Transacoes.Add(transacao);
        TransacaoRepository.Salvar(Transacoes);
    }

    public static void RemoverTransacao(Transacao transacao)
    {
        Transacoes.Remove(transacao);
        TransacaoRepository.Salvar(Transacoes);
    }

    public static void Salvar()
    {
        TransacaoRepository.Salvar(Transacoes);
    }
}