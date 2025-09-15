using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthentication.Models;

namespace RoleBasedAuthentication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TeacherClassModal> teacherClassMap { get; set; }
        public DbSet<ClassNameMapping> classNameMap { get; set; }
    }
}
