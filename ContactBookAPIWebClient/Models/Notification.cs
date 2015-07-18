using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class Notification
    {
        public Notification(string message, Nature nature)
        {
            this.Message = message;
            this.Nature = nature;
        }
        public string Message { get; set; }

        public Nature Nature { get; set; }

    }

    public enum Nature
    {
        success = 0,
        warning = 1,
        danger = 2
    }
}