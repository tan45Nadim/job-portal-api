using System.Security.Claims;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Job;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobCreateDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _jobService.CreateAsync(dto, userId);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _jobService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{jobId}")]
    public async Task<IActionResult> GetById(Guid jobId)
    {
        var result = await _jobService.GetByIdAsync(jobId);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _jobService.SearchAsync(keyword);

        return Ok(result);
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpPut("{jobId}")]
    public async Task<IActionResult> Update(Guid jobId, [FromBody] JobUpdateDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _jobService.UpdateAsync(jobId, dto, userId);

        return Ok("Job updated successfully");
    }

    [Authorize(Roles = Roles.Employer)]
    [HttpDelete("{jobId}")]
    public async Task<IActionResult> Delete(Guid jobId)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _jobService.DeleteAsync(jobId, userId);

        return Ok("Job deleted successfully");
    }

}
