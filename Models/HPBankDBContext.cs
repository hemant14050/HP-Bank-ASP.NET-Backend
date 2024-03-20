using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HPBank.Models
{
    public partial class HPBankDBContext : DbContext
    {
        private readonly IConfiguration _config;
        public HPBankDBContext()
        {
        }

        public HPBankDBContext(DbContextOptions<HPBankDBContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountTransaction> AccountTransactions { get; set; } = null!;
        public virtual DbSet<AccountType> AccountTypes { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("HPBankDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNo)
                    .HasName("PK__Account__F267E8253EB3B4DD");

                entity.ToTable("Account");

                entity.HasIndex(e => e.CustomerId, "UQ__Account__B611CB7C5C4DBACC")
                    .IsUnique();

                entity.Property(e => e.AccountNo).HasColumnName("accountNo");

                entity.Property(e => e.AccountTypeId).HasColumnName("accountTypeId");

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("balance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_accountTypeId");

                entity.HasOne(d => d.Customer)
                    .WithOne(p => p.Account)
                    .HasForeignKey<Account>(d => d.CustomerId)
                    .HasConstraintName("fk_customerId");
            });

            modelBuilder.Entity<AccountTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__AccountT__9B57CF722278C0CC");

                entity.ToTable("AccountTransaction");

                entity.Property(e => e.TransactionId).HasColumnName("transactionId");

                entity.Property(e => e.AccountNo).HasColumnName("accountNo");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.ClosingAmt)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("closingAmt");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("transactionType");

                entity.HasOne(d => d.AccountNoNavigation)
                    .WithMany(p => p.AccountTransactions)
                    .HasForeignKey(d => d.AccountNo)
                    .HasConstraintName("fk_accountNo");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("AccountType");

                entity.HasIndex(e => e.AccountType1, "UC_AccountType_AccountType")
                    .IsUnique();

                entity.Property(e => e.AccountTypeId).HasColumnName("accountTypeId");

                entity.Property(e => e.AccountType1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("accountType");

                entity.Property(e => e.InterestRate)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("interestRate");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.PanNo, "UQ__Customer__08ECE43E2B3EA6E4")
                    .IsUnique();

                entity.HasIndex(e => e.AddharNo, "UQ__Customer__121A24978692A1CA")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Customer__AB6E6164152C45ED")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.AddharNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("addharNo");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cust_state");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mobileNo");

                entity.Property(e => e.PanNo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("panNo");

                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("street");

                entity.Property(e => e.Zip).HasColumnName("zip");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
