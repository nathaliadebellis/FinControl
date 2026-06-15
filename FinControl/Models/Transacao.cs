using System;

namespace FinControl.Models;

public class Transacao
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public TipoTransacao Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
}
