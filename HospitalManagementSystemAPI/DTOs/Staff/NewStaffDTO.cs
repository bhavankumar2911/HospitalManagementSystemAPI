using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Staff
{
    public class NewStaffDTO
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$", ErrorMessage = "Phone number is not valid.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [Range(0, 4, ErrorMessage = "Give a valid role.")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string PlainTextPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Firstname is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Firstname must have only alphabets and spaces.")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Lastname must have only alphabets and spaces.")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of joining is required.")]
        public DateTime DateOfJoining { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Address of the staff is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
