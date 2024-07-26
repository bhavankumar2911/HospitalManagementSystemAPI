namespace HospitalManagementSystemAPI.Exceptions.Staff
{
    public class StaffPhoneDuplicationException : Exception
    {
        public StaffPhoneDuplicationException() : base("The phone number is used by another staff.")
        {
        }
    }
}
