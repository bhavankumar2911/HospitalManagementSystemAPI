namespace HospitalManagementSystemAPI.Objects.Authentication
{
    public class PasswordInformation
    {
        public byte[] PasswordHashKey { get; set; } = new byte[0];
        public byte[] HashedPassword { get; set; } = new byte[0];
    }
}
