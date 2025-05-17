using System;
using System.Collections.Generic;

namespace AuthTakePWVault.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<PasswordVault> PasswordVaults { get; set; }
    }
} 