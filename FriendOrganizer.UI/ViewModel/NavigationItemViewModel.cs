using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
  public class NavigationItemViewModel : ViewModelBase
  {
    private readonly IEventAggregator _eventAggregator;

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

    public ICommand OpenFriendDetailViewCommand { get; }

    public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
    {
      _eventAggregator = eventAggregator;
      Id = id;
      DisplayMember = displayMember;
      OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
    }

    private void OnOpenFriendDetailView()
    {
      _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
            .Publish(Id);
    }
  }
}
