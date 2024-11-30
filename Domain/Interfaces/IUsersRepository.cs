using Domain.Entities;

namespace Domain.Interfaces;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
}
