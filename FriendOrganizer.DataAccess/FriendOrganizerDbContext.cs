﻿using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.DataAccess
{
  public class FriendOrganizerDbContext : DbContext
  {
    public DbSet<Friend> Friends { get; set; }

    public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

    public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      //string dbFile = "FriendOrganizer.db";
      //string exDir = AppDomain.CurrentDomain.BaseDirectory;
      //string dbPath = Path.Combine(exDir, dbFile);

      optionsBuilder.UseSqlServer(@"Server = (localdb)\MSSQLLocalDB; Database = FriendOrganizer; Trusted_Connection = True;");
      optionsBuilder.UseLazyLoadingProxies();
      base.OnConfiguring(optionsBuilder);
    }

  }
}
