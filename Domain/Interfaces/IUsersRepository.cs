using Domain.Entities;

namespace Domain.Interfaces;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetUserByUsername(string username);
}
