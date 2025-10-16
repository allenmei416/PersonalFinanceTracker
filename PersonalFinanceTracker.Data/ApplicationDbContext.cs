using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data.Models;

namespace PersonalFinanceTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
