using System;
using System.Collections.Generic;
using System.Text;

namespace FinControl.Models
{
    public class Transacao
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
    }
}
