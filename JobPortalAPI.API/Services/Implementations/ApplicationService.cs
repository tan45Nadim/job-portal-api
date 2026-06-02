using AutoMapper;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;

namespace JobPortalAPI.API.Services.Implementations;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IJobRepository _jobsRepository;
    private readonly IMapper _mapper;

    public ApplicationService(IApplicationRepository applicationRepository, IJobRepository jobsRepository, IMapper mapper)
    {
        _applicationRepository = applicationRepository;
        _jobsRepository = jobsRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResponseDto> ApplyAsync(
        Guid candidateId, ApplicationCreateDto dto)
    {
        var job = await _jobsRepository.GetByIdWithCompanyAsync(dto.JobId);

        if (job == null)
            throw new Exception("Job not found");

        if (!job.IsActive)
            throw new Exception("Job is not active");

        if (job.Deadline < DateTime.UtcNow)
            throw new Exception("Application deadline has passed");

        var existingApplication =
            await _applicationRepository
                .GetByCandidateAndJobAsync(candidateId, dto.JobId);

        if (existingApplication != null)
            throw new Exception("You have already applied for this job");

        var application = _mapper.Map<Application>(dto);

        application.CandidateId = candidateId;

        await _applicationRepository.AddAsync(application);
        await _applicationRepository.SaveChangesAsync();

        application.Job = job; // Include job details in the response

        return _mapper.Map<ApplicationResponseDto>(application);

    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetMyApplicationsAsync(
        Guid candidateId)
    {
        var applications = await _applicationRepository
            .GetByCandidateIdAsync(candidateId);

        return _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetApplicantsAsync(
        Guid jobId, Guid employerId)
    {
        var job = await _jobsRepository.GetByIdWithCompanyAsync(jobId);

        if (job == null)
            throw new Exception("Job not found");

        if (job.Company.OwnerId != employerId)
            throw new Exception(
                "You do not have permission to view applications for this job");


        var applications = await _applicationRepository
            .GetByJobIdAsync(jobId);

        return _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
    }

    public async Task WithdrawAsync(Guid applicationId, Guid candidateId)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
            throw new Exception("Application not found");

        if (application.CandidateId != candidateId)
            throw new Exception("You do not have permission to withdraw this application");

        await _applicationRepository.DeleteAsync(application);
        await _applicationRepository.SaveChangesAsync();
    }

}