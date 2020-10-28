using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IMessageDialogService _messageDialogService;
    private Func<IFriendDetailViewModel> _friendDetailViewModelCreator { get; }

    public INavigationViewModel NavigationViewModel { get; }

    private IFriendDetailViewModel _friendDetailViewModel;
    public IFriendDetailViewModel FriendDetailViewModel
    {
      get { return _friendDetailViewModel; }
      private set
      {
        _friendDetailViewModel = value;
        OnPropertyChanged();
      }
    }

    public ICommand CreateNewFriendCommand { get; }

    public MainViewModel(INavigationViewModel navigationViewModel,
      Func<IFriendDetailViewModel> friendDetailViewModelCreator,
      IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService)
    {
      _friendDetailViewModelCreator = friendDetailViewModelCreator;
      _eventAggregator = eventAggregator;
      _messageDialogService = messageDialogService;

      NavigationViewModel = navigationViewModel;

      CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);

      _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
        .Subscribe(OnOpenFriendDetailView);
      _eventAggregator.GetEvent<AfterFriendDeletedEvent>()
        .Subscribe(AfterFriendDeleted);
    }

    public async Task LoadAsync()
    {
      await NavigationViewModel.LoadAsync();
    }

    private async void OnOpenFriendDetailView(int? friendId)
    {
      if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
      {
        var result = _messageDialogService.ShowOkCancelDialog("You have unsaved changes. Do you want to navigate away?", "Question");
        if (result == MessageDialogResult.Cancel)
        {
          return;
        }
      }
      FriendDetailViewModel = _friendDetailViewModelCreator();
      await FriendDetailViewModel.LoadAsync(friendId);
    }

    private void OnCreateNewFriendExecute()
    {
      OnOpenFriendDetailView(null);
    }

    private void AfterFriendDeleted(int friendId)
    {
      FriendDetailViewModel = null;
    }
  }
}