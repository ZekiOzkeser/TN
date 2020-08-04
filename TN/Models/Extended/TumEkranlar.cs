using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class TumEkranlar
    {
        public IEnumerable<Urunler> Urunlers { get; set; }

        public IEnumerable<Durum> Durums { get; set; }

        public IEnumerable<Kategoriler> Kategorilers { get; set; }

        public IEnumerable<Resimler> Resimlers { get; set; }


    }
}