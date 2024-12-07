using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

internal class UsersRepository(RealTimeChatDbContext dbContext) : IUsersRepository
{
    public async Task Add(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}
