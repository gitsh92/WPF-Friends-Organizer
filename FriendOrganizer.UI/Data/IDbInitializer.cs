namespace FriendOrganizer.UI.Data
{
  public interface IDbInitializer
  {
    void Initialize();

    IDbInitializer WithCommandLineArgs(string[] commandArgs);
  }
}