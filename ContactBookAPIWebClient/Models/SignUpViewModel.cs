using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmation is required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}