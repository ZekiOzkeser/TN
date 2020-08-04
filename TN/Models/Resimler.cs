using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblResimler")]
    public class Resimler : ModelBase
    {
        public Resimler() => AnaGorsels = new HashSet<AnaGorsel>();
        [Key]
        public int ResimId { get; set; }

        [Column(TypeName = "nvarchar")]
        [Required]
        [DataType(DataType.ImageUrl)]
        public string Resim { get; set; }

        public string ImageUrl { get; set; }

        public int UrunId { get; set; }

        public virtual Urunler Urunlers { get; set; }

        public ICollection<AnaGorsel> AnaGorsels { get; set; }

    }
}