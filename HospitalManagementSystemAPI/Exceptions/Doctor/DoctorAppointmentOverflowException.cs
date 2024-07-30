namespace HospitalManagementSystemAPI.Exceptions.Doctor
{
    public class DoctorAppointmentOverflowException : Exception
    {
        public DoctorAppointmentOverflowException() : base("The doctor has maximum appointments.")
        {
        }
    }
}
