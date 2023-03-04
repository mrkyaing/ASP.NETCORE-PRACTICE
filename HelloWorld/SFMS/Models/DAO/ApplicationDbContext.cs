using Microsoft.EntityFrameworkCore;
namespace SFMS.Models.DAO
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
