using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FriendOrganizer.UI.Data
{
  public class DbInitializer : IDbInitializer
  {
    private readonly Func<FriendOrganizerDbContext> _contextCreator;
    private readonly List<string> _args;

    public DbInitializer(Func<FriendOrganizerDbContext> contextCreator)
    {
      _contextCreator = contextCreator;
      _args = new List<string>();
    }

    public IDbInitializer WithCommandLineArgs(string[] commandArgs)
    {
      _args.AddRange(commandArgs);
      return this;
    }

    public void Initialize()
    {
      if (_args.Any(a => a.ToLowerInvariant() == "--seed-db"))
        seedDb();
      else if (_args.Any(a => a.ToLowerInvariant() == "--delete-db"))
        removeDb();
      else
        return; // no shutdown

      Application.Current.Shutdown();
    }

    private void seedDb()
    {
      using (var conn = _contextCreator())
      {
        conn.Database.EnsureDeleted();
        conn.Database.Migrate();

        conn.Friends.AddRange(
          new Friend { FirstName = "Stuart", LastName = "Huggins" },
          new Friend { FirstName = "Jordy", LastName = "Jurick" },
          new Friend { FirstName = "Squirrel", LastName = "Garden" },
          new Friend { FirstName = "Magpie", LastName = "Garden" }
        );
        conn.ProgrammingLanguages.AddRange(
          new ProgrammingLanguage { Name = "C#" },
          new ProgrammingLanguage { Name = "TypeScript" },
          new ProgrammingLanguage { Name = "C++" },
          new ProgrammingLanguage { Name = "Java" },
          new ProgrammingLanguage { Name = "Swift" }
        );
        conn.SaveChanges();

        conn.FriendPhoneNumbers.Add(
          new FriendPhoneNumber
          {
            Number = "+49 12345678",
            FriendId = conn.Friends.First().Id
          }
        );
        conn.SaveChanges();
      }
    }

    private void removeDb()
    {
      using (var conn = _contextCreator())
      {
        conn.Database.EnsureDeleted();
      }
    }
  }
}
