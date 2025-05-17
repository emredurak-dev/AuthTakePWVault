using System;

namespace AuthTakePWVault.Data.Entities
{
    public class PasswordVault
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
} 