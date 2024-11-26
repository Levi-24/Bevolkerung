using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bevolkerung
{
    class Allampolgar
    {
        private int haviNettoJovedelem;

        public int Id { get; set; }
        public string Nem { get; set; }
        public int SzuletesiEv { get; set; }
        public int Eletkor => DateTime.Now.Year - SzuletesiEv;
        public int Suly { get; set; }
        public int Magassag { get; set; }
        public string Dohanyzik { get; set; }
        public string Nemzetiseg { get; set; }
        public string Nepcsoport { get; set; }
        public string Tartomany { get; set; }
        public int NettoJovedelem { get; set; }
        public int HaviNettoJovedelem => NettoJovedelem / 12;
        public string IskolaiVegzettseg { get; set; }
        public string PolitikaiNezet { get; set; }
        public bool AktivSzavazo { get; set; }
        public int SorFogyasztasEvente { get; set; }
        public int KrumpliFogyasztasEvente { get; set; }

        public Allampolgar(string sor)
        {
            var adatok = sor.Split(';');
            Id = int.Parse(adatok[0]);
            Nem = adatok[1];
            SzuletesiEv = int.Parse(adatok[2]);
            Suly = int.Parse(adatok[3]);
            Magassag = int.Parse(adatok[4]);
            Dohanyzik = adatok[5];
            Nemzetiseg = adatok[6];
            Nepcsoport = adatok[7];
            Tartomany = adatok[8];
            NettoJovedelem = int.Parse(adatok[9]);
            IskolaiVegzettseg = adatok[10];
            PolitikaiNezet = adatok[11];
            AktivSzavazo = adatok[12] == "igen";
            if (adatok[13] == "NA") { SorFogyasztasEvente = -1; }
            else SorFogyasztasEvente = int.Parse(adatok[13]);
            if (adatok[14] == "NA") { KrumpliFogyasztasEvente = -1; }
            else KrumpliFogyasztasEvente = int.Parse(adatok[14]);
        }

        public string ToString(bool valami)
        {
            if (valami)
            {
                return $"{Id}\t{Nem}\t{SzuletesiEv}\t{Eletkor}\t{Suly}";
            }
            else
            {
                return $"{Id}\t{Nemzetiseg}\t{Nepcsoport}\t{Tartomany}\t{NettoJovedelem}";
            }
        }

        public override string ToString()
        {
            return $"{Id}\t{Nem}\t{SzuletesiEv}\t{Eletkor}\t{Suly}\t{Magassag}\t{Dohanyzik}\t{Nemzetiseg}\t{Nepcsoport}\t{Tartomany}\t{NettoJovedelem}\t{HaviNettoJovedelem}\t{IskolaiVegzettseg}\t{PolitikaiNezet}\t{AktivSzavazo}\t{SorFogyasztasEvente}\t{KrumpliFogyasztasEvente}";
        }
    }
   
}
