using AuthTakePWVault.Data.Context;
using AuthTakePWVault.Data.Entities;
using AuthTakePWVault.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthTakePWVault.API.Services
{
    public class PasswordVaultService : IPasswordVaultService
    {
        private readonly AuthTakePWVaultContext _context;

        public PasswordVaultService(AuthTakePWVaultContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PasswordVault>> GetUserPasswordsAsync(int userId)
        {
            var passwords = await _context.PasswordVaults
                .Where(p => p.UserId == userId)
                .ToListAsync();

            foreach (var password in passwords)
            {
                password.Password = CryptoHelper.DecryptString(password.Password);
            }

            return passwords;
        }

        public async Task<PasswordVault> GetPasswordByIdAsync(int id, int userId)
        {
            var password = await _context.PasswordVaults
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (password != null)
            {
                password.Password = CryptoHelper.DecryptString(password.Password);
            }

            return password;
        }

        public async Task<PasswordVault> AddPasswordAsync(PasswordVault passwordVault)
        {
            passwordVault.Password = CryptoHelper.EncryptString(passwordVault.Password);
            passwordVault.CreatedDate = DateTime.Now;

            await _context.PasswordVaults.AddAsync(passwordVault);
            await _context.SaveChangesAsync();

            return passwordVault;
        }

        public async Task<PasswordVault> UpdatePasswordAsync(PasswordVault passwordVault)
        {
            var existingPassword = await _context.PasswordVaults
                .FirstOrDefaultAsync(p => p.Id == passwordVault.Id && p.UserId == passwordVault.UserId);

            if (existingPassword == null)
                return null;

            existingPassword.SiteName = passwordVault.SiteName;
            existingPassword.Username = passwordVault.Username;
            existingPassword.Password = CryptoHelper.EncryptString(passwordVault.Password);
            existingPassword.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingPassword;
        }

        public async Task<bool> DeletePasswordAsync(int id, int userId)
        {
            var password = await _context.PasswordVaults
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (password == null)
                return false;

            _context.PasswordVaults.Remove(password);
            await _context.SaveChangesAsync();

            return true;
        }
    }
} 