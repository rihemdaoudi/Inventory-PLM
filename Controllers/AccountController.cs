using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using Inventory_PLM.Models.ViewModels;
using System.Runtime.InteropServices;

namespace Inventory_PLM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public AccountController(IUserService userService, IEmailSender emailSender, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login model)
        {
            

            var user = _userService.ValidateUser(model.Email, model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, user.Email),
                  new Claim(ClaimTypes.Role, user.Role),
                  new Claim("FullName", user.First_Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToDashboard(user.Role);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }


        private IActionResult RedirectToDashboard(string role)
        {
            User user = new User();
            switch (role)
            {
                case "admin":
                    return RedirectToAction("DashboardAdmin", "Admin");
                case "StoreManager":
                    return RedirectToAction("DashboardStore", "StoreManager");
                case "Supervisor":
                    return RedirectToAction("DashboardSupervisor", "Supervisor");
                case "PurchasingManager":
                    return RedirectToAction("DashboardPurchasing", "PurchasingManager");
                default:
                    return RedirectToAction("Index", "Account");
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                var token = await _userService.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                var result = await _userService.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get the logged-in user's ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                First_Name = user.First_Name,
                Last_Name = user.Last_Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                Department = user.Department
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdString, out var userId))
                {
                    return BadRequest("Invalid user ID");
                }

                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                user.First_Name = model.First_Name;
                user.Last_Name = model.Last_Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;
                user.Address = model.Address;
                user.Department = model.Department;
                

                await _userService.UpdateUserAsync(user);

                TempData["SuccessMessage"] = "Profile updated successfully!";

                return RedirectToAction(nameof(Profile));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new SettingsViewModel
            {
                // Populate model with user settings
                // Example: NotificationPreferences = user.NotificationPreferences
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdString, out var userId))
                {
                    return BadRequest("Invalid user ID");
                }

                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                // Update user settings
                // Example: user.NotificationPreferences = model.NotificationPreferences;

                await _userService.UpdateUserAsync(user);

                TempData["SuccessMessage"] = "Settings updated successfully!";

                return RedirectToAction(nameof(Settings));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ActivityLog()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            var logs = await _userService.GetActivityLogsByUserIdAsync(userId);

            var model = new ActivityLogViewModel
            {
                Logs = logs
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }

        //[HttpGet]
        //public IActionResult ExternalLogin(string provider)
        //{
        //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpGet]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        //        return View(nameof(Index));
        //    }

        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (signInResult.Succeeded)
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    var user = new IdentityUser { UserName = email, Email = email };

        //    var result = await _userManager.CreateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        result = await _userManager.AddLoginAsync(user, info);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToLocal(returnUrl);
        //        }
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return View(nameof(Index));
        //}

        //private IActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(Index), "Home");
        //    }
        //}

    }
}
