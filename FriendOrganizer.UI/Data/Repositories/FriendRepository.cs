using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
  public class FriendRepository : IFriendRepository
  {
    private readonly FriendOrganizerDbContext _context;
    public FriendRepository(FriendOrganizerDbContext context)
    {
      _context = context;
    }

    public async Task<Friend> GetByIdAsync(int friendId)
    {
      return await _context.Friends
        .SingleOrDefaultAsync(f => f.Id == friendId);
    }

    public bool HasChanges()
    {
      return _context.ChangeTracker.HasChanges();
    }

    public async Task SaveAsync()
    {
      //validateFriend(friend);

      await _context.SaveChangesAsync();
    }

    //// Less than ideal to separate validation from the model definition,
    //// but the Required attribute of EFCore does not reject empty strings
    //// so this manual check is needed
    //private void validateFriend(Friend friend)
    //{
    //  if (string.IsNullOrEmpty(friend.FirstName))
    //    throw new ValidationException("FirstName is required.");
    //}
  }
}
