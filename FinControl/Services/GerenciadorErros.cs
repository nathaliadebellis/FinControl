using FinControl.Models;
using System;
using System.Text.Json;

namespace FinControl.Services;

/// <summary>
/// Gerenciador centralizado de erros e recuperação
/// </summary>
/// <summary>
/// Utilitários centralizados de gerenciamento de erros que encapsulam operações com tratamento de erros, backups e logging.
/// </summary>
public static class GerenciadorErros
{
    private static LoggerArquivos? _logger;
    private static bool _loggerInicializado = false;

    public static void Inicializar(string? caminhoLogPersonalizado = null)
    {
        if (!_loggerInicializado)
        {
            _logger = new LoggerArquivos(caminhoLogPersonalizado);
            _loggerInicializado = true;
            _logger.RegistrarInfo("Gerenciador de Erros Inicializado");
        }
    }

    /// <summary>
    /// Trata exceções de persistência com retry automático
    /// </summary>
    public static bool TratarExcecaoPersistencia(
        Exception excecao,
        string caminhoArquivo,
        string operacao,
        int tentativas = 3)
    {
        _logger?.RegistrarErro(excecao, $"Operação: {operacao} | Arquivo: {caminhoArquivo}");

        if (excecao is not ExcecaoPersistenciaDados)
        {
            if (excecao is JsonException jsonEx)
            {
                _logger?.RegistrarErro(
                    new ExcecaoPersistenciaDados(
                        $"Erro ao desserializar JSON: {jsonEx.Message}",
                        caminhoArquivo,
                        jsonEx),
                    $"Arquivo corrompido: {caminhoArquivo}");

                ExibirErro($"Arquivo de dados corrompido: {Path.GetFileName(caminhoArquivo)}\nTentando recuperar...");
                return false;
            }
            else if (excecao is UnauthorizedAccessException uaaEx)
            {
                _logger?.RegistrarErro(
                    new ExcecaoPersistenciaDados(
                        $"Acesso negado ao arquivo: {caminhoArquivo}",
                        caminhoArquivo,
                        uaaEx),
                    "Acesso negado");

                ExibirErro($"Erro de permissão: Sem acesso de leitura/escrita em {Path.GetFileName(caminhoArquivo)}");
                return false;
            }
            else if (excecao is DirectoryNotFoundException dnfEx)
            {
                _logger?.RegistrarErro(
                    new ExcecaoPersistenciaDados(
                        $"Diretório não encontrado: {Path.GetDirectoryName(caminhoArquivo)}",
                        caminhoArquivo,
                        dnfEx),
                    "Diretório não encontrado");

                ExibirAviso($"Diretório será criado automaticamente.");
                return true;
            }
            else if (excecao is IOException ioEx)
            {
                // Pode ser problema transitório
                _logger?.RegistrarAviso($"Erro de I/O (tentativa com retry): {ioEx.Message}");
                return tentativas > 0;
            }
        }

        ExibirErro($"Erro de persistência: {excecao.Message}");
        return false;
    }

    /// <summary>
    /// Trata exceções de operação com transações
    /// </summary>
    public static void TratarExcecaoTransacao(
        Exception excecao,
        string operacao,
        int? idTransacao = null)
    {
        _logger?.RegistrarErro(
            excecao,
            $"Operação: {operacao} | ID Transação: {idTransacao ?? -1}");

        if (excecao is ExcecaoTransacaoNaoEncontrada)
        {
            ExibirInfo("Transação não encontrada com os critérios informados.");
        }
        else if (excecao is ExcecaoValidacao excecaoVal)
        {
            ExibirErro($"Validação falhou - Campo: {excecaoVal.Campo}\nMensagem: {excecao.Message}");
        }
        else
        {
            ExibirErro($"Erro ao {operacao}: {excecao.Message}");
        }
    }

    /// <summary>
    /// Trata exceções durante carregamento de dados com fallback
    /// </summary>
    public static T? TratarCarregamentoDados<T>(
        Func<T> funcaoCarregar,
        string descricao,
        T? valorPadrao = null) where T : class
    {
        int tentativas = 3;
        int delayMs = 100;

        for (int i = 0; i < tentativas; i++)
        {
            try
            {
                return funcaoCarregar();
            }
            catch (JsonException jsonEx)
            {
                _logger?.RegistrarErro(jsonEx, $"Desserialização de {descricao} - Tentativa {i + 1}/{tentativas}");

                if (i == tentativas - 1)
                {
                    ExibirAviso($"Não foi possível carregar {descricao}. Usando valores padrão.");
                    return valorPadrao;
                }

                System.Threading.Thread.Sleep(delayMs);
                delayMs *= 2;
            }
            catch (Exception ex)
            {
                _logger?.RegistrarErro(ex, $"Carregamento de {descricao} - Tentativa {i + 1}/{tentativas}");

                if (i == tentativas - 1)
                {
                    ExibirErro($"Erro ao carregar {descricao}: {ex.Message}");
                    return valorPadrao;
                }

                System.Threading.Thread.Sleep(delayMs);
                delayMs *= 2;
            }
        }

        return valorPadrao;
    }

    /// <summary>
    /// Trata exceções durante salvamento com tentativas
    /// </summary>
    public static bool TratarSalvamentoDados(
        Action funcaoSalvar,
        string descricao)
    {
        int tentativas = 3;
        int delayMs = 100;

        for (int i = 0; i < tentativas; i++)
        {
            try
            {
                funcaoSalvar();
                _logger?.RegistrarInfo($" {descricao} salvo com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                _logger?.RegistrarAviso($"Tentativa {i + 1}/{tentativas} falhou ao salvar {descricao}: {ex.Message}");

                if (i == tentativas - 1)
                {
                    _logger?.RegistrarErro(ex, $"Falha final ao salvar {descricao}");
                    ExibirErro($"Erro ao salvar {descricao} após {tentativas} tentativas: {ex.Message}");
                    return false;
                }

                System.Threading.Thread.Sleep(delayMs);
                delayMs *= 2;
            }
        }

        return false;
    }

    /// <summary>
    /// Executa uma operação com tratamento de erro genérico
    /// </summary>
    public static bool ExecutarComTratamento(
        Action operacao,
        string descricaoOperacao,
        Action<Exception>? tratadorCustomizado = null)
    {
        try
        {
            operacao();
            return true;
        }
        catch (Exception ex)
        {
            _logger?.RegistrarErro(ex, descricaoOperacao);

            if (tratadorCustomizado != null)
            {
                tratadorCustomizado(ex);
            }
            else
            {
                ExibirErro($"Erro em {descricaoOperacao}: {ex.Message}");
            }

            return false;
        }
    }

    /// <summary>
    /// Executa uma operação com resultado e tratamento de erro
    /// </summary>
    public static T? ExecutarComTratamento<T>(
        Func<T> operacao,
        string descricaoOperacao,
        T? valorPadrao = null,
        Action<Exception>? tratadorCustomizado = null) where T : class?
    {
        try
        {
            return operacao();
        }
        catch (Exception ex)
        {
            _logger?.RegistrarErro(ex, descricaoOperacao);

            if (tratadorCustomizado != null)
            {
                tratadorCustomizado(ex);
            }
            else
            {
                ExibirErro($"Erro em {descricaoOperacao}: {ex.Message}");
            }

            return valorPadrao;
        }
    }

    /// <summary>
    /// Verifica se já existem backups
    /// </summary>
    public static bool ExisteBackup(string caminhoArquivo)
    {
        return ObterBackups(caminhoArquivo).Quantidade > 0;
    }

    /// <summary>
    /// Cria backup de um arquivo
    /// </summary>
    public static bool CriarBackup(
        string caminhoArquivo,
        out string caminhoBackup)
    {
        caminhoBackup = string.Empty;

        try
        {
            if (!File.Exists(caminhoArquivo))
                return false;

            string diretorioBackup = Path.Combine(
                Path.GetDirectoryName(caminhoArquivo)!,
                "backup");

            Directory.CreateDirectory(diretorioBackup);

            caminhoBackup = Path.Combine(
                diretorioBackup,
                $"{Path.GetFileNameWithoutExtension(caminhoArquivo)}_" +
                $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json");

            File.Copy(caminhoArquivo, caminhoBackup, true);

            _logger?.RegistrarInfo($"Backup criado: {caminhoBackup}");

            return true;
        }
        catch (Exception ex)
        {
            _logger?.RegistrarAviso($"Falha ao criar backup: {ex.Message}");
            caminhoBackup = string.Empty;
            return false;
        }
    }


    /// <summary>
    /// Lista os backups
    /// </summary>
    public static BackupInfo ObterBackups(string caminhoArquivo)
    {
        string diretorio = Path.Combine(
            Path.GetDirectoryName(caminhoArquivo)!,
            "backup");

        if (!Directory.Exists(diretorio))
            return new BackupInfo();

        return new BackupInfo
        {
            Backups = Directory
                .GetFiles(diretorio, "*.json")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList()
        };
    }

    /// <summary>
    /// Restaura backup
    /// </summary>
    public static bool RestaurarBackup(
        string caminhoBackup,
        string caminhoDestino)
    {
        if (File.Exists(caminhoDestino))
        {
            CriarBackup(caminhoDestino, out _);
        }

        try
        {
            if (!File.Exists(caminhoBackup))
                return false;

            File.Copy(caminhoBackup, caminhoDestino, true);

            _logger?.RegistrarInfo(
                $"Backup restaurado: {caminhoBackup}");

            return true;
        }
        catch (Exception ex)
        {
            _logger?.RegistrarErro(
                ex,
                "Erro ao restaurar backup");

            return false;
        }
    }

    /// <summary>
    /// Limpa backups antigos
    /// </summary>
    public static void LimparBackupsAntigos(string caminhoArquivo, int diasRetencao = 30)
    {
        try
        {
            string diretorioBackup = Path.Combine(
                Path.GetDirectoryName(caminhoArquivo)!,
                "backup");

            if (!Directory.Exists(diretorioBackup))
                return;

            DateTime dataLimite = DateTime.Now.AddDays(-diasRetencao);

            var backupInfo = ObterBackups(caminhoArquivo);

            foreach (var backup in backupInfo.Backups)
            {
                if (backup.LastWriteTime < dataLimite)
                {
                    backup.Delete();
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.RegistrarAviso($"Erro ao limpar backups antigos: {ex.Message}");
        }
    }

    /// <summary>
    /// Exclui todos os backups
    /// </summary>
    public static bool ExcluirTodosBackups(string caminhoArquivo)
    {
        try
        {
            var backupInfo = ObterBackups(caminhoArquivo);

            if (!backupInfo.Backups.Any())
                return false;

            foreach (var backup in backupInfo.Backups)
            {
                File.Delete(backup.FullName);
            }

            _logger?.RegistrarInfo("Todos os backups foram excluídos.");

            return true;
        }
        catch (Exception ex)
        {
            _logger?.RegistrarErro(ex, "Erro ao excluir backups.");
            return false;
        }
    }

    /// <summary>
    /// Obtém relatório de erros recentes
    /// </summary>
    public static void ExibirRelatorioDErros()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║      RELATÓRIO DE ERROS RECENTES       ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();

        var (tamanho, linhas) = _logger?.ObterInfoLog() ?? (0, 0);
        Console.WriteLine($"Arquivo de Log: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "erros.log")}");
        Console.WriteLine($"Tamanho: {(tamanho / 1024.0):F2} KB");
        Console.WriteLine($"Linhas: {linhas}");
        Console.WriteLine();

        string? erros = _logger?.ObterUltimosErros(10);
        if (!string.IsNullOrWhiteSpace(erros))
        {
            Console.WriteLine(erros);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Nenhum erro registrado!");
            Console.ResetColor();
        }

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    // ============ MÉTODOS DE EXIBIÇÃO ============

    private static void ExibirErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }

    private static void ExibirAviso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }

    private static void ExibirInfo(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($" {mensagem}");
        Console.ResetColor();
    }
}
