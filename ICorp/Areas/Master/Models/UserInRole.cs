using System.ComponentModel.DataAnnotations;

namespace InventoryIT.Areas.Master.Models
{
    public class UserInRole
    {
        [Key]
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



        //UserID
        //Email
        //UserName
        //FullName
        //Fungsi
        //Role
        //UserType
        //Active
        //CreatedAt
        //CreatedBy
        //UpdatedAt
        //UpdatedBy
    }

    public class UserInRoleView : UserInRole
    {
        public bool IsActive { get; set; }
    }

    public class ActiveteUser
    {
        public string UserName { get; set; }
        public bool IsActivate { get; set; }
    }
    public class AddUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string[] Roles { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string  IdUser { get; set; }
        public int  IdProfile { get; set; }
        public int FungsiId { get; set; }


        //New columns
        //public string EmpNumber { get; set; }
        //public string FullName { get; set; }
        //public string PosID { get; set; }
        //public string PosName { get; set; }
        //public string DirID { get; set; }
        //public string DirName { get; set; }
        //public string DivID { get; set; }
        //public string DivName { get; set; }
        //public string DepID { get; set; }
        //public string DepName { get; set; }
        //public string IsMitra { get; set; }
        //public string IsPDSI { get; set; }
    }

    public class AspNetUserRoles
    {
        public string  UserId { get; set; }
        public string  RoleId { get; set; }
    }
}
