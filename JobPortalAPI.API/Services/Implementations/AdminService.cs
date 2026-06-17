using AutoMapper;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Admin;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;

namespace JobPortalAPI.API.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;

    public AdminService(
        IUserRepository userRepository,
        ICompanyRepository companyRepository,
        IJobRepository jobRepository,
        IApplicationRepository applicationRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _jobRepository = jobRepository;
        _applicationRepository = applicationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserSummaryDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
    }

    public async Task<UserSummaryDto> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException($"User with ID {id} not found.");

        return _mapper.Map<UserSummaryDto>(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException($"User with ID {id} not found.");

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException($"Company with ID {id} not found.");

        await _companyRepository.DeleteAsync(company);
        await _companyRepository.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(Guid id)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            throw new NotFoundException($"Job with ID {id} not found.");

        await _jobRepository.DeleteAsync(job);
        await _jobRepository.SaveChangesAsync();
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var userCount = await _userRepository.GetUserCountAsync();
        var employerCount = await _userRepository.GetUserCountByRoleAsync(Roles.Employer);
        var candidateCount = await _userRepository.GetUserCountByRoleAsync(Roles.Candidate);
        var companyCount = await _companyRepository.GetCompanyCountAsync();
        var jobCount = await _jobRepository.GetJobCountAsync();
        var applicationCount = await _applicationRepository.GetApplicationCountAsync();

        return new DashboardDto
        {
            TotalUsers = userCount,
            TotalEmployers = employerCount,
            TotalCandidates = candidateCount,
            TotalCompanies = companyCount,
            TotalJobs = jobCount,
            TotalApplications = applicationCount
        };
    }
}
