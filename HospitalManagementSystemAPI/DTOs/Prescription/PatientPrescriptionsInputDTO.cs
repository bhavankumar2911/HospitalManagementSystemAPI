
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Prescription
{
    public class PatientPrescriptionsInputDTO
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
