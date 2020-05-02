using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicProject.Models
{
    public class UserLogin
    {
        [Display(Name ="Email Id")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Email Id Required")]
        public string Email { get; set; }

        [Display(Name = "Password ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display (Name ="Remember Me")]
        public bool RememberMe { get; set; }

    }
}