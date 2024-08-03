using HospitalManagementSystemAPI.DTOs.Medicine;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Medicine;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IRepository<Medicine> _medicineRepository;

        public MedicineService(IRepository<Medicine> medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        private async Task<Medicine> SaveMedicineToDB(NewMedicineDTO newMedicineDTO)
        {
            var medicine = await _medicineRepository.Create(new Medicine
            {
                Name = newMedicineDTO.Name,
                QuantityInMG = newMedicineDTO.QuantityInMG,
                Price = newMedicineDTO.Price,
                Units = newMedicineDTO.Units,
            });

            return medicine;
        }

        public async Task<Medicine> AddNewMedicine(NewMedicineDTO newMedicineDTO)
        {
            try
            {
                var medicines = await _medicineRepository.GetAll();

                // check duplication
                var duplicateMedicine = medicines
                    .Where(m => m.Name.ToLower() == newMedicineDTO.Name.ToLower()
                        && m.QuantityInMG == newMedicineDTO.QuantityInMG
                    );

                if (duplicateMedicine.Any()) throw new MedicineDuplicationException();

                return await SaveMedicineToDB(newMedicineDTO);
            }
            catch (NoEntitiesAvailableException)
            {
                return await SaveMedicineToDB(newMedicineDTO);
            }
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicines()
        {
            var medicines = (await _medicineRepository.GetAll())
                .Where(m => m.Units > 0);

            return medicines;
        }
    }
}
