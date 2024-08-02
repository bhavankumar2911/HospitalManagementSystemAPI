using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.DTOs.Authentication
{
    public class AuthorizeResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
