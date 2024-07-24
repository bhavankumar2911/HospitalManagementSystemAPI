namespace HospitalManagementSystemAPI.Exceptions.Role
{
    public class RoleDuplicationException : Exception
    {
        public RoleDuplicationException(string roleName) : base($"{roleName} role already exists.") { }
    }
}
