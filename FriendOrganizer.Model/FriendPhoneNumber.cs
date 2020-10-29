using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class FriendPhoneNumber
  {
    public int Id { get; set; }

    [Phone]
    [Required]
    public string Number { get; set; }
    
    public int FriendId { get; set; }

    public virtual Friend Friend { get; set; }
  }
}
