using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblDurum")]
    public class Durum:ModelBase
    {
        public Durum() => Urunlers = new HashSet<Urunler>();
        public int DurumId { get; set; }

        [MaxLength(35)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Durumu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Durumunu giriniz..")]
        public string Durumu { get; set; }
        public ICollection<Urunler> Urunlers { get; set; }
     
    }
}