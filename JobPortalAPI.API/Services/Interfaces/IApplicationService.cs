using JobPortalAPI.API.DTOs.Application;

namespace JobPortalAPI.API.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponseDto> ApplyAsync(
        Guid candidateId, ApplicationCreateDto applicationCreateDto);
    Task<IEnumerable<ApplicationResponseDto>> GetMyApplicationsAsync(
        Guid candidateId);
    Task<IEnumerable<ApplicationResponseDto>> GetApplicantsAsync(
        Guid jobId, Guid employerId);

    Task WithdrawAsync(Guid applicationId, Guid candidateId);

}
