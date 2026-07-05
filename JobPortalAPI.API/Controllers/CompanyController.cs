using System.Security.Claims;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Company;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.API.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanyCreateDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _companyService.CreateAsync(dto, userId);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _companyService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetById(Guid companyId)
    {
        var result = await _companyService.GetByIdAsync(companyId);

        return Ok(result);
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpPut("{companyId}")]
    public async Task<IActionResult> Update(Guid companyId, [FromBody] CompanyUpdateDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _companyService.UpdateAsync(companyId, dto, userId);

        return Ok("Company updated successfully");
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpDelete("{companyId}")]
    public async Task<IActionResult> Delete(Guid companyId)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _companyService.DeleteAsync(companyId, userId);

        return Ok("Company deleted successfully");
    }

}
