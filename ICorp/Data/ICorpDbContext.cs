using PlanCorp.Areas.Master.Models;
using PlanCorp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PlanCorp.Data
{
    public class PlanCorpDbContext : IdentityDbContext
    {
        public PlanCorpDbContext(DbContextOptions<PlanCorpDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<TOKLANG_PEJABATFPP> TOKLANG_PEJABATFPP { get; set; }
        public virtual DbSet<UserInRole> UserInRoles { get; set; }
        public virtual DbSet<UserDetail> UserDetail { get; set; }
    }
}
