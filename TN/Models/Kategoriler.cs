using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblKategoriler")]
    public class Kategoriler : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoriler()
        {
            Urunler = new HashSet<Urunler>();
            AltKategoriler = new HashSet<AltKategoriler>();
        }
        [Key]
        public int KategoriId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "nvarchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Kategori giriniz..")]
        [Display(Name ="Kategori")]
        public string KategoriAdi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Urunler> Urunler { get; set; }

        public ICollection<AltKategoriler> AltKategoriler { get; set; }
    }
}
