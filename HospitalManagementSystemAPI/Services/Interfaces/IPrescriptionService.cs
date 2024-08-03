using HospitalManagementSystemAPI.DTOs.Prescription;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IPrescriptionService
    {
        public Task<Prescription> SaveNewPrescription(NewPrescriptionDTO newPrescriptionDTO, int userId);

        public Task<IDictionary<int, IList<PrescriptionItem>>> GetPatientPrescriptions(PatientPrescriptionsInputDTO patientPrescriptionsInputDTO);
    }
}
