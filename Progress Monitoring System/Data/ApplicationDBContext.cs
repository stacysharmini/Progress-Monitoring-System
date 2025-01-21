using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Models;
namespace Progress_Monitoring_System.Data

{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserRoleModel> Role { get; set; }
        public DbSet<UATFATModel> UATFAT { get; set; }
        public DbSet<UploadVersionModel> UploadVersion { get; set; }

    }
}
