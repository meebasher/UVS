
using Microsoft.EntityFrameworkCore;
using UVS.Domain.Entities;

namespace UVS.Infra.Data.Context
{
    public class UVSDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public UVSDbContext(DbContextOptions<UVSDbContext> options) : base(options)
        {

        }
    }
}
