using Microsoft.EntityFrameworkCore;
using Authentication_Service.Models;

namespace Authentication_Service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Staff> Staffs { get; set; }
}
