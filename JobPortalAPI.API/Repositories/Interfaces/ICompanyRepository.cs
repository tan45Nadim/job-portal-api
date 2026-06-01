using JobPortalAPI.API.Models;

namespace JobPortalAPI.API.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
    Task UpdateAsync(Company company);
    Task DeleteAsync(Company company);
    Task SaveChangesAsync();
}
