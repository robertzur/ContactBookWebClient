using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class ContactViewModel
    {
        public Contact contact { get; set; }
        public string[] tags { get; set; }
    }
}