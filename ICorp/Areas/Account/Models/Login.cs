using System.ComponentModel.DataAnnotations;

namespace PlanCorp.Areas.Account.Models
{
    public class Login
    {
        [Required]
        //public string Username { get; set; }
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class LoginLDAP
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Method { get; set; }
    }
}
