using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class TeklifGrubu
    {
        public int UrunId { get; set; }

        public int Kullanici { get; set; }

        public List<Teklifler> Teklifler { get; set; }
            = new List<Teklifler>();

        public int OkunmayanSayisi =>
            Teklifler.Count(m => m.Gorulme == 0);

        public int IlkGonderen =>
              Teklifler.OrderBy(m => m.TeklifTarihi).FirstOrDefault()?.TeklifVeren ?? -1;

        public override string ToString() =>
            $"U: {UrunId} | O: {OkunmayanSayisi} | G: {IlkGonderen} | M: {Teklifler.Count}";
    }
}