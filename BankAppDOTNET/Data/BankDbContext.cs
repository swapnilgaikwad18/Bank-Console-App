using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAppDOTNET.Models;
using Microsoft.EntityFrameworkCore;
namespace BankAppDOTNET.Data
{

        public class BankDbContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public DbSet<Account> Accounts { get; set; }

        public DbSet<TransactionLog> TransactionLogs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Configure your database connection here (e.g., SQL Server)
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=BankAppDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Define any additional configurations for entities and relationships here

                // Configure the one-to-many relationship between User and Account
                modelBuilder.Entity<Account>()
                    .HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId);
            }
        }
    
}
