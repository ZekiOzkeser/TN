using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TN.Models.Extended
{
   
        public class ResetPasswordModel
        {
            [Required(ErrorMessage = "Yeni Şifrenizi giriniz...", AllowEmptyStrings = false)]
            [DataType(DataType.Password)]
            [Display(Name = "Yeni Şifre")]
            public string NewPassword { get; set; }

            [Display(Name = "Şifre Tekrar")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "Yeni şifreniz eşleşmiyor...")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string ResetCode { get; set; }
        }
    
}