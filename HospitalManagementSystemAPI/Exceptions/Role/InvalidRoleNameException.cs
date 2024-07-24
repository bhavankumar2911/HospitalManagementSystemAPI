namespace HospitalManagementSystemAPI.Exceptions.Role
{
    public class InvalidRoleNameException : Exception
    {
        public InvalidRoleNameException() : base("Role name cannot be empty.")
        {
        }
    }
}
