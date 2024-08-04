using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Appointment
{
    public class NewAppointmentDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Give a valid doctor.")]
        public int DoctorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Give a valid patient.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Slot is required.")]
        public DateTime FixedDateTime { get; set; }

        [Required(ErrorMessage = "Concern is required.")]
        public string Concern { get; set; } = string.Empty;
    }
}
