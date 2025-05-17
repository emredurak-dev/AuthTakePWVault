using AuthTakePWVault.Data.Entities;
using System.Threading.Tasks;

namespace AuthTakePWVault.API.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<User> AuthenticateAsync(string username, string password);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<bool> IsEmailUniqueAsync(string email);
    }
} 