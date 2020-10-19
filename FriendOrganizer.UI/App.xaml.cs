using Autofac;
using FriendOrganizer.UI.Startup;
using System.Windows;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI
{
  public partial class App : Application
  {
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      var bootstrapper = new Bootstrapper();
      var container = bootstrapper.Bootstrap();

      container.Resolve<IDbInitializer>()
          .WithCommandLineArgs(e.Args)
          .Initialize();

      var mainWindow = container.Resolve<MainWindow>();
      mainWindow.Show();
    }
  }
}
