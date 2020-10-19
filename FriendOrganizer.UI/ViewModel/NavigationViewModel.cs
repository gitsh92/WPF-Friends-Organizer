using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
  public class NavigationViewModel : ViewModelBase, INavigationViewModel
  {
    private readonly IFriendLookupDataService _friendLookupService;
    private readonly IEventAggregator _eventAggregator;

    public ObservableCollection<LookupItem> Friends { get; }

    private LookupItem _selectedFriend;

    public LookupItem SelectedFriend
    {
      get { return _selectedFriend; }
      set
      {
        _selectedFriend = value;
        OnPropertyChanged();
        if (_selectedFriend != null)
        {
          _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
            .Publish(_selectedFriend.Id);
        }
      }
    }

    public NavigationViewModel(IFriendLookupDataService friendLookupService,
      IEventAggregator eventAggregator)
    {
      _friendLookupService = friendLookupService;
      _eventAggregator = eventAggregator;
      Friends = new ObservableCollection<LookupItem>();
    }

    public async Task LoadAsync()
    {
      var lookup = await _friendLookupService.GetFriendsLookupAsync();
      Friends.Clear();
      foreach (var item in lookup)
      {
        Friends.Add(item);
      }
    }
  }
}
