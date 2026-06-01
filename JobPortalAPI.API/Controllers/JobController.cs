using System.Security.Claims;
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

    [Authorize(Roles = "Employer")]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _jobService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _jobService.SearchAsync(keyword);

        return Ok(result);
    }

    [Authorize(Roles = "Employer")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] JobUpdateDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _jobService.UpdateAsync(id, dto, userId);

        return Ok("Job updated successfully");
    }

    [Authorize(Roles = "Employer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _jobService.DeleteAsync(id, userId);

        return Ok("Job deleted successfully");
    }

}
