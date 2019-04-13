using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AleAlbumWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kategorie k1 = new Kategorie(this);
            k1.Pobierz();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kategorie k1 = new Kategorie(this);
            k1.Wczytaj();
            for (int i = 0; i < k1.KategorieL.Count(); i++)
            {
                k1.KategorieL[i].Pobierz(1);
            }
            k1.ZapisXLS();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kategorie k1 = new Kategorie();
            
        }

        public ProgressBar ProgresBar1
        {
            get { return progressBar1; }
            set { progressBar1 = value; }
        }
    }
}
