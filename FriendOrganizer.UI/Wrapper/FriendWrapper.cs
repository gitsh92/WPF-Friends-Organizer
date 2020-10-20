using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Wrapper
{
  public class FriendWrapper : ModelWrapper<Friend>
  {
    public FriendWrapper(Friend model) : base(model) { }

    public int Id { get => Model.Id; }

    public string FirstName
    {
      get => GetValue<string>();
      set => SetValue(value);
    }

    public string LastName
    {
      get => GetValue<string>();
      set => SetValue(value);
    }

    public string Email
    {
      get => GetValue<string>();
      set => SetValue(value);
    }

    protected override IEnumerable<string> ValidateProperty(string propertyName)
    {
      switch (propertyName)
      {
        case nameof(FirstName):
          if (string.IsNullOrEmpty(FirstName))
          {
            yield return "First name cannot be empty";
          }
          break;
      }
    }
  }
}