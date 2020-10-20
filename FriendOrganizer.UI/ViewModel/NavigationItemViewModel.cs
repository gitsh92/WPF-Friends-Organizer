namespace FriendOrganizer.UI.ViewModel
{
  public class NavigationItemViewModel : ViewModelBase
  {
    public int Id { get; }

    private string _displayMember;
    public string DisplayMember
    {
      get { return _displayMember; }
      set
      {
        _displayMember = value;
        OnPropertyChanged();
      }
    }

    public NavigationItemViewModel(int id, string displayMember)
    {
      Id = id;
      DisplayMember = displayMember;
    }
  }
}
