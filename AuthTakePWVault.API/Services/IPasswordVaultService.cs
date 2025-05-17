using AuthTakePWVault.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthTakePWVault.API.Services
{
    public interface IPasswordVaultService
    {
        Task<IEnumerable<PasswordVault>> GetUserPasswordsAsync(int userId);
        Task<PasswordVault> GetPasswordByIdAsync(int id, int userId);
        Task<PasswordVault> AddPasswordAsync(PasswordVault passwordVault);
        Task<PasswordVault> UpdatePasswordAsync(PasswordVault passwordVault);
        Task<bool> DeletePasswordAsync(int id, int userId);
    }
} 