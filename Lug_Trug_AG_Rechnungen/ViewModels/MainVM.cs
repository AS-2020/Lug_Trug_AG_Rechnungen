using Lug_Trug_AG_Rechnungen.Helper;
using Lug_Trug_AG_Rechnungen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lug_Trug_AG_Rechnungen.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Rechnung> SelectedRechnung { get; set; }

        private ObservableCollection<Rechnung> _rechnungen;
        public ObservableCollection<Rechnung> Rechnungen
        {
            get { return _rechnungen; }
            set
            {
                _rechnungen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rechnungen"));
            }
        }

        private bool _rechnungsNummerTextBox;
        public bool RechnungsNummerTextBox
        {
            get { return _rechnungsNummerTextBox; }
            set
            {
                _rechnungsNummerTextBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RechnungsNummerTextBox"));
            }
        }
        private string _sortieren;
        public string Sortieren
        {
            get { return _sortieren; }
            set
            {
                _sortieren = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sortieren"));
            }
        }

        private bool _aendernButton;
        public bool AendernButton
        {
            get { return _aendernButton; }
            set
            {
                _aendernButton = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AendernButton"));
            }
        }

        private string _rechnungSuchen = "";
        public string RechnungSuchen
        {
            get { return _rechnungSuchen; }
            set
            {
                _rechnungSuchen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RechnungSuchen"));
            }
        }

        private int _kundenNummer;
        public int KundenNummer
        {
            get { return _kundenNummer; }
            set
            {
                _kundenNummer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KundenNummer"));
            }
        }

        private int _rechnungsNummer;
        public int RechnungsNummer
        {
            get { return _rechnungsNummer; }
            set
            {
                _rechnungsNummer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RechnungsNummer"));
            }
        }

        private DateTime _datumFaelligkeit = DateTime.Now.Date;
        public DateTime DatumFaelligkeit
        {
            get { return _datumFaelligkeit; }
            set
            {
                _datumFaelligkeit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DatumFaelligkeit"));
            }
        }
        private string _summeRechnung;
        public string SummeRechnung
        {
            get { return _summeRechnung; }
            set
            {
                _summeRechnung = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SummeRechnung"));
            }
        }
        private DateTime? _datumBegleichung;
        public DateTime? DatumBegleichung
        {
            get { return _datumBegleichung; }
            set
            {
                _datumBegleichung = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DatumBegleichung"));
            }
        }


        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand SortCommand { get; set; }
        public void Cancel(object o)
        {
            RechnungsNummer = 0;
            DatumFaelligkeit = DateTime.Now;
            KundenNummer = 0;
            SummeRechnung = "";
            DatumBegleichung = null;
            AendernButton = false;
            RechnungsNummerTextBox = false;
        }

        public void Exit(object o)
        {
            Environment.Exit(0);
        }

        public void SelectRechnung(object o)
        {
            try
            {
                Rechnung r = SelectedRechnung.LastOrDefault<Rechnung>();
                RechnungsNummer = r.RechnungsNummer;
                DatumFaelligkeit = r.DatumFaelligkeit;
                KundenNummer = r.KundenNummer;
                SummeRechnung = r.SummeRechnung.ToString();
                DatumBegleichung = r.DatumBegleichung;
                AendernButton = true;
                RechnungsNummerTextBox = true;

            }
            catch (System.ArgumentNullException)
            {

                MessageBox.Show("Keine Rechnung gewählt");
            }

        }

        public MainVM()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("de-DE", false);
            Buchhaltung.Instance.Load();
            Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(r => r.RechnungsNummer));

            SaveCommand = new RelayCommand((o) =>
            {
                Rechnung vorhanden = Buchhaltung.Instance.GetRechnungen().Find(r => r.RechnungsNummer == RechnungsNummer);
                if (vorhanden == null && RechnungsNummer > 0 && KundenNummer > 0)
                {
                    Rechnung r = new Rechnung()
                    {
                        RechnungsNummer = RechnungsNummer,
                        DatumFaelligkeit = DatumFaelligkeit,
                        KundenNummer = KundenNummer,
                        SummeRechnung = Convert.ToDouble(SummeRechnung, System.Globalization.CultureInfo.CurrentCulture),
                        DatumBegleichung = DatumBegleichung
                    };
                    Buchhaltung.Instance.AddRechnung(r);
                    Rechnungen.Add(r);
                    Buchhaltung.Instance.Save(r);
                    Cancel(o);
                }
                else if (vorhanden != null)
                {
                    MessageBox.Show("Rechnungsnummer existert bereits");
                }
                else if (RechnungsNummer <= 0 )
                {
                    MessageBox.Show("Rechnungsnummer muss größer als 0 sein");
                }
                else if (KundenNummer <= 0)
                {
                    MessageBox.Show("Kundennummer muss größer als 0 sein");
                }
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            SearchCommand = new RelayCommand((o) =>
            {
                Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().FindAll(r => r.KundenNummer.Equals(RechnungSuchen)));
            });

            SortCommand = new RelayCommand((o) =>
            {
                if (Sortieren == "Rechnung sortieren")
                {
                    Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(r => r.RechnungsNummer)); //.OrderBy(u => u.KundenNummer));
                    Sortieren = "Kunden sortieren";
                }
                else if (Sortieren == "Kunden sortieren")
                {
                    Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(r => r.KundenNummer));
                    Sortieren = "Rechnung sortieren";
                }
            });

            SelectCommand = new RelayCommand(SelectRechnung);

            ChangeCommand = new RelayCommand((o) =>
            {
                Rechnung vorhanden = Buchhaltung.Instance.GetRechnungen().Find(r => r.RechnungsNummer == RechnungsNummer);
                bool[] geandert = new bool[4] { false, false, false, false};
                if (RechnungsNummer > 0 && KundenNummer > 0)
                {
                    Rechnung r = new Rechnung()
                    {
                        RechnungsNummer = RechnungsNummer,
                        DatumFaelligkeit = DatumFaelligkeit,
                        KundenNummer = KundenNummer,
                        SummeRechnung = Convert.ToDouble(SummeRechnung, System.Globalization.CultureInfo.CurrentCulture),
                        DatumBegleichung = DatumBegleichung
                    };
                    if (r.DatumFaelligkeit != vorhanden.DatumFaelligkeit)
                    {
                        geandert[0] = true;
                    }
                    if (r.KundenNummer != vorhanden.KundenNummer)
                    {
                        geandert[1] = true;
                    }
                    if (r.SummeRechnung != vorhanden.SummeRechnung)
                    {
                        geandert[2] = true;
                    }
                    if (r.DatumBegleichung != vorhanden.DatumBegleichung)
                    {
                        geandert[3] = true;
                    }

                    Buchhaltung.Instance.RemoveRechnung(vorhanden);
                    Buchhaltung.Instance.AddRechnung(r);
                    Buchhaltung.Instance.Change(r, geandert);
                    Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(re => re.RechnungsNummer));
                    Cancel(o);
                }
                else if (RechnungsNummer > 0)
                {
                    MessageBox.Show("Rechnungsnummer muss größer als 0 sein");
                }
                else if (KundenNummer <= 0)
                {
                    MessageBox.Show("Kundennummer muss größer als 0 sein");
                }
            });
        }
    }
}