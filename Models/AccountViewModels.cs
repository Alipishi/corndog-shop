using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HelmonyCornDog.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace HelmonyCornDog.Models
{
    public class RegisterViewModel
    {
        
        [MaxLength(20)]
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]

        [MaxLength(300)]
        [EmailAddress]
        [DisplayName("ایمیل")]
        [Remote("VerifyEmail","Account")]

        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("کلمه عبور")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$" , ErrorMessage = " کلمه عبور باید شامل حرف و عدد و حداقل 6 کارکتر باشد.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("تکرار کلمه عبور")]

        [Compare("Password",ErrorMessage = "تکرار کلمه عبور یکسان نیست.")]
        
        public string Repassword { get; set; }

    }

    public class LoginViewModel()
    {
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]

        [MaxLength(300)]
        [EmailAddress]
        [DisplayName("ایمیل")]


        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("کلمه عبور")]

        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RemmemberMe { get; set; }

    }
}
