using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblaltkategoriler")]
    public class AltKategoriler:ModelBase
    {
        [Key]
        public int AltId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "AltKategori")]
        public string AltKategori { get; set; }

        public int KategoriId { get; set; }

        public virtual Kategoriler Kategoriler { get; set; }


    }
}