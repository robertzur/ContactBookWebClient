using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class Tag
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string ownerId { get; set; }
    }
}