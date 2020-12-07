using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lug_Trug_AG_Rechnungen.Models
{
    class Kunde
    {
        public int KundenNummer { get; set; }
        public string NameFirma { get; set; }
        //public Adresse AdresseKunde { get; set; }
        public string AdresseKunde { get; set; }
        public string Ansprechpartner { get; set; }
        public string Telefonnummer { get; set; }
        public string Aktiv { get; set; }
    }
}
