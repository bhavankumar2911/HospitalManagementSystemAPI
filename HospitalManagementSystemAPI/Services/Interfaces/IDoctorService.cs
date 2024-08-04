using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task AddNewDoctor(Doctor doctor);
        public Task<IEnumerable<DoctorResponseDTO>> ViewAllDoctors();
        public Task<DoctorAppointmentSlotsDTO> GetDoctorAppointmentSlots(int doctorId);
        public Task<IEnumerable<DoctorResponseDTO>> GetDoctorsWithLeastAppointments();
    }
}
