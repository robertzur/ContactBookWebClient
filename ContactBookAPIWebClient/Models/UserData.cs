using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ContactBookAPIWebClient.Models
{
    public class UserData
    {
        public string PublicKey { get; set; }
        public string AuthenticationHash { get; set; }
        public DateTime Timestamp { get; set; }

        public void GenerateAuthenticationHash(string plainText)
        {
            SHA256Managed hashAlgorithm = new SHA256Managed();
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] hash = hashAlgorithm.ComputeHash(bytes);
            

            this.AuthenticationHash = Convert.ToBase64String(hash);
        }
    }
}