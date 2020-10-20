using Autofac;
using FriendOrganizer.UI.Startup;
using System.Windows;
using FriendOrganizer.UI.Data;
using System;

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

    private void Application_DispatcherUnhandledException(object sender,
      System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      string msg = "An unexpected error occurred."
        + Environment.NewLine + e.Exception.Message;

      MessageBox.Show(msg, "Application error");

      e.Handled = true;
    }
  }
}
