using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblTeklifler")]
    public class Teklifler
        : ModelBase
    {
        [Key]
        public int TeklifId { get; set; }

        [Display(Name = "Teklif")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Tutar kısmı boş..", AllowEmptyStrings = false)]
        public int Tutar { get; set; }

        public int Kime { get; set; }
        public int Gorulme { get; set; }
        public int TeklifVeren { get; set; }
        public DateTime TeklifTarihi { get; set; }

        public int UrunId { get; set; }

        public virtual Urunler Urunler { get; set; }
        public override string ToString() =>
               $"K: {Kime} | G: {TeklifVeren} | M: {Tutar}";
    }
}