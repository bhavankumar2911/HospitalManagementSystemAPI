using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Authentication
{
    public class StaffLoginInputDTO
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string PlainTextPassword { get; set; } = string.Empty;
    }
}
