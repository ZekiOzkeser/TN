using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblIl")]
    public class Il : ModelBase
    {
        public Il() => Ilces = new HashSet<Ilce>();
        [Key]
        public int IlId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "İl")]
        [Required]
        public string Ad { get; set; }

        public ICollection<Ilce> Ilces { get; set; }
    }
}