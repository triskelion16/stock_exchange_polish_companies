using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;


namespace Projekt_Giełda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pobierz_i_rozpakuj_Click(object sender, EventArgs e)
        {
            IPlik plik = new OperacjePlikowe();
            string pathString = "D:\\Users";
            if (!System.IO.File.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(pathString);
            }
            if (plik.PobierzPlikzSieci() == true)
            {
                MessageBox.Show("Pobranie zakończone powodzeniem!");
                if(plik.RozpakujPlik() == true)
                    MessageBox.Show("Plik rozpakowny poprawnie!");
                else
                    MessageBox.Show("Nie udało się rozpakować pliku!\nPrawdopodobnie jest już rozpakowany!");
            }
            else
                MessageBox.Show("Niepowodzenie przy pobieraniu!\nUtwórz folder 'Users' na dysku 'D'!");
        }

        private void wyswietl_pobrane_Click(object sender, EventArgs e)
        {
            try
            {
                string[] listaObiektow = Directory.GetFiles(@"D:\Users\mstcgl");

                foreach (string obiekt in listaObiektow)
                {
                    lista.Items.Add(obiekt);
                    comboBox1.Items.Add(obiekt);
                    comboBox2.Items.Add(obiekt);
                }
            }
            catch
            {
                MessageBox.Show("Brak danych!");
            }
        }

        private void lista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IWskazniki daneSpolki = new OperacjePlikowe();

            string sciezka;
            sciezka = comboBox1.Text;

            List<double> dane = daneSpolki.Zamkniecia(sciezka);

            chart1.Series.Clear();
            chart1.Series.Add(sciezka);
            chart1.Series[sciezka].ChartType = SeriesChartType.Area;

            foreach (var obiekt in dane)
            {
                chart1.Series[sciezka].Points.Add(obiekt);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            IWskazniki daneSpolki = new OperacjePlikowe();

            string sciezka;
            sciezka = comboBox2.Text;

            List<double> dane = daneSpolki.Zamkniecia(sciezka);

            chart2.Series.Clear();
            chart2.Series.Add(sciezka);
            chart2.Series[sciezka].ChartType = SeriesChartType.Area;

            foreach (var obiekt in dane)
            {
                chart2.Series[sciezka].Points.Add(obiekt);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IWskazniki daneSpolki = new OperacjePlikowe();

            string sciezka_wykres1;
            string sciezka_wykres2;
            sciezka_wykres1 = comboBox1.Text;
            sciezka_wykres2 = comboBox2.Text;
            int dni = 3000;
            double correl = 0;

            List<double> daneA = daneSpolki.Zamkniecia(sciezka_wykres1);
            List<double> daneB = daneSpolki.Zamkniecia(sciezka_wykres2);

            if (daneA.Count < dni || daneB.Count < dni)
                MessageBox.Show("Spółka musi być na giełdzie co najmniej " + dni + "dni!");

            else
            {
                int dlugosc = 0;
                string jaka;

                dlugosc = daneA.Count - 3000;
                daneA.RemoveRange(0, dlugosc);
                daneA.RemoveRange(2997, 3);
                dlugosc = daneB.Count - 2997;
                daneB.RemoveRange(0, dlugosc);

                correl = Correlation.Pearson(daneA, daneB);

                if (correl > -0.5 && correl < 0.5)
                    jaka = "słaba";
                else
                    jaka = "silna";

                try
                {
                    chart3.Series.Clear();
                    chart3.Series.Add(sciezka_wykres1);
                    chart3.Series[sciezka_wykres1].ChartType = SeriesChartType.Point;

                    foreach (var obiekt in daneA)
                    {
                        chart3.Series[sciezka_wykres1].Points.Add(obiekt);
                    }

                    chart3.Series.Add(sciezka_wykres2);
                    chart3.Series[sciezka_wykres2].ChartType = SeriesChartType.Point;
                    foreach (var obiekt in daneB)
                    {
                        chart3.Series[sciezka_wykres2].Points.Add(obiekt);
                    }

                    string text = Convert.ToString(correl);
                    MessageBox.Show("Korelacja z ostatnich 3000 dni (z opóźnieniem 3-dniowym) jest " + jaka + " a jej współczynnik wynosi: " + text);
                }
                catch
                {
                    string text = Convert.ToString(correl);
                    MessageBox.Show("Korelacja z ostatnich 3000 dni (z opóźnieniem 3-dniowym) jest " + jaka + " a jej współczynnik wynosi: " + text);
                    //MessageBox.Show("Należy wybrać różne spółki");
                }
            }
        }

        private void autor(object sender, EventArgs e)
        {
            MessageBox.Show("Program opracował:\n\nSebastian Solecki");
        }

      
    }
}
