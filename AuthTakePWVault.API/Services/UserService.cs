using AuthTakePWVault.Data.Context;
using AuthTakePWVault.Data.Entities;
using AuthTakePWVault.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthTakePWVault.API.Services
{
    public class UserService : IUserService
    {
        private readonly AuthTakePWVaultContext _context;

        public UserService(AuthTakePWVaultContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required");

            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                throw new ArgumentException("Username is already taken");

            if (await _context.Users.AnyAsync(x => x.Email == user.Email))
                throw new ArgumentException("Email is already registered");

            user.Password = CryptoHelper.HashPassword(password);
            user.CreatedDate = DateTime.Now;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!CryptoHelper.VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AnyAsync(x => x.Email == email);
        }
    }
} 