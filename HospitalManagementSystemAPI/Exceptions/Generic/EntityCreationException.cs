namespace HospitalManagementSystemAPI.Exceptions.Generic
{
    public class EntityCreationException : Exception
    {
        public EntityCreationException(string entityName) : base($"Cannot create {entityName}") { }
    }
}
