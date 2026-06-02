using System.Security.Claims;
using JobPortalAPI.API.DTOs.Application;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.API.Controllers;

[ApiController]
[Route("api/applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [Authorize(Roles = "Candidate")]
    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] ApplicationCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _applicationService.ApplyAsync(userId, dto);

        return Ok(result);
    }

    [Authorize(Roles = "Candidate")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyApplications()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _applicationService.GetMyApplicationsAsync(userId);

        return Ok(result);
    }

    [Authorize(Roles = "Candidate")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Withdraw(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _applicationService.WithdrawAsync(id, userId);

        return Ok("Application withdrawn successfully");
    }

    [Authorize(Roles = "Employer")]
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetApplicants(Guid jobId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _applicationService.GetApplicantsAsync(jobId, userId);

        return Ok(result);
    }
}