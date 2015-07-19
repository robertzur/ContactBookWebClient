using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class Contact
    {
        public string _id { get; set; }

        [Display(Name="Name")]
        public string name { get; set; }
        [Display(Name="Description")]
        public string description { get; set; }
        [Display(Name="Address Line 1")]
        public string addressLine1 { get; set; }
        [Display(Name="Address Line 2")]
        public string addressLine2 { get; set; }
        [Display(Name="Phone Number")]
        public string phoneNumber { get; set; }
        [Display(Name="Cell Number")]
        public string cellNumber { get; set; }
        [Display(Name="Email")]
        public string email { get; set; }
        [Display(Name="Skype Id")]
        public string skypeId { get; set; }
        [Display(Name="Twitter")]
        public string twitter { get; set; }
        [Display(Name="Facebook")]
        public string facebook { get; set; }
        public bool isContactGroup { get; set; }
        public string ownerId { get; set; }
        public string parentId { get; set; }
        public string[] tags { get; set; }
    }
}