namespace HospitalManagementSystemAPI.Exceptions.Authentication
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("You are not authorized.")
        {
        }
    }
}
