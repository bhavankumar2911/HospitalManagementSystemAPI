namespace HospitalManagementSystemAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; } = new Role();
        public Staff Staff { get; set; } = new Staff();
        public byte[] PasswordHashKey { get; set; } = new byte[0];
        public byte[] HashedPassword { get; set; } = new byte[0];
    }
}
