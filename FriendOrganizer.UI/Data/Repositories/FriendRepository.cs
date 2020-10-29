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

    public void Add(Friend friend)
    {
      _context.Friends.Add(friend);
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

    public void Remove(Friend model)
    {
      _context.Friends.Remove(model);
    }

    public void RemovePhoneNumber(FriendPhoneNumber model)
    {
      _context.FriendPhoneNumbers.Remove(model);
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
