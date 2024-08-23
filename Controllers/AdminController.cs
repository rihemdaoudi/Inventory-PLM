using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Models;
using Inventory_PLM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Controllers
{

    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        

        public AdminController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
          
        }
      
        public async Task<IActionResult> DashboardAdmin()
        {
            var users = await _userService.GetAllUsersAsync();
            var userCountsByRole = users
                                   .GroupBy(u => u.Role)
                                   .ToDictionary(g => g.Key, g => g.Count());

            var categoriesCount = await _context.Categories.CountAsync();
            var subCategoriesCount = await _context.SubCategories.CountAsync();
            var unitsOfMeasureCount = await _context.UnitOfMeasures.CountAsync();

            var viewModel = new DashboardAdminViewModel
            {
                UserCountsByRole = userCountsByRole,
                CategoriesCount = categoriesCount,
                SubCategoriesCount = subCategoriesCount,
                UnitsOfMeasureCount = unitsOfMeasureCount
            };

            return View(viewModel);
            //return View(users);
        }
        public async Task<IActionResult> UserManagement()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {

            await _userService.CreateUserAsync(model);
            return RedirectToAction(nameof(UserManagement));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(model);
                return RedirectToAction(nameof(UserManagement));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(UserManagement));
        }
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Return user details as JSON
            return Json(new
            {
                Id = user.Id,
                First_Name = user.First_Name,
                Last_Name = user.Last_Name,
                Email = user.Email,
                Role = user.Role
                
            });
        }

    }
}


