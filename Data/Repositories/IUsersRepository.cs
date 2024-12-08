using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetByIdAsync(Guid id);
}
