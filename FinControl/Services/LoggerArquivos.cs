using System;
using System.IO;
using System.Text;

namespace FinControl.Services;

/// <summary>
/// Gerencia logging de erros em arquivo
/// </summary>
public class LoggerArquivos
{
    private readonly string _caminhoLog;
    private readonly object _lock = new object();
    private const int MaxTamanhoDB = 5_000_000; // 5 MB

    public LoggerArquivos(string? caminhoPersonalizado = null)
    {
        _caminhoLog = caminhoPersonalizado ?? Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Logs",
            "erros.log");
    }

    /// <summary>
    /// Registra um erro no arquivo de log
    /// </summary>
    public void RegistrarErro(Exception excecao, string contexto = "")
    {
        try
        {
            lock (_lock)
            {
                // Verifica se precisa fazer rotação do arquivo
                VerificarRotacaoArquivo();

                // Cria diretório se não existir
                string? diretorio = Path.GetDirectoryName(_caminhoLog);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio!);
                }

                // Formata a mensagem de erro
                string mensagem = FormatarErro(excecao, contexto);

                // Escreve no arquivo
                File.AppendAllText(_caminhoLog, mensagem, Encoding.UTF8);
            }
        }
        catch (Exception ex)
        {
            // Se não conseguir escrever no arquivo, pelo menos exibe no console
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Falha ao registrar erro no log: {ex.Message}");
            Console.WriteLine($"Erro original: {excecao.Message}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Registra uma informação no log
    /// </summary>
    public void RegistrarInfo(string mensagem)
    {
        try
        {
            lock (_lock)
            {
                VerificarRotacaoArquivo();

                string? diretorio = Path.GetDirectoryName(_caminhoLog);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio!);
                }

                string registro = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [INFO] {mensagem}{Environment.NewLine}";
                File.AppendAllText(_caminhoLog, registro, Encoding.UTF8);
            }
        }
        catch
        {
            // Silenciosamente falha se não conseguir logar info
        }
    }

    /// <summary>
    /// Registra um aviso no log
    /// </summary>
    public void RegistrarAviso(string mensagem)
    {
        try
        {
            lock (_lock)
            {
                VerificarRotacaoArquivo();

                string? diretorio = Path.GetDirectoryName(_caminhoLog);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio!);
                }

                string registro = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [AVISO] {mensagem}{Environment.NewLine}";
                File.AppendAllText(_caminhoLog, registro, Encoding.UTF8);
            }
        }
        catch
        {
            // Silenciosamente falha se não conseguir logar aviso
        }
    }

    /// <summary>
    /// Obtém os últimos N erros do log
    /// </summary>
    public string? ObterUltimosErros(int quantidade = 10)
    {
        try
        {
            lock (_lock)
            {
                if (!File.Exists(_caminhoLog))
                    return null;

                string[] linhas = File.ReadAllLines(_caminhoLog);
                int inicio = Math.Max(0, linhas.Length - (quantidade * 5)); // Aproximadamente 5 linhas por erro

                StringBuilder sb = new StringBuilder();
                for (int i = inicio; i < linhas.Length; i++)
                {
                    sb.AppendLine(linhas[i]);
                }

                return sb.ToString();
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Limpa o arquivo de log
    /// </summary>
    public void LimparLog()
    {
        try
        {
            lock (_lock)
            {
                if (File.Exists(_caminhoLog))
                {
                    File.Delete(_caminhoLog);
                }
            }
        }
        catch
        {
            // Silenciosamente falha se não conseguir limpar
        }
    }

    /// <summary>
    /// Obtém informações sobre o arquivo de log
    /// </summary>
    public (long Tamanho, int Linhas) ObterInfoLog()
    {
        try
        {
            lock (_lock)
            {
                if (!File.Exists(_caminhoLog))
                    return (0, 0);

                FileInfo info = new FileInfo(_caminhoLog);
                int linhas = File.ReadAllLines(_caminhoLog).Length;

                return (info.Length, linhas);
            }
        }
        catch
        {
            return (0, 0);
        }
    }

    // ============ MÉTODOS PRIVADOS ============

    private string FormatarErro(Exception excecao, string contexto)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("╔════════════════════════════════════════════════════════════════╗");
        sb.AppendLine($"║ ERRO: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        sb.AppendLine("╚════════════════════════════════════════════════════════════════╝");

        if (!string.IsNullOrWhiteSpace(contexto))
        {
            sb.AppendLine($"Contexto: {contexto}");
        }

        sb.AppendLine($"Tipo: {excecao.GetType().Name}");
        sb.AppendLine($"Mensagem: {excecao.Message}");
        sb.AppendLine($"Stack Trace:{Environment.NewLine}{excecao.StackTrace}");

        // Exceção interna
        if (excecao.InnerException != null)
        {
            sb.AppendLine($"Exceção Interna: {excecao.InnerException.GetType().Name}");
            sb.AppendLine($"Mensagem Interna: {excecao.InnerException.Message}");
        }

        // Informações adicionais da exceção customizada
        if (excecao is ExcecaoFinControl excecaoCustom)
        {
            sb.AppendLine($"Código: {excecaoCustom.Codigo}");
            sb.AppendLine($"Data de Ocorrência: {excecaoCustom.DataOcorrencia:yyyy-MM-dd HH:mm:ss}");

            if (excecao is ExcecaoPersistenciaDados excecaoPersist)
            {
                sb.AppendLine($"Caminho Arquivo: {excecaoPersist.CaminhoArquivo}");
            }
            else if (excecao is ExcecaoOperacaoTransacao excecaoTrans)
            {
                sb.AppendLine($"ID Transação: {excecaoTrans.IdTransacao}");
                sb.AppendLine($"Operação: {excecaoTrans.Operacao}");
            }
        }

        sb.AppendLine();
        return sb.ToString();
    }

    private void VerificarRotacaoArquivo()
    {
        try
        {
            if (!File.Exists(_caminhoLog))
                return;

            FileInfo info = new FileInfo(_caminhoLog);
            if (info.Length > MaxTamanhoDB)
            {
                // Renomeia o arquivo atual
                string nomeArquivoAntigo = Path.Combine(
                    Path.GetDirectoryName(_caminhoLog)!,
                    $"erros_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");

                File.Move(_caminhoLog, nomeArquivoAntigo, true);
            }
        }
        catch
        {
            // Silenciosamente ignora erros na rotação
        }
    }
}
