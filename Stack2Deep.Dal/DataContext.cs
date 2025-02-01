using Microsoft.EntityFrameworkCore;

using Stack2Deep.Dal.Structures.Contexts;
using Stack2Deep.Dal.Structures.User;

namespace Stack2Deep.Dal;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<GroupProfile> Groups { get; set; }
    
    public DbSet<UserGroupContext> Contexts { get; set; }
}