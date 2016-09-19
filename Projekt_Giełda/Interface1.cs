using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Giełda
{
    internal interface IPlik
    {
        bool PobierzPlikzSieci();
        bool RozpakujPlik();
    }
    internal interface IWskazniki
    {
        List<double> Zamkniecia(string sciezka);
    }
}
