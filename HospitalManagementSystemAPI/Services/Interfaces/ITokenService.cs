namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(int userId, string role);
    }
}
