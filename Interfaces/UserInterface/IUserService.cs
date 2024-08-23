using Inventory_PLM.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_PLM.Interfaces.UserInterface
{
    public interface IUserService
    {
        User ValidateUser(string email, string password);

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> FindByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);

        // Methods for Profile and Settings
        Task<User> GetUserProfileAsync(string email);
        Task UpdateUserProfileAsync(User user);

        // Method for Activity Log
        Task<IEnumerable<ActivityLog>> GetActivityLogsByUserIdAsync(int id);
    }

}
