namespace HospitalManagementSystemAPI.Exceptions.Generic
{
    public class EntityUpdationException : Exception
    {
        public EntityUpdationException(string entityName, int id) : base($"Cannot update {entityName} with id {id}")
        {
        }
    }
}
