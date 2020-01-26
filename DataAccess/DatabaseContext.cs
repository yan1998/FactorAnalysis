using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<ExchangeRateFactors> ExchangeRateFactors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExchangeRateFactors>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<ExchangeRateFactors>()
                .HasIndex(x => x.Date)
                .IsUnique();
        }
    }
}
