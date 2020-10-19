﻿using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace FriendOrganizer.DataAccess
{
  public class FriendOrganizerDbContext : DbContext
  {
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      string dbFile = "FriendOrganizer.db";
      string exDir = AppDomain.CurrentDomain.BaseDirectory;
      string dbPath = Path.Combine(exDir, dbFile);

      optionsBuilder.UseSqlite($"Data Source={dbPath}");
      optionsBuilder.UseLazyLoadingProxies();
      base.OnConfiguring(optionsBuilder);
    }

  }
}
