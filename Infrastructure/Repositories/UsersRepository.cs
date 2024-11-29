using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

internal class UsersRepository(ChatDbContext dbContext) : IUsersRepository
{
    public async Task Add(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}
