using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TN.Models;

namespace TN.Models.Extended
{
    
    [MetadataType(typeof(UserMetadata))]
    public partial class User1 : Models.User
    {
        public string ConfirmPassword { get; set; }
      
    }

    public class UserMetadata
    {
        [Display(Name = "Ad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adınızı giriniz..")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Soyadınızı giriniz...")]
        public string LastName { get; set; }

        [Display(Name = "E-mail ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail adresinizi giriniz..")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Range(typeof(DateTime), "1/1/1966", "1/12/2020",ErrorMessage ="Geçerli Doğum Tarihi giriniz..")]
        public DateTime DateOfBirth { get; set; }


        [Display(Name = "Şifre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Şifrenizi giriniz...")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 karakter giriniz..")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreniz eşleşmiyor...")]
        public string ConfirmPassword { get; set; }

       

    }
}