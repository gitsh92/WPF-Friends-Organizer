﻿using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

    public async Task<IEnumerable<Friend>> GetAllAsync()
    {
      using (var ctx = _contextCreator())
      {
        return await ctx.Friends.AsNoTracking().ToListAsync();
      }
    }
  }
}
