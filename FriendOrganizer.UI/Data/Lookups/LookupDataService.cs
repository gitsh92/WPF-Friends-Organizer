using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Lookups
{
  public class LookupDataService : IFriendLookupDataService
  {
    private readonly Func<FriendOrganizerDbContext> _contextCreator;

    public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
    {
      _contextCreator = contextCreator;
    }

    public async Task<IEnumerable<LookupItem>> GetFriendsLookupAsync()
    {
      using (var ctx = _contextCreator())
      {
        return await ctx.Friends.AsNoTracking()
          .Select(f =>
          new LookupItem
          {
            Id = f.Id,
            DisplayMember = $"{f.FirstName} {f.LastName}"
          })
          .ToListAsync();
      }
    }
  }
}
