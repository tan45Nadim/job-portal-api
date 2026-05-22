using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.API.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public endpoint");
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        return Ok("Protected endpoint");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("Admin only endpoint");
    }

    [Authorize(Roles = "Employer")]
    [HttpGet("employer")]
    public IActionResult EmployerOnly()
    {
        return Ok("Employer only endpoint");
    }

    [Authorize(Roles = "Candidate")]
    [HttpGet("candidate")]
    public IActionResult CandidateOnly()
    {
        return Ok("Candidate only endpoint");
    }
}