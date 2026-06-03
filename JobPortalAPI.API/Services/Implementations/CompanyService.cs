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

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<CompanyResponseDto> CreateAsync(CompanyCreateDto dto, Guid ownerId)
    {
        var company = _mapper.Map<Company>(dto);

        company.OwnerId = ownerId;

        await _companyRepository.AddAsync(company);
        await _companyRepository.SaveChangesAsync();

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

    public async Task UpdateAsync(Guid id, UpdateCompanyDto dto, Guid ownerId)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException("Company not found");

        // OWNERSHIP CHECK
        if (company.OwnerId != ownerId)
            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");

        // map updated fields from dto to company
        _mapper.Map(dto, company);

        await _companyRepository.UpdateAsync(company);
        await _companyRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, Guid ownerId)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new NotFoundException("Company not found");

        // OWNERSHIP CHECK
        if (company.OwnerId != ownerId)
            throw new ForbiddenException("Unauthorized! You are not the owner of this company.");

        await _companyRepository.DeleteAsync(company);
        await _companyRepository.SaveChangesAsync();
    }


}
