using AutoMapper;
using JobPortalAPI.API.DTOs.Company;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;

namespace JobPortalAPI.API.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompanyService> logger)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CompanyResponseDto> CreateAsync(CompanyCreateDto dto, Guid ownerId)
    {
        _logger.LogInformation(
            "Create company request received for {CompanyName} by owner {OwnerId}",
            dto.Name, ownerId);

        var company = _mapper.Map<Company>(dto);

        company.OwnerId = ownerId;

        await _companyRepository.AddAsync(company);
        await _companyRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Company created successfully: {CompanyName} with ID {CompanyId} by owner {OwnerId}",
            company.Name, company.Id, ownerId);

        return _mapper.Map<CompanyResponseDto>(company);
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);
    }

    public async Task<CompanyResponseDto?> GetByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException("Company not found");

        return _mapper.Map<CompanyResponseDto>(company);
    }

    public async Task UpdateAsync(Guid id, CompanyUpdateDto dto, Guid ownerId)
    {
        _logger.LogInformation(
            "Update company request received for ID {CompanyId} by owner {OwnerId}",
            id, ownerId);

        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException("Company not found");

        // OWNERSHIP CHECK
        if (company.OwnerId != ownerId)
        {
            _logger.LogWarning(
                "Employer {EmployerId} attempted to update Company {CompanyId} without ownership.",
                ownerId, id);

            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");
        }

        // map updated fields from dto to company
        _mapper.Map(dto, company);

        await _companyRepository.UpdateAsync(company);
        await _companyRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Company updated successfully: {CompanyName} with ID {CompanyId} by owner {OwnerId}",
            company.Name, company.Id, ownerId);
    }

    public async Task DeleteAsync(Guid id, Guid ownerId)
    {
        _logger.LogInformation(
            "Company delete request received for ID {CompanyId} by owner {OwnerId}",
            id, ownerId);

        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException("Company not found");

        // OWNERSHIP CHECK
        if (company.OwnerId != ownerId)
        {
            _logger.LogWarning(
                "Employer {EmployerId} attempted to delete Company {CompanyId} without ownership.",
                ownerId, id);

            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");
        }


        _logger.LogInformation(
            "Company deleted successfully: {CompanyName} with ID {CompanyId} by owner {OwnerId}",
            company.Name, company.Id, ownerId);

        await _companyRepository.DeleteAsync(company);
        await _companyRepository.SaveChangesAsync();

    }
}
