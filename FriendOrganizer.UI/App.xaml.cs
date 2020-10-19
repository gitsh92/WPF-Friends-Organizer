using Autofac;
using Castle.Core.Internal;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Startup;
using FriendOrganizer.UI.ViewModel;
using System.Windows;
using System.Linq;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace FriendOrganizer.UI
{
  public partial class App : Application
  {
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      #region Seed DB
      // seed DB with dummy data here
      bool initDb = e.Args.Any(s => s.ToLowerInvariant() == "--initdb");
      if (initDb)
      {
        using (var conn = new FriendOrganizerDbContext())
        {
          conn.Database.EnsureDeleted();
          conn.Database.Migrate();
          conn.Friends.AddRange(
            new Friend { FirstName = "Stuart", LastName = "Huggins" },
            new Friend { FirstName = "Jordy", LastName = "Jurick" },
            new Friend { FirstName = "Squirrel", LastName = "Garden" },
            new Friend { FirstName = "Magpie", LastName = "Garden" }
          );
          conn.SaveChanges();
        }

        Shutdown();
      }
      #endregion

      var bootstrapper = new Bootstrapper();
      var container = bootstrapper.Bootstrap();

      var mainWindow = container.Resolve<MainWindow>();
      mainWindow.Show();
    }
  }
}
