namespace JobPortalAPI.API.DTOs.Application;

public class ApplicationCreateDto
{
    public Guid JobId { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;

}