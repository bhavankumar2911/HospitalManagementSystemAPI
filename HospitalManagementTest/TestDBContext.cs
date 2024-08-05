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
            base.OnModelCreating(modelBuilder); 
        }
    }
}
