using System.ComponentModel.DataAnnotations;

namespace PlanCorp.Areas.Master.Models
{
    public class UserDetail
    {
        [Key]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int AccessFailedCount { get; set; }
        public int UserType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class PIC
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
