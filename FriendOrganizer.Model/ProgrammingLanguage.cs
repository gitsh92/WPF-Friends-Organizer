using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
  public class ProgrammingLanguage
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
  }
}