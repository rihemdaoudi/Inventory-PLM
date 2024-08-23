using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public UserService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public User ValidateUser(string email, string password)
        {
           
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            return await _userManager.GeneratePasswordResetTokenAsync(identityUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            return await _userManager.ResetPasswordAsync(identityUser, token, newPassword);
        }

        // New methods for Profile and Settings

        public async Task<User> GetUserProfileAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserProfileAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // New method for Activity Log

        public async Task<IEnumerable<ActivityLog>> GetActivityLogsByUserIdAsync(int userId)
        {
            return await _context.ActivityLogs
                .Where(log => log.UserId == userId).ToListAsync();
        }
    }

}
