using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media.Media3D;

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

            for (int i = 0; i < 45; i++) cmbFeladat.Items.Add($"{i + 1}");
        }
        private void cmbFeladat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MegoldasMondatos.Content = "";
            MegoldasLista.ItemsSource = null;
            grid.ItemsSource = null;
            var methodName = $"Feladat{cmbFeladat.SelectedItem}";
            var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(this, null);
        }
        private void Feladat1()
        {
            MegoldasMondatos.Content = Lakossag.Max(x => x.NettoJovedelem);
        }
        private void Feladat2()
        {
            MegoldasMondatos.Content = Math.Round(Lakossag.Average(x => x.NettoJovedelem), 2);
        }
        private void Feladat3()
        {
            MegoldasLista.ItemsSource = Lakossag.GroupBy(x => x.Tartomany).Select(x => new { Tartomany = x.Key, Nepszam = x.Count() });
        }
        private void Feladat4()
        {
            grid.ItemsSource = Lakossag.Where(x => x.Nemzetiseg == "angolai").ToList();
        }
        private void Feladat5()
        {
            grid.ItemsSource = Lakossag.Where(x => x.Eletkor == Lakossag.Min(y => y.Eletkor)).ToList();
        }
        private void Feladat6()
        {
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Dohanyzik == "nem").Select(x => new { Id = x.Id, HaviJovedelem = x.HaviNettoJovedelem });
        }
        private void Feladat7()
        {
            grid.ItemsSource = Lakossag.Where(x => x.Tartomany == "Bajorország" && x.NettoJovedelem > 30000).OrderBy(x => x.IskolaiVegzettseg).ToList();
        }
        private void Feladat8()
        {
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Nem == "férfi").Select(x => x.ToString(true)).ToList();
        }
        private void Feladat9()
        {
            MegoldasLista.ItemsSource = Lakossag.Where(x => x.Nem == "nő" && x.Tartomany == "Bajorország").Select(x => x.ToString(false)).ToList();
        }
        private void Feladat10()
        {
            grid.ItemsSource = Lakossag.OrderByDescending(x => x.NettoJovedelem).Where(x => x.Dohanyzik == "nem").Take(10).ToList();
        }
        private void Feladat11()
        {
            grid.ItemsSource = Lakossag.OrderByDescending(x => x.Eletkor).Take(5).ToList();
        }
        private void Feladat12()
        {
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
            MegoldasMondatos.Content = $"A férfiak átlagos sörfogysztása évent: {Lakossag.Where(x => x.Nem == "férfi" && x.SorFogyasztasEvente != -1)
                .Average(x => x.SorFogyasztasEvente): 0.00}";
        }
        private void Feladat14()
        {
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
            var temp = Lakossag.OrderBy(x => x.NettoJovedelem).Select(x => x.ToString(false)).Take(3).ToList();
            temp.AddRange(Lakossag.OrderByDescending(x => x.NettoJovedelem).Select(x => x.ToString(false)).Take(3).ToList());

            MegoldasLista.ItemsSource = temp;
        }
        private void Feladat16()
        {
            MegoldasMondatos.Content = Math.Round((float)Lakossag.Where(x => x.AktivSzavazo).Count() / Lakossag.Count() * 100, 2);
        }
        private void Feladat17()
        {
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
            MegoldasMondatos.Content = Lakossag.Average(x => x.Eletkor);
        }
        private void Feladat19()
        {
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
        private void Feladat21()
        {
            int szavazoSor = Lakossag.Where(x => x.AktivSzavazo).Sum(x => x.SorFogyasztasEvente);
            int nemSzavazoSor = Lakossag.Where(x => !x.AktivSzavazo).Sum(x => x.SorFogyasztasEvente);

            if (szavazoSor > nemSzavazoSor)
                MegoldasMondatos.Content = szavazoSor;
            else
                MegoldasMondatos.Content = nemSzavazoSor;
        }
        private void Feladat37()
        {
            double atlag = Lakossag.Average(x => x.NettoJovedelem);
            var leszurt = Lakossag.Where(x => x.NettoJovedelem > atlag);
            MegoldasLista.ItemsSource = leszurt;
            MegoldasMondatos.Content = $"Átlag fizetés: {atlag}, Leszűrt darabszám: {leszurt.Count()}";
        }
        private void Feladat45()
        {
            var elsoOT = Lakossag.Where(x => x.Nem == "nő" && x.IskolaiVegzettseg != "Univerität" && x.Nepcsoport != "bajor").Take(5).ToList();
            grid.ItemsSource = elsoOT;
            var nagyobbJovedelem = Lakossag.Where(x => x.NettoJovedelem > elsoOT[0].NettoJovedelem && x.Nem == "nő").ToArray();
            var randomHarom = new List<Allampolgar>();
            Random rnd = new Random();
            if (nagyobbJovedelem.Length < 3)
            {
                MegoldasMondatos.Content = "Nincs legalább három a feltételnek megfelelő állampolgár.";
                return;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    randomHarom.Add(nagyobbJovedelem[rnd.Next(0, nagyobbJovedelem.Length)]);
                MegoldasLista.ItemsSource = randomHarom;
            }
        }
    }
}