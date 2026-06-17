using JobPortalAPI.API.Constants;
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

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _adminService.GetUserAsync(id);
        return Ok(user);
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _adminService.DeleteUserAsync(id);
        return Ok();
    }

    [HttpDelete("companies/{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _adminService.DeleteCompanyAsync(id);
        return Ok();
    }

    [HttpDelete("jobs/{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        await _adminService.DeleteJobAsync(id);
        return Ok();
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var stats = await _adminService.GetDashboardAsync();
        return Ok(stats);
    }

}
