namespace HospitalManagementSystemAPI.Exceptions.Patient
{
    public class PatientAppointmentConflictException : Exception
    {
        public PatientAppointmentConflictException(string message) : base (message) { }
    }
}
