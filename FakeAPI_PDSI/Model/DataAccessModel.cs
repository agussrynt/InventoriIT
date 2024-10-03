namespace FakeAPI_PDSI.Model
{
    public class DataAccessModel
    {
    }

  
    public class UserInRoleView 
    {
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? Fungsi { get; set; }
        public int? FungsiId { get; set; }
        public string? Role { get; set; }
        public string? RoleId { get; set; }
        public int UserType { get; set; }
        public int Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
