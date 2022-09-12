using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTLicVerify.Models;

namespace WTLicVerify.DBContexts
{
    public class WTDBContext : DbContext
    {
        public DbSet<AuthorSale> AuthorSales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<EnvatoAccess> EnvatoAccesses { get; set; }

        public WTDBContext(DbContextOptions<WTDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<AuthorSale>().ToTable("AuthorSale");
            modelBuilder.Entity<SaleItem>().ToTable("SaleItem");
            modelBuilder.Entity<EnvatoAccess>().ToTable("EnvatoAccess");

            // Configure Primary Keys  
            modelBuilder.Entity<AuthorSale>().HasKey(u => u.Id).HasName("PK_AuthorSale");
            modelBuilder.Entity<SaleItem>().HasKey(u => u.Id).HasName("PK_SaleItem");
            modelBuilder.Entity<EnvatoAccess>().HasKey(u => u.Id).HasName("PK_EnvatoAccess");

            // Configure columns  
            modelBuilder.Entity<AuthorSale>().Property(u => u.Id).HasColumnType("bigint").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.license).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.sold_at).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.supported_until).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.support_amount).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.code).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<AuthorSale>().Property(ug => ug.email).HasColumnType("nvarchar(100)");

            modelBuilder.Entity<SaleItem>().Property(u => u.Id).HasColumnType("bigint").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<SaleItem>().Property(u => u.name).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<SaleItem>().Property(u => u.site).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<SaleItem>().Property(u => u.updated_at).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<SaleItem>().Property(u => u.url).HasColumnType("nvarchar(250)");
            modelBuilder.Entity<SaleItem>().Property(u => u.author_url).HasColumnType("nvarchar(200)");
            modelBuilder.Entity<SaleItem>().Property(u => u.author_username).HasColumnType("nvarchar(100)");

            modelBuilder.Entity<EnvatoAccess>().Property(u => u.Id).HasColumnType("bigint").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.access_token).HasColumnType("nvarchar(200)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.activationCode).HasColumnType("nvarchar(150)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.refresh_token).HasColumnType("nvarchar(200)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.token_type).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.application_name).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.application_version).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.extenstion_version).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.extenstion_name).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<EnvatoAccess>().Property(ug => ug.machine_name).HasColumnType("nvarchar(100)");

        }
    }
}
