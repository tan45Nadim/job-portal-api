using AutoMapper;
using JobPortalAPI.API.DTOs.Job;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;

namespace JobPortalAPI.API.Services.Implementations;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public JobService(IJobRepository jobRepository, ICompanyRepository companyRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<JobResponseDto> CreateAsync(JobCreateDto dto, Guid ownerId)
    {
        // Check if the company exists and belongs to the owner
        var company = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (company == null)
            throw new NotFoundException("Company not found");

        if (company.OwnerId != ownerId)
            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");

        // verify deadline is in future
        if (dto.Deadline <= DateTime.UtcNow)
            throw new BadRequestException(
                "Deadline must be a future date");

        // verify salary range is valid
        if (dto.SalaryMin > dto.SalaryMax)
            throw new BadRequestException(
                "SalaryMin cannot be greater than SalaryMax");

        // map dto to job entity
        var job = _mapper.Map<Job>(dto);

        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChangesAsync();

        return _mapper.Map<JobResponseDto>(job);
    }

    public async Task<IEnumerable<JobResponseDto>> GetAllAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<JobResponseDto>>(jobs);
    }

    public async Task<JobResponseDto?> GetByIdAsync(Guid id)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            throw new NotFoundException("Job not found");

        return _mapper.Map<JobResponseDto>(job);
    }

    public async Task<IEnumerable<JobResponseDto>> SearchAsync(string keyword)
    {
        var jobs = await _jobRepository.SearchAsync(keyword);

        return _mapper.Map<IEnumerable<JobResponseDto>>(jobs);
    }

    public async Task<JobResponseDto> UpdateAsync(Guid id, JobUpdateDto dto, Guid ownerId)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            throw new NotFoundException("Job not found");

        // Check if the company exists and belongs to the owner
        var company = await _companyRepository.GetByIdAsync(job.CompanyId);
        if (company == null)
            throw new NotFoundException("Company not found");

        if (company.OwnerId != ownerId)
            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");

        // verify deadline is in future
        if (dto.Deadline <= DateTime.UtcNow)
            throw new BadRequestException(
                "Deadline must be a future date");

        // verify salary range is valid
        if (dto.SalaryMin > dto.SalaryMax)
            throw new BadRequestException(
                "SalaryMin cannot be greater than SalaryMax");

        // map updated fields from dto to job entity
        _mapper.Map(dto, job);

        await _jobRepository.UpdateAsync(job);
        await _jobRepository.SaveChangesAsync();

        return _mapper.Map<JobResponseDto>(job);
    }

    public async Task DeleteAsync(Guid id, Guid ownerId)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            throw new NotFoundException("Job not found");

        // Check if the company exists and belongs to the owner
        var company = await _companyRepository.GetByIdAsync(job.CompanyId);
        if (company == null)
            throw new NotFoundException("Company not found");

        if (company.OwnerId != ownerId)
            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");

        await _jobRepository.DeleteAsync(job);
        await _jobRepository.SaveChangesAsync();
    }

}