﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
  public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
  {
    private readonly IFriendDataService _dataService;
    private readonly IEventAggregator _eventAggregator;

    private Friend _friend;

    public Friend Friend
    {
      get { return _friend; }
      private set
      {
        _friend = value;
        OnPropertyChanged();
      }
    }

    public FriendDetailViewModel(IFriendDataService dataService,
      IEventAggregator eventAggregator)
    {
      _dataService = dataService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
        .Subscribe(OnOpenFriendDetailView);
    }

    private async void OnOpenFriendDetailView(int friendId)
    {
      await LoadAsync(friendId);
    }

    public async Task LoadAsync(int friendId)
    {
      Friend = await _dataService.GetByIdAsync(friendId);
    }
  }
}
