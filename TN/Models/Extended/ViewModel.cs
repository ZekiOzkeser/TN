using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class ViewModel
    {
        public int UserID { get; set; }

        public IEnumerable<Urunler> Urunlers { get; set; }
     
        public IEnumerable<Mesaj> Mesajs { get; set; }

        public IEnumerable<KeyValuePair<int, MesajGrubu>> GelenMesajlar { get; set; }

        public IEnumerable<Kategoriler> Kategorilers { get; set; }

        public IEnumerable<KeyValuePair<int, TeklifGrubu>> GelenTeklifler { get; set; }

        public IEnumerable<Teklifler> Tekliflers { get; set; }


        public IEnumerable<User> Users { get; set; }

        public int OkunmayanSayisi { get; set; }

    }
}