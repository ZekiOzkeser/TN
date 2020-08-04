using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblIlce")]
    public class Ilce:ModelBase
    {

        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "İlçe")]
        [Required]
        public string Ad { get; set; }

        public int IlId { get; set; }
        public virtual Il Il { get; set; }

  
    }
}