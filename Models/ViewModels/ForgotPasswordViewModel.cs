using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
