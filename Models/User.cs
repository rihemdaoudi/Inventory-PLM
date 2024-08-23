using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Max 100 characters are allowed")]
        [Display(Name = "First Name")]
        public string First_Name { get; set;}
        [Required]
        [StringLength(100, ErrorMessage = "Max 100 characters are allowed")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set;}
        public string Role { get; set; }
        public string Department { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordHash { get; set; }
        



    }
}
