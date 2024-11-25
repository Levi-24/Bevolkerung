using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Bevolkerung
{
    public partial class MainWindow : Window
    {
        private List<Allampolgar> Lakossag;

        public MainWindow()
        {
            InitializeComponent();

            Lakossag = new List<Allampolgar>();
            using StreamReader sr = new StreamReader(@"../../../src/bevölkerung.txt", Encoding.UTF8);
            _ = sr.ReadLine();
            while (!sr.EndOfStream) Lakossag.Add(new Allampolgar(sr.ReadLine()));

            for (int i = 0; i < 40; i++) cmbFeladat.Items.Add($"{i+1}");
        }
        private void cmbFeladat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var methodName = $"Feladat{cmbFeladat.SelectedItem}";
            var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(this, null);
        }
        public void Reset()
        {
            MegoldasMondatos.Content = "";
            MegoldasLista.ItemsSource = null;
            grid.ItemsSource = null;
        }
        private void Feladat1() 
        {
            Reset();
            MegoldasMondatos.Content = Lakossag.Max(x => x.NettoJovedelem);
        }
        private void Feladat2()
        {
            Reset();
            MegoldasMondatos.Content = Math.Round(Lakossag.Average(x => x.NettoJovedelem),2);
        }
        private void Feladat3()
        {
            Reset();
            MegoldasLista.ItemsSource = Lakossag.GroupBy(x => x.Tartomany).Select(x => new { Tartomany = x.Key, Nepszam = x.Count() });
        }
        private void Feladat4()
        {
            Reset();
            grid.ItemsSource = Lakossag.Where(x => x.Nemzetiseg == "angolai").ToList();
        }
        private void Feladat5()
        {
            Reset();
            grid.ItemsSource = Lakossag.Where(x => x.Eletkor == Lakossag.Min(y => y.Eletkor)).ToList();
        }
        private void Feladat6()
        {
            Reset();
            MegoldasLista.ItemsSource = Lakossag.Where(x => !x.Dohanyzik).Select(x => new { Id = x.Id, HaviJovedelem = x.HaviNettoJovedelem });
        }
        private void Feladat7()
        {
            Reset();
            grid.ItemsSource = Lakossag.Where(x => x.Tartomany == "Bajorország" && x.NettoJovedelem > 30000).OrderBy(x => x.IskolaiVegzettseg).ToList();
        }
        private void Feladat8()
        {
            Reset();
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Nem == "férfi").Select(x => x.ToString(true)).ToList();
        }
        private void Feladat9()
        {
            Reset();
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Nem == "nő" && x.Tartomany == "Bajorország").Select(x => x.ToString(false)).ToList();
        }
    }
}