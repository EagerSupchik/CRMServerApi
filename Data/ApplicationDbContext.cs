using CRMServerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMServerApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<CrmCall> Calls { get; set; }
        public DbSet<CrmMeeting> Meetings { get; set; }
        public DbSet<CrmDeal> Deals { get; set; }
    }
}
