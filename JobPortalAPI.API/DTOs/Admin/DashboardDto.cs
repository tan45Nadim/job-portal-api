namespace JobPortalAPI.API.DTOs.Admin;

public class DashboardDto
{
    public int TotalUsers { get; set; }
    public int TotalEmployers { get; set; }
    public int TotalCandidates { get; set; }
    public int TotalCompanies { get; set; }
    public int TotalJobs { get; set; }
    public int TotalApplications { get; set; }
}