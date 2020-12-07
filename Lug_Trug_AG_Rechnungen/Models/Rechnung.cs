using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lug_Trug_AG_Rechnungen.Models
{
    class Rechnung
    {
        public int RechnungsNummer { get; set; }
        public DateTime DatumFaelligkeit { get; set; }
        public int KundenNummer { get; set; }
        public double SummeRechnung { get; set; }
        public DateTime? DatumBegleichung { get; set; }

    }
}
