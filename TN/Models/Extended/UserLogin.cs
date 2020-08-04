using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
    public class UserLogin
    {
        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email giriniz..")]
        public string EmailID { get; set; }

        [Display(Name = "Şifreniz")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Şifrenizi giriniz...")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

    }
}