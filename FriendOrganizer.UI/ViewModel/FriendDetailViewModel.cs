using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
  public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
  {
    private readonly IFriendRepository _friendRepository;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMessageDialogService _messageDialogService;

    private FriendWrapper _friend;
    public FriendWrapper Friend
    {
      get { return _friend; }
      private set
      {
        _friend = value;
        OnPropertyChanged();
      }
    }

    private bool _hasChanges;

    public bool HasChanges
    {
      get { return _hasChanges; }
      set
      {
        if (_hasChanges != value)
        {
          _hasChanges = value;
          OnPropertyChanged();
          ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
      }
    }

    public ICommand SaveCommand { get; }

    public ICommand DeleteCommand { get; }

    public FriendDetailViewModel(IFriendRepository friendRepository,
      IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService)
    {
      _friendRepository = friendRepository;
      _eventAggregator = eventAggregator;
      _messageDialogService = messageDialogService;

      SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
      DeleteCommand = new DelegateCommand(OnDeleteExecute);
    }

    public async Task LoadAsync(int? friendId)
    {
      var friend = friendId.HasValue
        ? await _friendRepository.GetByIdAsync(friendId.Value)
        : CreateNewFriend();

      Friend = new FriendWrapper(friend);

      Friend.PropertyChanged += (s, e) =>
      {
        if (!HasChanges)
        {
          HasChanges = _friendRepository.HasChanges();
        }
        if (e.PropertyName == nameof(Friend.HasErrors))
        {
          ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
      };
      ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

      // if a new friend has been created
      if (Friend.Id == 0)
      {
        Friend.FirstName = ""; // triggers validation through SetValue
      }
    }

    private Friend CreateNewFriend()
    {
      var friend = new Friend();
      _friendRepository.Add(friend);
      return friend;
    }

    private bool OnSaveCanExecute()
    {
      return Friend != null && !Friend.HasErrors && HasChanges;
    }

    private async void OnSaveExecute()
    {
      await _friendRepository.SaveAsync();
      HasChanges = _friendRepository.HasChanges();
      _eventAggregator.GetEvent<AfterFriendSavedEvent>()
        .Publish(
        new AfterFriendSavedEventArgs
        {
          Id = Friend.Id,
          DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
        });
    }

    private async void OnDeleteExecute()
    {
      var result = _messageDialogService.ShowOkCancelDialog($"Are you sure you want to delete {Friend.FirstName} {Friend.LastName}?",
        "Confirm action");
      if (result == MessageDialogResult.OK)
      {
        _friendRepository.Remove(Friend.Model);
        await _friendRepository.SaveAsync();
        _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
      }
    }
  }
}
