using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class MesajGrubu
    {
        public int UrunId { get; set; }

        public int Kullanici { get; set; }

        public List<Mesaj> Mesajlar { get; set; }
            = new List<Mesaj>();

        public int OkunmayanSayisi =>
            Mesajlar.Count(m => m.Okunma == 0);

        public int IlkGonderen =>
            Mesajlar.OrderBy(m => m.MesajTarihi).FirstOrDefault()?.Gonderen ?? -1;

        public override string ToString() =>
            $"U: {UrunId} | O: {OkunmayanSayisi} | G: {IlkGonderen} | M: {Mesajlar.Count}";
    }
}