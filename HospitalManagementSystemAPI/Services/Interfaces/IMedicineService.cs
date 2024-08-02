using HospitalManagementSystemAPI.DTOs.Medicine;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IMedicineService
    {
        public Task<Medicine> AddNewMedicine(NewMedicineDTO newMedicineDTO);
        public Task<IEnumerable<Medicine>> GetAllMedicines();
    }
}
