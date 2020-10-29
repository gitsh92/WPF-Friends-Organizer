using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class Friend
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "A first name is required")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed {1} characters")]
    public string FirstName { get; set; }

    [MaxLength(50, ErrorMessage = "Last name cannot exceed {1} characters")]
    public string LastName { get; set; }

    [MaxLength(50, ErrorMessage = "Email cannot exceed {1} characters")]
    [EmailAddress]
    public string Email { get; set; }

    public int? FavoriteLanguageId { get; set; }

    public virtual ProgrammingLanguage FavoriteLanguage { get; set; }

    public virtual ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
      = new Collection<FriendPhoneNumber>();

  }
}
