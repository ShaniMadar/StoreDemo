using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StoreDemoTest.Entities
{
    public partial class StoreDemoTestContext : DbContext
    {
        public StoreDemoTestContext()
        {
        }

        public StoreDemoTestContext(DbContextOptions<StoreDemoTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public virtual DbSet<PurchasePayment> PurchasePayment { get; set; }
        public virtual DbSet<PurchaseStatusType> PurchaseStatusType { get; set; }
        public virtual DbSet<Returns> Returns { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=StoreDemoTest;Integrated Security=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<EmployeeLogin>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Password)
            //        .IsRequired()
            //        .HasMaxLength(18)
            //        .IsUnicode(false);

            //    entity.HasOne(d => d.Employee)
            //        .WithMany(p => p.EmployeeLogin)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__EmployeeL__Emplo__5070F446");
            //});

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK__Inventory__Emplo__4316F928");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Item__4222D4EF");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemType).HasColumnName("itemType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.ItemTypeNavigation)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ItemType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Items__itemType__36B12243");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnPeriod).HasColumnName("returnPeriod");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.TotalSum).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.Employee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__Employ__45F365D3");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__Status__46E78A0C");
            });

            modelBuilder.Entity<PurchaseDetails>(entity =>
            {
                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseDe__Item__628FA481");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseD__Purch__619B8048");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseD__Statu__6383C8BA");
            });

            modelBuilder.Entity<PurchasePayment>(entity =>
            {
                entity.Property(e => e.Sum).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.PaymentMethodNavigation)
                    .WithMany(p => p.PurchasePayment)
                    .HasForeignKey(d => d.PaymentMethod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseP__Payme__5DCAEF64");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchasePayment)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseP__Purch__5CD6CB2B");
            });

            modelBuilder.Entity<PurchaseStatusType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Returns>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.CreditTypeNavigation)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.CreditType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Returns__CreditT__6D0D32F4");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.Employee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Returns__Employe__6E01572D");

                entity.HasOne(d => d.PurchaseDetail)
                    .WithMany(p => p.Returns)
                    .HasForeignKey(d => d.PurchaseDetailId)
                    .HasConstraintName("FK__Returns__Purchas__72C60C4A");
            });
        }
    }
}
