using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryIT.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthOfDate { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? FungsiId { get; set; }
        //public IdentityUser User { get; set; }
        public bool IsActive { get; set; }
    }

    public enum UserType
    {
        LDAP,
        SITE
    }
}
