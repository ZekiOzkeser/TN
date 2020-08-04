using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class Mesajlar
    {
        public IEnumerable<Mesaj> Mesajs { get; set; }

        public User Kime { get; set; }
        
        public IEnumerable<User> Users { get; set; }

        public IEnumerable<Teklifler> TeklifIcerik { get; set; }

        public IEnumerable<Mesaj> MesajIcerik { get; set; }

        public Teklifler Teklifler { get; set; }

        public Urunler Urunler { get; set; }

    }
}