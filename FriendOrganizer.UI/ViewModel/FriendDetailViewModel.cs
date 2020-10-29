using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
  public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
  {
    private readonly IFriendRepository _friendRepository;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMessageDialogService _messageDialogService;
    private readonly IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;

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

    private FriendPhoneNumberWrapper _seletedPhoneNumber;
    public FriendPhoneNumberWrapper SelectedPhoneNumber
    {
      get => _seletedPhoneNumber;
      set
      {
        _seletedPhoneNumber = value;
        OnPropertyChanged();
        ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

    public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

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
    public ICommand AddPhoneNumberCommand { get; }
    public ICommand RemovePhoneNumberCommand { get; }

    public FriendDetailViewModel(IFriendRepository friendRepository,
      IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService,
      IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
    {
      _friendRepository = friendRepository;
      _eventAggregator = eventAggregator;
      _messageDialogService = messageDialogService;
      _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

      SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
      DeleteCommand = new DelegateCommand(OnDeleteExecute);
      AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
      RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

      ProgrammingLanguages = new ObservableCollection<LookupItem>();
      PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
    }

    public async Task LoadAsync(int? friendId)
    {
      var friend = friendId.HasValue
        ? await _friendRepository.GetByIdAsync(friendId.Value)
        : CreateNewFriend();

      InitializeFriend(friend);

      InitializeFriendPhoneNumbers(friend.PhoneNumbers);

      await LoadProgrammingLanguagesLookupAsync();
    }

    private void InitializeFriend(Friend friend)
    {
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

    private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
    {
      foreach (var wrapper in PhoneNumbers)
      {
        wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
      }
      PhoneNumbers.Clear();
      foreach (var friendPhoneNumber in phoneNumbers)
      {
        var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
        PhoneNumbers.Add(wrapper);
        wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
      }
    }

    private async Task LoadProgrammingLanguagesLookupAsync()
    {
      ProgrammingLanguages.Clear();
      ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "-" });
      var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
      foreach (var lookupItem in lookup)
      {
        ProgrammingLanguages.Add(lookupItem);
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
      return Friend != null
        && !Friend.HasErrors
        && PhoneNumbers.All(pn => !pn.HasErrors)
        && HasChanges;
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

    private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!HasChanges)
      {
        HasChanges = _friendRepository.HasChanges();
      }
      if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
      {
        ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
      }
    }

    private bool OnRemovePhoneNumberCanExecute()
    {
      return SelectedPhoneNumber != null;
    }

    private void OnRemovePhoneNumberExecute()
    {
      SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
      //Friend.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
      _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
      PhoneNumbers.Remove(SelectedPhoneNumber);
      SelectedPhoneNumber = null;
      HasChanges = _friendRepository.HasChanges();
      ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
    }

    private void OnAddPhoneNumberExecute()
    {
      var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
      newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
      PhoneNumbers.Add(newNumber);
      Friend.Model.PhoneNumbers.Add(newNumber.Model);
      newNumber.Number = ""; // Triggers validation
    }
  }
}
