using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class UrunKullanici
    {
        public User User { get; set; }
        public Iletisim Iletisim { get; set; }

        public Il Il { get; set; }

        public Ilce Ilce { get; set; }
    }

}