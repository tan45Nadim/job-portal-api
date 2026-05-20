using JobPortalAPI.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.API.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  // Tables
  public DbSet<User> Users => Set<User>();
  public DbSet<Company> Companies => Set<Company>();
  public DbSet<Job> Jobs => Set<Job>();
  public DbSet<Application> Applications => Set<Application>();


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // USER - COMPANY (1 - M)
    modelBuilder.Entity<Company>()
        .HasOne(c => c.Owner)
        .WithMany(u => u.Companies)
        .HasForeignKey(c => c.OwnerId)
        .OnDelete(DeleteBehavior.Restrict);

    // COMPANY - JOB (1 - M)
    modelBuilder.Entity<Job>()
        .HasOne(j => j.Company)
        .WithMany(c => c.Jobs)
        .HasForeignKey(j => j.CompanyId)
        .OnDelete(DeleteBehavior.Cascade);

    // JOB - APPLICATION (1 - M)
    modelBuilder.Entity<Application>()
        .HasOne(a => a.Job)
        .WithMany(j => j.Applications)
        .HasForeignKey(a => a.JobId)
        .OnDelete(DeleteBehavior.Cascade);

    // USER - APPLICATION (1 - M)
    modelBuilder.Entity<Application>()
        .HasOne(a => a.Candidate)
        .WithMany(u => u.Applications)
        .HasForeignKey(a => a.CandidateId)
        .OnDelete(DeleteBehavior.Restrict);

    // UNIQUE EMAIL
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

  }

}
