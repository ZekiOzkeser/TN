using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblUrunler")]
    public partial class Urunler : ModelBase
    {
        public Urunler()
        {
            Mesajs = new HashSet<Mesaj>();
            Tekliflers = new HashSet<Teklifler>();
            Resimlers = new HashSet<Resimler>();
        }

        [Key]
        [Display(Name = "İlan No")]
        public int UrunId { get; set; }

        [MaxLength(300,ErrorMessage ="300 karakter yazabilirsiniz..")]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama giriniz..", AllowEmptyStrings = false)]
        public string Aciklama { get; set; }

        [MaxLength(50,ErrorMessage ="50 karakter yazabilirsiniz..")]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Ürün Başlığı")]
        [Required(ErrorMessage = "Başlık giriniz..", AllowEmptyStrings = false)]
        public string Baslik { get; set; }

        
        [Display(Name = "Alt Limit")]
        [Required(ErrorMessage = "Altlimit giriniz..", AllowEmptyStrings = false)]
        [DataType(DataType.Currency)]
        public int AltLimit { get; set; }

        [Display(Name = "İlan Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UrunOlusturma { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UrunBitisTarihi { get; set; }

        [Display(Name = "Bakılma")]
        public int Bakilma { get; set; }

        [Display(Name ="Yayın da")]
        public bool Yayın { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }

        public int? GorselId { get; set; }

        public virtual AnaGorsel AnaGorsel { get; set; }
        public int DurumId { get; set; }

        public virtual Durum Durum { get; set; }

        public int KategoriId { get; set; }

        public virtual Kategoriler Kategoriler { get; set; }

        public ICollection<Mesaj> Mesajs { get; set; }

        public ICollection<Teklifler> Tekliflers { get; set; }

        public ICollection<Resimler> Resimlers { get; set; }

    }
}