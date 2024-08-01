namespace HospitalManagementSystemAPI.Exceptions.Authentication
{
    public class InvalidLoginCredentialsException : Exception
    {
        public InvalidLoginCredentialsException() : base("Invalid login credentials.")
        {
        }
    }
}
