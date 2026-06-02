using JobPortalAPI.API.Data;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.API.Repositories.Implementations;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;
    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
    }

    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        return await _context.Jobs
            .Include(j => j.Company)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(Guid id)
    {
        return await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<IEnumerable<Job>> SearchAsync(string keyword)
    {
        keyword = keyword.ToLower();

        return await _context.Jobs
            .Include(j => j.Company)
            .Where(j =>
                j.Title.ToLower().Contains(keyword) ||
                j.Description.ToLower().Contains(keyword) ||
                j.Company.Name.ToLower().Contains(keyword))
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Jobs.Update(job);
    }

    public async Task DeleteAsync(Job job)
    {
        _context.Jobs.Remove(job);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Job?> GetByIdWithCompanyAsync(Guid id)
    {
        return await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

}
