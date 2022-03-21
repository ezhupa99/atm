using atm.Models;
using Microsoft.EntityFrameworkCore;

namespace atm.Data
{
    public class ComprogContext : DbContext
    {
        public ComprogContext(DbContextOptions<ComprogContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHistory> AccountHistories { get; set; }
        public DbSet<ATM> Atms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}