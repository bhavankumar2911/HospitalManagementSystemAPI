namespace HospitalManagementSystemAPI.Exceptions.Generic
{
    public class NoEntitiesAvailableException : Exception
    {
        public NoEntitiesAvailableException(string entityName) : base($"No {entityName}s were found.")
        {
        }
    }
}
