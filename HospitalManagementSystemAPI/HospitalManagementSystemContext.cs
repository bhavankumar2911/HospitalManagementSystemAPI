using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI
{
    public class HospitalManagementSystemContext : DbContext
    {
        public HospitalManagementSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
