using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class Enums
    {
        public enum SearchScopes
        {
            all = 0,
            name = 1,
            address = 2,
            tag = 3

        }
    }
}