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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Food" },
                new Category { CategoryId = 2, Name = "Transport" },
                new Category { CategoryId = 3, Name = "Entertainment" },
                new Category { CategoryId = 4, Name = "Bills" },
                new Category { CategoryId = 5, Name = "Salary" },
                new Category { CategoryId = 6, Name = "Health" },
                new Category { CategoryId = 7, Name = "Shopping" },
                new Category { CategoryId = 8, Name = "Others" }
            );
        }
    }
}
