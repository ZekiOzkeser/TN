using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    [Table("tblAnagorsel")]
    public class AnaGorsel : ModelBase
    {
        public AnaGorsel() => Urunlers = new HashSet<Urunler>();
        [Key]
        public int GorselId { get; set; }

        public int ResimId { get; set; }

        public virtual Resimler Resimler { get; set; }

        public ICollection<Urunler> Urunlers { get; set; }


    }
}