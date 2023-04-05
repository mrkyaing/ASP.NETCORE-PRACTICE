using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SFMS.Models.DAO{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<FinePolicy> FinePolicies { get; set; }
        public DbSet<FineTransaction> FineTransactions { get; set; }
        public DbSet<TeacherCourses> TeacherCourses { get; set; }
    }
}
