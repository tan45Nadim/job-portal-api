using AutoMapper;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Admin;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.DTOs.Company;
using JobPortalAPI.API.DTOs.Job;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.API.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminService> _logger;

    public AdminService(
        IUserRepository userRepository,
        ICompanyRepository companyRepository,
        IJobRepository jobRepository,
        IApplicationRepository applicationRepository,
        IMapper mapper,
        ILogger<AdminService> logger)
    {
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _jobRepository = jobRepository;
        _applicationRepository = applicationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // =========================================================
    // USERS
    // =========================================================

    public async Task<IEnumerable<UserSummaryDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
    }

    public async Task<UserSummaryDto> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with ID {id} not found.");
        }

        return _mapper.Map<UserSummaryDto>(user);
    }

    public async Task ActivateUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with ID {id} not found.");
        }

        if (user.IsActive)
        {
            throw new BadRequestException(
                "User account is already active.");
        }

        user.IsActive = true;

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Admin activated user {UserId}",
            user.Id);
    }

    public async Task DeactivateUserAsync(Guid id, Guid currentAdminId)
    {
        if (id == currentAdminId)
            throw new BadRequestException(
                "An admin cannot deactivate their own account.");

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException(
                $"User with ID {id} not found.");

        if (!user.IsActive)
            throw new BadRequestException(
                "User account is already inactive.");

        user.IsActive = false;

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Admin deactivated user {UserId}",
            user.Id);
    }

    public async Task DeleteUserAsync(
        Guid id,
        Guid currentAdminId)
    {
        if (id == currentAdminId)
        {
            throw new BadRequestException(
                "An admin cannot delete their own account.");
        }

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with ID {id} not found.");
        }

        try
        {
            await _userRepository.DeleteAsync(user);

            await _userRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Admin deleted user {UserId}",
                user.Id);
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning(
                "Admin could not delete user {UserId} because related records exist.",
                user.Id);

            throw new BadRequestException(
                "This user cannot be deleted because they have related companies or applications. Deactivate the account instead.");
        }
    }

    // =========================================================
    // COMPANIES
    // =========================================================

    public async Task<IEnumerable<CompanyResponseDto>> GetCompaniesAsync()
    {
        var companies = await _companyRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);
    }

    public async Task DeleteCompanyAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
        {
            throw new NotFoundException(
                $"Company with ID {id} not found.");
        }

        await _companyRepository.DeleteAsync(company);

        await _companyRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Admin deleted company {CompanyId}",
            company.Id);
    }

    // =========================================================
    // JOBS
    // =========================================================

    public async Task<IEnumerable<JobResponseDto>> GetJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<JobResponseDto>>(jobs);
    }

    public async Task DeleteJobAsync(Guid id)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
        {
            throw new NotFoundException(
                $"Job with ID {id} not found.");
        }

        await _jobRepository.DeleteAsync(job);

        await _jobRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Admin deleted job {JobId}",
            job.Id);
    }

    // =========================================================
    // APPLICATIONS
    // =========================================================

    public async Task<IEnumerable<ApplicationResponseDto>>
        GetApplicationsAsync()
    {
        var applications =
            await _applicationRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ApplicationResponseDto>>(
            applications);
    }

    public async Task UpdateApplicationStatusAsync(
        Guid id,
        UpdateApplicationStatusDto dto)
    {
        var application =
            await _applicationRepository.GetByIdAsync(id);

        if (application == null)
        {
            throw new NotFoundException(
                $"Application with ID {id} not found.");
        }

        var status = dto.Status.Trim();

        if (status != ApplicationStatuses.Pending &&
            status != ApplicationStatuses.Accepted &&
            status != ApplicationStatuses.Rejected)
        {
            throw new BadRequestException(
                "Invalid application status. " +
                "Allowed values are Pending, Accepted, or Rejected.");
        }

        application.Status = status;

        await _applicationRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Admin changed application {ApplicationId} status to {Status}",
            application.Id,
            status);
    }

    // =========================================================
    // DASHBOARD
    // =========================================================

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var totalUsers =
            await _userRepository.GetUserCountAsync();

        var totalEmployers =
            await _userRepository.GetUserCountByRoleAsync(
                Roles.Employer);

        var totalCandidates =
            await _userRepository.GetUserCountByRoleAsync(
                Roles.Candidate);

        var totalCompanies =
            await _companyRepository.GetCompanyCountAsync();

        var totalJobs =
            await _jobRepository.GetJobCountAsync();

        var totalApplications =
            await _applicationRepository.GetApplicationCountAsync();

        return new DashboardDto
        {
            TotalUsers = totalUsers,
            TotalEmployers = totalEmployers,
            TotalCandidates = totalCandidates,
            TotalCompanies = totalCompanies,
            TotalJobs = totalJobs,
            TotalApplications = totalApplications
        };
    }
}