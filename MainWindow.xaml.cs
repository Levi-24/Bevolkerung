using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;

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

            for (int i = 0; i < 40; i++) cmbFeladat.Items.Add($"{i + 1}");
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
            MegoldasMondatos.Content = Math.Round(Lakossag.Average(x => x.NettoJovedelem), 2);
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
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Dohanyzik == "nem").Select(x => new { Id = x.Id, HaviJovedelem = x.HaviNettoJovedelem });
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
        private void Feladat10()
        {
            Reset();
            grid.ItemsSource = Lakossag.OrderByDescending(x => x.NettoJovedelem).Where(x => x.Dohanyzik == "nem").Take(10).ToList();
        }
        private void Feladat11()
        {
            Reset();
            grid.ItemsSource = Lakossag.OrderByDescending(x => x.Eletkor).Take(5).ToList();
        }
        private void Feladat12()
        {
            Reset();

            MegoldasLista.ItemsSource = Lakossag
                .Where(x => x.Nemzetiseg == "német")
                .GroupBy(x => x.Nepcsoport)
                .SelectMany(group =>
                {
                    var sorok = new List<string> { group.Key };

                    sorok.AddRange(group.Select(person =>
                        $"    {(person.AktivSzavazo ? "aktív szavazó" : "nem aktív szavazó")} - {person.PolitikaiNezet}"
                    ));

                    return sorok;
                })
                .ToList();


        }
        private void Feladat13()
        {
            Reset();
            MegoldasMondatos.Content = $"A férfiak átlagos sörfogysztása évent: {Lakossag.Where(x => x.Nem == "férfi" && x.SorFogyasztasEvente != -1)
                .Average(x => x.SorFogyasztasEvente): 0.00}";
        }
        private void Feladat14()
        {
            Reset();
            MegoldasLista.ItemsSource = Lakossag
                .GroupBy(x => x.IskolaiVegzettseg)
                .OrderBy(group => group.Key)
                .SelectMany(group =>
                {
                    var sorok = new List<string> { group.Key };

                    sorok.AddRange(group.Select(person => $"    {person.Id} ({person.SzuletesiEv})"));

                    return sorok;
                })
                .ToList();
        }
        private void Feladat15()
        {
            Reset();
            var temp = Lakossag.OrderBy(x => x.NettoJovedelem).Select(x => x.ToString(false)).Take(3).ToList();
            temp.AddRange(Lakossag.OrderByDescending(x => x.NettoJovedelem).Select(x => x.ToString(false)).Take(3).ToList());

            MegoldasLista.ItemsSource = temp;
        }
        private void Feladat16()
        {
            Reset();
            MegoldasMondatos.Content = Math.Round((float)Lakossag.Where(x => x.AktivSzavazo).Count() / Lakossag.Count() * 100, 2);
        }
        private void Feladat17()
        {
            Reset();
            var temp = Lakossag
                .Where(x => x.AktivSzavazo)
                .GroupBy(x => x.Tartomany)
                .SelectMany(group =>
                {
                    var sorok = new List<string> { group.Key };
                    sorok.AddRange(group.Select(person => $"    {person.Id}, {person.SzuletesiEv}, {person.Nem}, {person.Suly}, {person.Magassag}, {person.Dohanyzik}, {person.Nemzetiseg}, {person.Nepcsoport}, {person.Tartomany}, {person.NettoJovedelem}, {person.IskolaiVegzettseg}, {person.PolitikaiNezet}, {person.AktivSzavazo}, {person.SorFogyasztasEvente}, {person.KrumpliFogyasztasEvente}"));
                    return sorok;
                })
                .ToList();

            MegoldasLista.ItemsSource = temp;
        }
        private void Feladat18()
        {
            Reset();
            MegoldasMondatos.Content = Lakossag.Average(x => x.Eletkor);
        }
        private void Feladat19()
        {
            Reset();
            var temp = Lakossag.GroupBy(x => x.Tartomany)
                .Select(group => new
                {
                    Tartomany = group.Key,
                    AtlagJovedelem = group.Average(x => x.NettoJovedelem),
                    LakossagSzama = group.Count()
                })
                .ToList();

            var maxAtlag = temp.Max(x => x.AtlagJovedelem);

            var legmagasabbAtlagTartomany = temp.Where(x => x.AtlagJovedelem == maxAtlag)
                .OrderByDescending(x => x.LakossagSzama)
                .ToList();

            MegoldasLista.ItemsSource = legmagasabbAtlagTartomany;
        }
        private void Feladat20()
        {

        }
    }
}