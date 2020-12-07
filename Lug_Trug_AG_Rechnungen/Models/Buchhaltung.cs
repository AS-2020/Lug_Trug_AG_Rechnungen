using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lug_Trug_AG_Rechnungen.Models
{
    class Buchhaltung
    {
        private List<Rechnung> Rechnungen;
        private Buchhaltung()
        {
            Rechnungen = new List<Rechnung>();
        }
        private static Buchhaltung _instance;
        public static Buchhaltung Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Buchhaltung();
                }
                return _instance;
            }
        }
        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=Lug_Trug_AG;User ID=sa;Password=P@ssword";
        public void AddRechnung(Rechnung r)
        {
            Rechnungen.Add(r);
        }
      
        public List<Rechnung> GetRechnungen()
        {
            return Rechnungen;
        }
        public void RemoveRechnung(Rechnung r)
        {
            Rechnungen.Remove(r);
        }
        public void Save(Rechnung rechnung)
        {
            //CultureInfo.CurrentUICulture = new CultureInfo("en-US", false);

            string summemitkomma = rechnung.SummeRechnung.ToString();
            string summemitpunkt = summemitkomma.Replace(',', '.');

            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();
            SqlCommand com = new SqlCommand();

            string sql = $"Insert into Rechnungen values ({rechnung.RechnungsNummer}, '{rechnung.DatumFaelligkeit}', '{rechnung.KundenNummer}', " + 
                $"'{summemitpunkt}', '{((rechnung.DatumBegleichung!=null)? $"{rechnung.DatumBegleichung}" : null)}')"; //Convert.ToDecimal(rechnung.SummeRechnung, System.Globalization.CultureInfo.CurrentCulture)

            com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.InsertCommand = com;
            adapter.InsertCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();

            //CultureInfo.CurrentUICulture = new CultureInfo("de-DE", false);
        }

        public void Load()
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            string sql = "Select * from Rechnungen";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                Rechnung r = new Rechnung();

                r.RechnungsNummer = (int)reader.GetValue(0);
                r.DatumFaelligkeit = (DateTime)reader.GetValue(1);
                r.KundenNummer = (int)reader.GetValue(2);
                string summe = reader.GetValue(3).ToString();
                r.SummeRechnung = double.Parse(summe);
                r.DatumBegleichung = (DateTime)reader.GetValue(4);
                DateTime falsch = new DateTime(1900, 1, 1);
                if (r.DatumBegleichung == falsch.Date)
                {
                    r.DatumBegleichung = null;
                }
                Instance.AddRechnung(r);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }

        public void Change(Rechnung r, bool[] geandert)
        {
            string summemitkomma = r.SummeRechnung.ToString();
            string summemitpunkt = summemitkomma.Replace(',', '.');

            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();

            string sql = $@"Update Rechnungen set {(geandert[0] ? $"DatumFaelligkeit = '{r.DatumFaelligkeit}'" : "")}{((geandert[0] && geandert[1] | geandert[2] | geandert[3]) ? "," : "")}" +
                $"{(geandert[1] ? $"Kundennummer = '{r.KundenNummer}'" : "")}{((geandert[1] && geandert[2] | geandert[3]) ? "," : "")}" +
                $"{ (geandert[2] ? $"Summe = '{summemitpunkt}'" : "")}{((geandert[2] && geandert[3]) ? "," : "")}" +
                $"{ (geandert[3] ? $"DatumBegleichung = '{r.DatumBegleichung}'" : "")} where Rechnungsnummer = '{r.RechnungsNummer}'";
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.UpdateCommand = com;
            adapter.UpdateCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();
        }
    }
}