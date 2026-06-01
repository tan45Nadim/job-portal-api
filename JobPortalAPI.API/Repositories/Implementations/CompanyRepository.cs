using JobPortalAPI.API.Data;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.API.Repositories.Implementations;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;
    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
    }

    public async Task DeleteAsync(Company company)
    {
        _context.Companies.Remove(company);

    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
