using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IUsersRepository
{
    Task Add(User user);
}
