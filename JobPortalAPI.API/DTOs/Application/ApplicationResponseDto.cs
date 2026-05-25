namespace JobPortalAPI.API.DTOs.Application;

public class ApplicationResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
    public string JobTitle { get; set; } = string.Empty;
}
