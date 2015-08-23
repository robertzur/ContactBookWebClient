using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class GroupViewModel
    {
        public Group group { get; set; }
        public string[] tags { get; set; }
    }
}