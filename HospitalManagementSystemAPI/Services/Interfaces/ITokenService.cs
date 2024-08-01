using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(int userId, Role role);
    }
}
