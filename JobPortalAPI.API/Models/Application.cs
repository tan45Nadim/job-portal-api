namespace JobPortalAPI.API.Models;

public class Application
{
    public Guid Id { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public Guid JobId { get; set; }
    public Guid CandidateId { get; set; }

    // Navigation properties
    public Job Job { get; set; } = null!;
    public User Candidate { get; set; } = null!;
}
