using JobPortalAPI.API.Models;

namespace JobPortalAPI.API.Repositories.Interfaces;

public interface IApplicationRepository
{
    Task AddAsync(Application application);
    Task<Application?> GetByIdAsync(Guid id);
    Task<IEnumerable<Application>> GetByCandidateIdAsync(Guid candidateId);
    Task<Application?> GetByCandidateAndJobAsync(Guid candidateId, Guid jobId);
    Task<IEnumerable<Application>> GetByJobIdAsync(Guid jobId);
    Task DeleteAsync(Application application);
    Task SaveChangesAsync();

}