using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Web;
using Spire.Xls;

namespace AleAlbumWin
{
    class Kategorie
    {
        private List<Kategoria> kategorieL = new List<Kategoria>();
        private Form1 main;

        public Kategorie() { }
        public Kategorie(Form1 m)
        {
            main = m;
        }

        public Form1 Main
        {
            get { return main; }
            set { main = value; }
        }
        public List<Kategoria> KategorieL
        {
            get { return kategorieL; }
            set { kategorieL = value; }
        }
        public void Pobierz()
        {
            string str;
            string URL = "http://www.alealbum.pl/pl/c/";
            System.Net.WebRequest req = System.Net.WebRequest.Create(URL);
            req.Proxy = new System.Net.WebProxy(URL, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            str = sr.ReadToEnd().Trim();
            str = WebUtility.HtmlDecode(str);
            str = str.Substring(str.IndexOf("<tbody>"));
            str = str.Substring(str.IndexOf("<ul"));
            str = str.Substring(0, str.IndexOf("</ul>") + 8);
            if (File.Exists(@"E:\alealbum.txt")) { File.Delete(@"E:\alealbum.txt"); }
            StreamWriter sw = new StreamWriter(@"E:\alealbum.txt", true);
            sw.Write(str);
            sw.Close();
        }
        public void Wczytaj()
        {
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("utf-8") }; //deklaracja obiektu HTML
            HtmlDocument htmlDocument = htmlWeb.Load(@"E:\alealbum.txt");  //załadowanie całego dokumentu
            IEnumerable<HtmlNode> list = htmlDocument.DocumentNode.Descendants("li");
            for (int i = 0; i < list.Count()-2; i++)
            {
                HtmlNode element = list.ElementAt(i);
                IEnumerable<HtmlNode> aList = element.Descendants("a");
                IEnumerable<HtmlNode> emList = element.Descendants("em");
                string katName = aList.ElementAt(0).InnerHtml;
                string katLink = aList.ElementAt(0).Attributes["href"].Value;
                string count = emList.ElementAt(0).InnerHtml;
                count = count.Substring(1, count.Length - 2);
                DodajKategorie(katName, katLink, Convert.ToInt32(count));
            }
        }
        public void DodajKategorie(string n, string l, int c)
        {
            Kategoria k = new Kategoria(n, l, c, this);
            kategorieL.Add(k);
        }
        public void ZapisXLS()
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];

            /*CellStyle oddStyle = workbook.Styles.Add("oddStyle");
	        oddStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
	        oddStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
	        oddStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
	        oddStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
	        oddStyle.KnownColor = ExcelColors.LightGreen1;
	 
	        CellStyle evenStyle = workbook.Styles.Add("evenStyle");
	        evenStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
	        evenStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
	        evenStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
	        evenStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
	        evenStyle.KnownColor = ExcelColors.LightTurquoise;

            CellStyle styleHeader = sheet.Rows[0].Style;
	        styleHeader.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
	        styleHeader.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
	        styleHeader.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
	        styleHeader.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
	        styleHeader.VerticalAlignment = VerticalAlignType.Center;
	        styleHeader.KnownColor = ExcelColors.Green;
	        styleHeader.Font.KnownColor = ExcelColors.White;
	        styleHeader.Font.IsBold = true;
            */
            sheet.Range["A1:D1"].Merge();
            sheet.Range["A1"].Text = "Wykaz cen książek";
            
            sheet.Range["A2"].Text = "Nazwa";
            sheet.Range["B2"].Text = "Cena";
            sheet.Range["C2"].Text = "CENA -20%";
            sheet.Range["D2"].Text = "Uwagi";
            int row=3;
            foreach (Kategoria kategoria in kategorieL)
            {
                sheet.Range[row, 1, row, 4].Merge();
                sheet.Range[row, 1].Text = kategoria.Nazwa;
                row++;
                foreach (Ksiazka ksiazka in kategoria.Ksiazki)
                {
                    sheet.Range[row, 1].Text = ksiazka.Nazwa;
                    sheet.Range[row, 2].Text = Convert.ToString(ksiazka.Cena);
                    sheet.Range[row, 3].Text = Convert.ToString(0.8*ksiazka.Cena);
                    row++;
                }
                
                

            }
            workbook.SaveToFile("Sample.xls");
        }
    }
}
