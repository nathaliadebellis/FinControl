using System;

namespace FinControl.Services;

/// <summary>
/// Exceção base para operações do FinControl
/// </summary>
public class ExcecaoFinControl : Exception
{
    public string? Codigo { get; set; }
    public DateTime DataOcorrencia { get; set; }

    public ExcecaoFinControl(string mensagem, string? codigo = null) 
        : base(mensagem)
    {
        Codigo = codigo;
        DataOcorrencia = DateTime.Now;
    }

    public ExcecaoFinControl(string mensagem, Exception? excecaoInterna, string? codigo = null) 
        : base(mensagem, excecaoInterna)
    {
        Codigo = codigo;
        DataOcorrencia = DateTime.Now;
    }
}

/// <summary>
/// Exceção para erros de persistência de dados (leitura/escrita de arquivos)
/// </summary>
public class ExcecaoPersistenciaDados : ExcecaoFinControl
{
    public string? CaminhoArquivo { get; set; }

    public ExcecaoPersistenciaDados(string mensagem, string? caminhoArquivo = null, Exception? excecaoInterna = null)
        : base(mensagem, excecaoInterna, "ERRO_PERSISTENCIA")
    {
        CaminhoArquivo = caminhoArquivo;
    }
}

/// <summary>
/// Exceção para erros de operações com transações
/// </summary>
public class ExcecaoOperacaoTransacao : ExcecaoFinControl
{
    public int? IdTransacao { get; set; }
    public string? Operacao { get; set; }

    public ExcecaoOperacaoTransacao(string mensagem, int? idTransacao = null, string? operacao = null, Exception? excecaoInterna = null)
        : base(mensagem, excecaoInterna, "ERRO_TRANSACAO")
    {
        IdTransacao = idTransacao;
        Operacao = operacao;
    }
}

/// <summary>
/// Exceção para erros de validação de dados
/// </summary>
public class ExcecaoValidacao : ExcecaoFinControl
{
    public string? Campo { get; set; }
    public object? ValorInvalido { get; set; }

    public ExcecaoValidacao(string mensagem, string? campo = null, object? valorInvalido = null)
        : base(mensagem, "ERRO_VALIDACAO")
    {
        Campo = campo;
        ValorInvalido = valorInvalido;
    }
}

/// <summary>
/// Exceção para erros de transação não encontrada
/// </summary>
public class ExcecaoTransacaoNaoEncontrada : ExcecaoFinControl
{
    public int? IdTransacao { get; set; }
    public string? DescricaoTransacao { get; set; }

    public ExcecaoTransacaoNaoEncontrada(string mensagem, int? idTransacao = null, string? descricao = null)
        : base(mensagem, "TRANSACAO_NAO_ENCONTRADA")
    {
        IdTransacao = idTransacao;
        DescricaoTransacao = descricao;
    }
}

/// <summary>
/// Exceção para erros de logging
/// </summary>
public class ExcecaoLogger : ExcecaoFinControl
{
    public ExcecaoLogger(string mensagem, Exception? excecaoInterna = null)
        : base(mensagem, excecaoInterna, "ERRO_LOGGER")
    {
    }
}

/// <summary>
/// Exceção para erros não recuperáveis
/// </summary>
public class ExcecaoFatal : ExcecaoFinControl
{
    public ExcecaoFatal(string mensagem, Exception? excecaoInterna = null)
        : base(mensagem, excecaoInterna, "ERRO_FATAL")
    {
    }
}
