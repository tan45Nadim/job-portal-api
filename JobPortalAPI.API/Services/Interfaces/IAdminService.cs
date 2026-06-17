using JobPortalAPI.API.DTOs.Admin;

namespace JobPortalAPI.API.Services.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<UserSummaryDto>> GetUsersAsync();
    Task<UserSummaryDto> GetUserAsync(Guid id);
    Task DeleteUserAsync(Guid id);
    Task DeleteCompanyAsync(Guid id);
    Task DeleteJobAsync(Guid id);
    Task<DashboardDto> GetDashboardAsync();
}
