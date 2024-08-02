namespace HospitalManagementSystemAPI.Exceptions.Medicine
{
    public class MedicineDuplicationException : Exception
    {
        public MedicineDuplicationException() : base("The medicine is already available.")
        {
        }
    }
}
