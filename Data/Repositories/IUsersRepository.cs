using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
}
