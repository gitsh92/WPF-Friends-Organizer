using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class Friend
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "A first name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed {1} characters")]
    public string FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name cannot exceed {1} characters")]
    public string LastName { get; set; }

    [StringLength(50, ErrorMessage = "Email cannot exceed {1} characters")]
    [EmailAddress]
    public string Email { get; set; }
  }
}
