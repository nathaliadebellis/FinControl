namespace FinControl.Models;

public class GastoCategoriaResumo
{
    public string Categoria { get; set; } = string.Empty;

    public decimal Total { get; set; }

    // Valor entre 0 e 1
    public decimal Percentual { get; set; }
}