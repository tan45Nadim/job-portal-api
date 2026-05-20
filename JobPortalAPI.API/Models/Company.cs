using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalAPI.API.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to User (Employer)
        public Guid OwnerId { get; set; }

        // Navigation property
        public User Owner { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}