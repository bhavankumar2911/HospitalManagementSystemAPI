namespace HospitalManagementSystemAPI.Exceptions.Patient
{
    public class PatientEmailDuplicationException : Exception
    {
        public PatientEmailDuplicationException() : base("The email address is used given by another patient.")
        {
        }
    }
}
