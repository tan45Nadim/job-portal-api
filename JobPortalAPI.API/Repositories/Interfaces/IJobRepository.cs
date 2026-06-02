using JobPortalAPI.API.Models;

namespace JobPortalAPI.API.Repositories.Interfaces;

public interface IJobRepository
{
    Task AddAsync(Job job);
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(Guid id);
    Task<IEnumerable<Job>> SearchAsync(string keyword);
    Task UpdateAsync(Job job);
    Task DeleteAsync(Job job);
    Task SaveChangesAsync();

    Task<Job?> GetByIdWithCompanyAsync(Guid id);

}
