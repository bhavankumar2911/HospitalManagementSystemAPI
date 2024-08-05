using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementTest
{
    internal class TestDBContext : HospitalManagementSystemAPI.HospitalManagementSystemContext
    {
        public TestDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        static public DbContextOptions GetDBContextOptions()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dbTest");

            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Staff staff = new Staff
            {
                Id = 1,
                Firstname = "staff",
                Lastname = "one",
                DateOfJoining = DateTime.Today,
                Email = "staff@gmail.com",
                Address = "India",
                Phone = "+91 8989898989",
                DateOfBirth = DateTime.Today,
            };

            modelBuilder.Entity<Medicine>()
                .HasData(
                    new Medicine
                    {
                        Id = 1,
                        Name = "Paracetamol",
                        QuantityInMG = 100,
                        Price = 150,
                        Units = 250
                    }
                );

            modelBuilder.Entity<Staff>().HasData(staff);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Email = staff.Email,
                    Role = HospitalManagementSystemAPI.Enums.Role.Doctor,
                    Staff = staff,
                    PasswordHashKey = new byte[] { },
                    HashedPassword = new byte[] { },
                });

            modelBuilder.Entity<Doctor>()
                .HasData(new Doctor
                {
                    Id = 1,
                    Qualification = "mbbs",
                    Specialization = "dentist",
                    Staff = staff,
                });

            modelBuilder.Entity<Patient>()
                .HasData(new Patient
                {
                    Id = 1,
                    Gender = HospitalManagementSystemAPI.Enums.Gender.Male,
                    Blood = HospitalManagementSystemAPI.Enums.Blood.OPositive,
                    Firstname = "Patient",
                    Email = "patient@gmail.com",
                    Phone = "+1 2345678901",
                    Address = "India",
                    DateOfBirth = DateTime.Today,
                });

            base.OnModelCreating(modelBuilder); 
        }
    }
}
