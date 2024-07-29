using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI
{
    public class HospitalManagementSystemContext : DbContext
    {
        public HospitalManagementSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
