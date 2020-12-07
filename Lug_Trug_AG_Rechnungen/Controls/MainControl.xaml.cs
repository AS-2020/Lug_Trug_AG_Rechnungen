using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lug_Trug_AG_Rechnungen.Models;
using Lug_Trug_AG_Rechnungen.ViewModels;

namespace Lug_Trug_AG_Rechnungen.Controls
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            viewmodel.SelectedRechnung = RechnungenListe.SelectedItems.Cast<Rechnung>().ToList();
        }
    }
}
