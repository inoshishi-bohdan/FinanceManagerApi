using FinanceManagerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Data
{
    public partial class FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCaregories { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Currency");
                entity.Property(e => e.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Expense");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Currency).WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expenses_Currencies");

                entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.ExpenseCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expenses_ExpenseCategory");

                entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expenses_Users");
            });

            modelBuilder.Entity<ExpenseCategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ExpenseCategory");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Currency).WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Incomes_Currencies");

                entity.HasOne(d => d.IncomeCategory).WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.IncomeCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Incomes_IncomeCategory");

                entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Incomes_Users");
            });

            modelBuilder.Entity<IncomeCategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_IncomeCategory");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_User"); 
                entity.HasIndex(e => e.UserName, "UC_Users").IsUnique();
                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.ProfileImage).WithMany(p => p.Users)
                    .HasForeignKey(d => d.ProfileImageId)
                    .HasConstraintName("FK_Users_ProfileImage");

            });

            modelBuilder.Entity<ProfileImage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ProfileImage"); 
                entity.Property(e => e.Path).HasMaxLength(50);
                entity.Property(e => e.Caption).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
