using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace Projekt_Giełda
{
    class OperacjePlikowe : IPlik, IWskazniki
    {
        string nazwaPliku;
        string sciezka;

        public OperacjePlikowe()
        {
            nazwaPliku = @"D:\Users\mstcgl.zip";
            sciezka = Path.GetDirectoryName(nazwaPliku);
        }

        public bool PobierzPlikzSieci()
        {
            string uri = "http://bossa.pl/pub/ciagle/mstock/mstcgl.zip";
            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(uri, nazwaPliku);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RozpakujPlik()
        {
            try
            {
                ZipFile.ExtractToDirectory(nazwaPliku, sciezka + @"\mstcgl");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<double> Zamkniecia(string nazwaSpolki)
        {
            string naszaKultura = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(naszaKultura);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            List<double> WartoscNaZamknieciu = new List<double>();
            string daneSpolki = (nazwaSpolki);

            Console.WriteLine(daneSpolki);
            try
            {
                FileStream sp = new FileStream(daneSpolki, FileMode.Open);
                StreamReader czyt = new StreamReader(sp);
                int licznik = 0;
                string wiersz;

                while ((wiersz = czyt.ReadLine()) != null)
                {
                    if (licznik != 0)
                    {
                        string[] tablica = wiersz.Split(',');
                        string W_na_Z_Str = tablica[5];
                        double W_na_Z_Dou = Convert.ToDouble(W_na_Z_Str);
                        WartoscNaZamknieciu.Add(W_na_Z_Dou);
                    }
                    licznik++;
                    
                }
                sp.Close();
            }
            catch
            {
                WartoscNaZamknieciu.Add(0);
            }

            return WartoscNaZamknieciu;
        }
    }
}
