using System.Security.Claims;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // =========================================================
    // USERS
    // =========================================================

    // GET: api/admin/users
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();

        return Ok(users);
    }

    // GET: api/admin/users/{id}
    [HttpGet("users/{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _adminService.GetUserAsync(id);

        return Ok(user);
    }

    // PUT: api/admin/users/{id}/activate
    [HttpPut("users/{id:guid}/activate")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        await _adminService.ActivateUserAsync(id);

        return NoContent();
    }

    // PUT: api/admin/users/{id}/deactivate
    [HttpPut("users/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var currentAdminId = GetCurrentUserId();

        await _adminService.DeactivateUserAsync(
            id,
            currentAdminId);

        return NoContent();
    }

    // DELETE: api/admin/users/{id}
    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var currentAdminId = GetCurrentUserId();

        await _adminService.DeleteUserAsync(
            id,
            currentAdminId);

        return NoContent();
    }

    // =========================================================
    // COMPANIES
    // =========================================================

    // GET: api/admin/companies
    [HttpGet("companies")]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _adminService.GetCompaniesAsync();

        return Ok(companies);
    }

    // DELETE: api/admin/companies/{id}
    [HttpDelete("companies/{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _adminService.DeleteCompanyAsync(id);

        return NoContent();
    }

    // =========================================================
    // JOBS
    // =========================================================

    // GET: api/admin/jobs
    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _adminService.GetJobsAsync();

        return Ok(jobs);
    }

    // DELETE: api/admin/jobs/{id}
    [HttpDelete("jobs/{id:guid}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        await _adminService.DeleteJobAsync(id);

        return NoContent();
    }

    // =========================================================
    // APPLICATIONS
    // =========================================================

    // GET: api/admin/applications
    [HttpGet("applications")]
    public async Task<IActionResult> GetApplications()
    {
        var applications =
            await _adminService.GetApplicationsAsync();

        return Ok(applications);
    }

    // PUT: api/admin/applications/{id}/status
    [HttpPut("applications/{id:guid}/status")]
    public async Task<IActionResult> UpdateApplicationStatus(
        Guid id,
        UpdateApplicationStatusDto dto)
    {
        await _adminService.UpdateApplicationStatusAsync(
            id,
            dto);

        return NoContent();
    }

    // =========================================================
    // DASHBOARD
    // =========================================================

    // GET: api/admin/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var dashboard =
            await _adminService.GetDashboardAsync();

        return Ok(dashboard);
    }

    // =========================================================
    // PRIVATE HELPER
    // =========================================================

    private Guid GetCurrentUserId()
    {
        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
        {
            throw new UnauthorizedAccessException(
                "Unable to identify the current user.");
        }

        return id;
    }
}