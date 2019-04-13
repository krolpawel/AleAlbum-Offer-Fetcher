using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AleAlbumWin
{
    class Ksiazka
    {
        private string nazwa;
        private double cena;

        public Ksiazka()
        {
            nazwa = "nieznana";
            cena = 0.0;
        }
        public Ksiazka(string _n, double _c)
        {
            nazwa = _n;
            cena = _c;
        }

        public string Nazwa
        {
            get { return nazwa; }
            set { nazwa = value; }
        }
        public double Cena
        {
            get { return cena; }
            set { cena = value; }
        }
    }
}
