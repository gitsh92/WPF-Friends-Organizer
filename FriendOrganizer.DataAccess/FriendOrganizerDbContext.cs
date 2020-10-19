using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess
{
  public class FriendOrganizerDbContext : DbContext
  {
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=FriendOrganizer.db");
      optionsBuilder.UseLazyLoadingProxies();
      base.OnConfiguring(optionsBuilder);
    }

  }
}
