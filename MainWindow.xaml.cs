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

            grid.ItemsSource = Lakossag;

            for (int i = 0; i < 40; i++) cmbFeladat.Items.Add($"{i+1}");
        }

        private void cmbFeladat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var methodName = $"Feladat{cmbFeladat.SelectedItem}";
            var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(this, null);
        }

        private void Feladat1()
        {
            MegoldasMondatos.Content = Math.Round(Lakossag.Average(x => x.NettoJovedelem),2);
        }
    }
}