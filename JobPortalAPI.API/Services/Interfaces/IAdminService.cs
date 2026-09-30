using JobPortalAPI.API.DTOs.Admin;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.DTOs.Company;
using JobPortalAPI.API.DTOs.Job;

namespace JobPortalAPI.API.Services.Interfaces;

public interface IAdminService
{
    // Users
    Task<IEnumerable<UserSummaryDto>> GetUsersAsync();
    Task<UserSummaryDto> GetUserAsync(Guid id);

    Task ActivateUserAsync(Guid id);
    Task DeactivateUserAsync(Guid id, Guid currentAdminId);
    Task DeleteUserAsync(Guid id, Guid currentAdminId);

    // Companies
    Task<IEnumerable<CompanyResponseDto>> GetCompaniesAsync();
    Task DeleteCompanyAsync(Guid id);

    // Jobs
    Task<IEnumerable<JobResponseDto>> GetJobsAsync();
    Task DeleteJobAsync(Guid id);

    // Applications
    Task<IEnumerable<ApplicationResponseDto>> GetApplicationsAsync();
    Task UpdateApplicationStatusAsync(
        Guid id,
        UpdateApplicationStatusDto dto);

    // Dashboard
    Task<DashboardDto> GetDashboardAsync();
}