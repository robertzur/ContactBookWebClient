using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class SettingsViewModel
    {
        [Display(Name ="Private Key")]
        public string PrivateKey { get; set; }

        [Display(Name ="Public Key")]
        public string PublicKey { get; set; }

        [Display(Name ="Current Password")]
        public string OldPassword { get; set; }

        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Display(Name ="Confirm New Password")]
        public string ConfirmNewPassword { get; set; }

    }
}