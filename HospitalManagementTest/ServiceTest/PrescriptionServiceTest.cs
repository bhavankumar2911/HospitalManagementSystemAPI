using HospitalManagementSystemAPI.DTOs.Prescription;
using HospitalManagementSystemAPI.DTOs.PrescriptionItem;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementTest.ServiceTest
{
    internal class PrescriptionServiceTest
    {
        private TestDBContext _dbContext;

        private IRepository<Prescription> _prescriptionRepository;
        private IRepository<PrescriptionItem> _prescriptionItemRepository;
        private IRepository<Medicine> _medicineRepository;
        private IRepository<Doctor> _doctorRepository;
        private IRepository<Patient> _patientRepository;
        private IRepository<User> _userRepository;


        private IPrescriptionService _prescriptionService;

        [SetUp]
        public void Setup ()
        {
            _dbContext = new TestDBContext(TestDBContext.GetDBContextOptions());
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _prescriptionRepository = new PrescriptionRepository(_dbContext);
            _prescriptionItemRepository = new PrescriptionItemRepository(_dbContext);
            _medicineRepository = new MedicineRepository(_dbContext);
            _doctorRepository = new DoctorRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);
            _patientRepository = new PatientRepository(_dbContext);

            _prescriptionService = new PrescriptionService(
                    _medicineRepository,
                    _prescriptionRepository,
                    _doctorRepository,
                    _patientRepository,
                    _prescriptionItemRepository,
                    _userRepository
                );
        }

        [Test]
        public async Task SaveNewPrescription ()
        {
            NewPrescriptionDTO newPrescriptionDTO = new NewPrescriptionDTO
            {
                PatientId = 1,
                PrescriptionItems = new List<PrescriptionItemDTO>()
                {
                    new PrescriptionItemDTO
                    {
                        MedicineId = 1,
                        DosageInMG = 10,
                        ConsumingInterval = HospitalManagementSystemAPI.Enums.ConsumingInterval.Morning,
                        NoOfDays = 3,
                    },
                    new PrescriptionItemDTO
                    {
                        MedicineId = 1,
                        DosageInMG = 10,
                        ConsumingInterval = HospitalManagementSystemAPI.Enums.ConsumingInterval.Night,
                        NoOfDays = 3,
                    }
                }
            };

            var prescription = await _prescriptionService.SaveNewPrescription(newPrescriptionDTO, 1);

            Assert.Multiple(() =>
            {
                Assert.That(_dbContext.Prescriptions.Count, Is.EqualTo(1));
                Assert.That(_dbContext.PrescriptionItems.Count, Is.EqualTo(2));
            });
        }
    }
}
