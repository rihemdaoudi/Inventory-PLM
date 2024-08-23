using Elfie.Serialization;
using Inventory_PLM.Enums;
using Inventory_PLM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Inventory_PLM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PurchaseProposal> PurchaseProposals { get; set; }
        public DbSet <ProposalReview> ProposalReviews { get; set; }
        public DbSet <PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships if needed
            modelBuilder.Entity<SalesOrder>()
                .HasMany(s => s.Items)
                .WithOne(i => i.SalesOrder)
                .HasForeignKey(i => i.SalesOrderID);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(p => p.PurchaseOrderItems)
                .WithOne(i => i.PurchaseOrder)
                .HasForeignKey(i => i.PurchaseOrderID);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryID);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany()
                .HasForeignKey(p => p.SubCategoryID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.UnitOfMeasure)
                .WithMany()
                .HasForeignKey(p => p.UnitOfMeasureID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryID);

            // Configure decimal properties
            modelBuilder.Entity<SalesOrderItem>()
                .Property(p => p.UnitPriceExclTax)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SalesOrderItem>()
                .Property(p => p.UnitPriceInclTax)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SalesOrderItem>()
                .Property(p => p.VAT)
                .HasColumnType("decimal(18, 2)");

            //modelBuilder.Entity<SalesOrderItem>()
            //    .Property(p => p.TotalPrice)
            //    .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Inventory>()
                .Property(i => i.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Inventory>()
                .Property(i => i.TotalPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPriceExclTax)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPriceInclTax)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.VAT)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SalesOrder>()
                 .Property(so => so.Status)
                 .HasConversion(
                  v => v.HasValue ? (int)v.Value : (int?)null,
                  v => v.HasValue ? (OrderStatus)v.Value : (OrderStatus?)null
                  );

            modelBuilder.Entity<SalesOrder>()
                .Property(so => so.PaymentStatus)
                .HasConversion(
                    v => v.HasValue ? (int)v.Value : (int?)null,
                    v => v.HasValue ? (PaymentStatus)v.Value : (PaymentStatus?)null
                );
        }

    }
}
