using HospitalManagementSystemAPI.DTOs.Patient;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<IEnumerable<Patient>> SearchPatientByName(string searchString);
        public Task<Patient> AddNewPatient(NewPatientDTO newPatientDTO);
    }
}
