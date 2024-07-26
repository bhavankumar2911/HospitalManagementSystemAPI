namespace HospitalManagementSystemAPI.Exceptions.Generic
{
    public class EmptySearchStringException : Exception
    {
        public EmptySearchStringException(string placeholder) : base($"Enter a {placeholder} to search.")
        {
        }
    }
}
