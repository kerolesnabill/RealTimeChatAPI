﻿using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

internal class UsersRepository(RealTimeChatDbContext dbContext) : IUsersRepository
{
    public async Task Add(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }
}
