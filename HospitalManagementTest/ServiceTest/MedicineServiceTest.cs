using HospitalManagementSystemAPI;
using HospitalManagementSystemAPI.DTOs.Medicine;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementTest.ServiceTest
{
    internal class MedicineServiceTest
    {
        private TestDBContext _context;

        private IRepository<Medicine> _medicineRepository;

        private IMedicineService _medicineService;

        [SetUp]
        public void Setup ()
        {
            // context
            _context = new TestDBContext(TestDBContext.GetDBContextOptions());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // repos
            _medicineRepository = new MedicineRepository(_context);

            //service
            _medicineService = new MedicineService(_medicineRepository);
        }

        [Test]
        public async Task GetAllMedicines ()
        {
            var medicines = await _medicineService.GetAllMedicines();

            Assert.That(medicines.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task AddNewMedicine()
        {
            NewMedicineDTO newMedicineDTO = new NewMedicineDTO
            {
                Name = "Amoxicillin",
                QuantityInMG = 200,
                Price = 150,
                Units = 250
            };

            var medicine = await _medicineService.AddNewMedicine(newMedicineDTO);

            Assert.That(_context.Medicines.Count, Is.EqualTo(2));
        }
    }
}
