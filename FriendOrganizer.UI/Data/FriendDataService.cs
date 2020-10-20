using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
  public class FriendDataService : IFriendDataService
  {
    private readonly Func<FriendOrganizerDbContext> _contextCreator;
    public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
    {
      _contextCreator = contextCreator;
    }

    public async Task<Friend> GetByIdAsync(int friendId)
    {
      using (var ctx = _contextCreator())
      {
        return await ctx.Friends.AsNoTracking()
          .SingleOrDefaultAsync(f => f.Id == friendId);
      }
    }

    public async Task SaveAsync(Friend friend)
    {
      //validateFriend(friend);

      using (var ctx = _contextCreator())
      {
        ctx.Friends.Attach(friend);
        ctx.Entry(friend).State = EntityState.Modified;
        await ctx.SaveChangesAsync();
      }
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
