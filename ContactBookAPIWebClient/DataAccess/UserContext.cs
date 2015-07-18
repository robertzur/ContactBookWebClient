using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContactBookAPIWebClient.DataAccess
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        { }

        public DbSet<UserProfile> UserProfiles { get; set; }


    }
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

}