using JobPortalAPI.API.DTOs.Company;

namespace JobPortalAPI.API.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto> CreateAsync(
        CompanyCreateDto dto,
        Guid ownerId);

    Task<IEnumerable<CompanyResponseDto>> GetAllAsync();

    Task<CompanyResponseDto?> GetByIdAsync(Guid id);

    Task UpdateAsync(Guid id,
        UpdateCompanyDto dto,
        Guid ownerId);

    Task DeleteAsync(Guid id, Guid ownerId);
}
