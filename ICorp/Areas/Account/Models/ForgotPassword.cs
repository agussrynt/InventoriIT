using System.ComponentModel.DataAnnotations;

namespace InventoryIT.Areas.Account.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
