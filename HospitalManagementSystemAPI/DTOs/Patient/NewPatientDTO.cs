using HospitalManagementSystemAPI.DTOs.MedicalHistory;
using HospitalManagementSystemAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Patient
{
    public class NewPatientDTO
    {
        [Required(ErrorMessage = "Gender is required")]
        [Range(0, 2, ErrorMessage = "Give a valid gender.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        [Range(0, 7, ErrorMessage = "Give a valid blood type.")]
        public Blood Blood { get; set; }

        [Required(ErrorMessage = "Firstname is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Firstname must have only alphabets and spaces.")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Lastname must have only alphabets and spaces.")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$", ErrorMessage = "Phone number is not valid.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public PatientMedicalHistoryDTO MedicalHistory { get; set; } = new PatientMedicalHistoryDTO();
    }
}
