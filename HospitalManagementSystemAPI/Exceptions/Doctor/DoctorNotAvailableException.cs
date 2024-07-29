namespace HospitalManagementSystemAPI.Exceptions.Doctor
{
    public class DoctorNotAvailableException : Exception
    {
        public DoctorNotAvailableException() : base("The doctor is not available at this time.") { }
    }
}
