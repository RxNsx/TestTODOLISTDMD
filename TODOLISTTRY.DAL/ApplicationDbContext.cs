using Microsoft.EntityFrameworkCore;
using TODOLISTTRY.DAL.Model;

namespace TODOLISTTRY.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Do> AllDoes { get; set; }
    }
}
