using JobPortalAPI.API.Data;
using JobPortalAPI.API.Models;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.API.Repositories.Interfaces;

namespace JobPortalAPI.API.Repositories.Implementations;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _context;

    public ApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Application application)
    {
        await _context.Applications.AddAsync(application);
    }

    public async Task<Application?> GetByIdAsync(Guid id)
    {
        return await _context.Applications
            .Include(a => a.Job)
            .Include(a => a.Candidate)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Application>> GetByCandidateIdAsync(Guid candidateId)
    {
        return await _context.Applications
            .Include(a => a.Job)
            .Where(a => a.CandidateId == candidateId)
            .ToListAsync();
    }

    public async Task<Application?> GetByCandidateAndJobAsync(Guid candidateId, Guid jobId)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(a => a.CandidateId == candidateId && a.JobId == jobId);
    }

    public async Task<IEnumerable<Application>> GetByJobIdAsync(Guid jobId)
    {
        return await _context.Applications
            .Include(a => a.Candidate)
            .Where(a => a.JobId == jobId)
            .ToListAsync();
    }

    public async Task DeleteAsync(Application application)
    {
        _context.Applications.Remove(application);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}