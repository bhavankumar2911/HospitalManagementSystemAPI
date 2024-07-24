namespace HospitalManagementSystemAPI.Exceptions.Generic
{
    public class EntityDeletionException : Exception
    {
        public EntityDeletionException(string entityName, int id) : base($"Cannot delete {entityName} with id {id}")
        {
        }
    }
}
