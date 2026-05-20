namespace JobPortalAPI.API.Models;

public class Job
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
    public string Location { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to Company
    public Guid CompanyId { get; set; }

    // Navigation property
    public Company Company { get; set; } = null!;
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
