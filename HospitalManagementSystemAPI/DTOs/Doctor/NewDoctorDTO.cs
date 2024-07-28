using HospitalManagementSystemAPI.DTOs.Staff;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Doctor
{
    public class NewDoctorDTO : NewStaffDTO
    {
        [Required(ErrorMessage = "Qualification is required for a doctor.")]
        public string Qualification { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required for a doctor.")]
        public string Specialization { get; set; } = string.Empty;
    }
}
