using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TN.Models
{
    [Table("tblIletisim")]
    public class Iletisim:ModelBase
    {

        [Key]
        public int IletisimId { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Telefon")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Telefon numaranızı giriniz..")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Geçerli bir telefon numarası değil..")]
        public string Tel { get; set; }

        [MaxLength(500)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Adres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adres giriniz..")]
        public string Adres { get; set; }

        public int IlId { get; set; }

        public Il Il { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }

    }

   
}