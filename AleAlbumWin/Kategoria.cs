using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace AleAlbumWin
{
    class Kategoria
    {
        private List<Ksiazka> ksiazki = new List<Ksiazka>();
        private string nazwa;
        private string link;
        private int cnt;
        Kategorie main;

        public Kategoria(string _n, string _l, int c, Kategorie m)
        {
            nazwa = _n;
            link = _l;
            cnt = c;
            main = m;
        }

        public List<Ksiazka> Ksiazki
        {
            get { return ksiazki; }
            set { ksiazki = value; }
        }
        public string Nazwa
        {
            get { return nazwa; }
            set { nazwa = value; }
        }
        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        public int Cnt
        {
            get { return cnt; }
            set { cnt = value; }
        }
        public void DodajKsiazke(string n, double c)
        {
            Ksiazka k = new Ksiazka(n, c);
            ksiazki.Add(k);
        }

        public void Pobierz(int page) //pobieranie pozycji
        {
            string str;
            string adress="aa-"+nazwa+page+".txt";
            string URL = "http://www.alealbum.pl"+link+"/"+page;
            /*System.Net.WebRequest req = System.Net.WebRequest.Create(URL);
            req.Proxy = new System.Net.WebProxy(URL, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            str = sr.ReadToEnd().Trim();
            //str = WebUtility.HtmlDecode(str);
            str = str.Substring(str.IndexOf("<tbody>"));
            str = str.Substring(str.IndexOf("box_mainproducts"));
            str = str.Substring(str.IndexOf("tbody"));
            str = str.Substring(0, str.IndexOf("bottombar"));
            if (File.Exists(adress)) { File.Delete(adress); }
            StreamWriter sw = new StreamWriter(adress, true);
            sw.Write(str);
            sw.Close();*/
            if(page==1) main.Main.ProgresBar1.Value += 100/main.KategorieL.Count();
            Wczytaj(nazwa,page);
        }
        public void Wczytaj(string nazwa, int page)
        {
            string adress2=@"E:\Projekty C#\AleAlbumWin\AleAlbumWin\bin\Debug\aa-"+nazwa+page+".txt";
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("utf-8") }; //deklaracja obiektu HTML
            HtmlDocument htmlDocument = htmlWeb.Load(adress2);  //załadowanie całego dokumentu
            IEnumerable<HtmlNode> list = htmlDocument.DocumentNode.Descendants().Where(x=>x.Name=="td" && x.Attributes.Contains("class") && !x.Attributes["class"].Value.Split().Contains("spacefill"));
            for (int i = 0; i < list.Count(); i++)
            {
                HtmlNode element = list.ElementAt(i);
                IEnumerable<HtmlNode> spanList = element.Descendants().Where(x=>x.Name=="span" && x.Attributes.Contains("class") && x.Attributes["class"].Value=="productname");
                IEnumerable<HtmlNode> emList = element.Descendants("em");
                string pozName = spanList.ElementAt(0).InnerHtml;
                string cena = emList.ElementAt(0).InnerHtml;
                cena = cena.Substring(0, cena.Length - 3);
                //cena = cena.Replace(',', '.');
                DodajKsiazke(pozName, Convert.ToDouble(cena));
            }
            if(ksiazki.Count()<cnt && page<15) 
            {
                Pobierz(page + 1);
            }
        }
    }
}
