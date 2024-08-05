using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
