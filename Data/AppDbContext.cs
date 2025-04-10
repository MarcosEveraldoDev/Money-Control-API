using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //public DbSet<Business> Business { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<SaleItem> SaleItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey }); // Configura chave composta

            modelBuilder.Entity<Products>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Products>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Products>()
                .Property(p => p.Description)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Products>()
                .Property(p => p.Category)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasPrecision(10, 2)
                .IsRequired();

            modelBuilder.Entity<Products>()
                .Property(p => p.Quantity)
                .IsRequired();

            modelBuilder.Entity<Products>()
                .Property(p => p.Unity)
                .HasMaxLength(2)
                .IsRequired();

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sales");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.TotalPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(s => s.Status)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(s => s.SaleDate)
                    .HasColumnType("datetime2")
                    .IsRequired();
                entity.HasMany(s => s.Items)
                    .WithOne(si => si.Sale)
                    .HasForeignKey(si => si.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.ToTable("SaleItems");
                entity.HasKey(si => si.Id);
                entity.Property(si => si.Quantity)
                    .IsRequired();
                entity.Property(si => si.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(si => si.TotalPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.HasOne(si => si.Product)
                    .WithMany()
                    .HasForeignKey(si => si.ProductId)
                    .IsRequired();
            });
        }

    }
}
