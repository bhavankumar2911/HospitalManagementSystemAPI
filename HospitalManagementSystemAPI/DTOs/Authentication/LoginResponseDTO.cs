
using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.DTOs.Authentication
{
    public class LoginResponseDTO : Models.Staff
    {
        public string Token { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
