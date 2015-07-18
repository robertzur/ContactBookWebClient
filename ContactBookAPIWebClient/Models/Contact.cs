using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class Contact
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string phoneNumber { get; set; }
        public string cellNumber { get; set; }
        public string email { get; set; }
        public string skypeId { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public bool isContactGroup { get; set; }
        public string ownerId { get; set; }
        public string parentId { get; set; }
        public string[] tags { get; set; }
    }
}