namespace HospitalManagementSystemAPI.Exceptions.Staff
{
    public class StaffEmailDuplicationException : Exception
    {
        public StaffEmailDuplicationException() : base("The email address is used by another staff.")
        {
        }
    }
}
