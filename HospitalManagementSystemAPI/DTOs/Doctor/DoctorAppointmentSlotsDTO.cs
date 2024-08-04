using HospitalManagementSystemAPI.DTOs.Appointment;

namespace HospitalManagementSystemAPI.DTOs.Doctor
{
    public class DoctorAppointmentSlotsDTO
    {
        public IEnumerable<SlotDTO> AppointmentSlots { get; set; } = null!;
    }
}
