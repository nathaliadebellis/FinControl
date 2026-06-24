using System;
using System.Collections.Generic;
using System.Text;

namespace FinControl.Models;

public class BackupInfo
{
    public List<FileInfo> Backups { get; set; } = [];

    public int Quantidade => Backups.Count;

    public long TamanhoTotalBytes =>
        Backups.Sum(b => b.Length);

    public DateTime? UltimoBackup =>
        Backups.FirstOrDefault()?.LastWriteTime;
}