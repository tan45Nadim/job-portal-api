using JobPortalAPI.API.DTOs.Job;

namespace JobPortalAPI.API.Services.Interfaces;

public interface IJobService
{
    Task<JobResponseDto> CreateAsync(
        JobCreateDto dto,
        Guid ownerId);
    Task<IEnumerable<JobResponseDto>> GetAllAsync();
    Task<JobResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<JobResponseDto>> SearchAsync(string keyword);
    Task<JobResponseDto> UpdateAsync(Guid id,
        JobUpdateDto dto,
        Guid ownerId);
    Task DeleteAsync(Guid id, Guid ownerId);
}
