using FinanceManagerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Data
{
    public class FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
