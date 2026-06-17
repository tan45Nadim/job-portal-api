using JobPortalAPI.API.Models;

namespace JobPortalAPI.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task DeleteAsync(User user);
    Task SaveChangesAsync();
    Task<int> GetUserCountAsync();
    Task<int> GetUserCountByRoleAsync(string role);

}