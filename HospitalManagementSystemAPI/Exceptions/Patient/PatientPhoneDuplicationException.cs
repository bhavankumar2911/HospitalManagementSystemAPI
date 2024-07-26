namespace HospitalManagementSystemAPI.Exceptions.Patient
{
    public class PatientPhoneDuplicationException : Exception
    {
        public PatientPhoneDuplicationException() : base("This phone number is given by another patient.")
        {
        }
    }
}
