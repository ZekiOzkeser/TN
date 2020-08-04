using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblMesajlar")]
    public class Mesaj
        :ModelBase
    {
       

        [Key]
        public int MesajId { get; set; }

       
        public int Kime { get; set; }


        public int Gonderen { get; set; }
           
        public int UrunId { get; set; }

        public Urunler Urunler { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "Mesaj kısmı boş..", AllowEmptyStrings = false)]
        public string MesajIcerik { get; set; }

        public int Okunma { get; set; }

        public DateTime MesajTarihi { get; set; }

        public override string ToString() =>
            $"K: {Kime} | G: {Gonderen} | M: {MesajIcerik}";
    }
}