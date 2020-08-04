using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TN.Models
{
    public partial class User:ModelBase
    {
        public User()
        {
            Iletisims = new HashSet<Iletisim>();
            Urunlers = new HashSet<Urunler>();
        }
        [Key]
        public int UserID { get; set; }

        [Display(Name = "Ad")]
        [Required,MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "E-mail ")]
        [Required]
        public string EmailID { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Üyelik Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime UyelikBaslangic { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }

        public System.Guid ActivationCode { get; set; }

        public string ResetPasswordCode { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Üye olmak için Üyelik Sözleşme Koşulları’nı ve Gizlilik Politikası’nı kabul etmeniz gerekmektedir.")]
        public bool Sozlesme { get; set; }
      
        public ICollection<Iletisim> Iletisims { get; set; }
        public ICollection<Urunler> Urunlers { get; set; }


    }
}