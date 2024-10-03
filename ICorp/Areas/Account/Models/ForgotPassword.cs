using System.ComponentModel.DataAnnotations;

namespace PlanCorp.Areas.Account.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
