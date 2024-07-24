namespace HospitalManagementSystemAPI.Repositories.Interfaces
{
    public interface IRepository<Entity> where Entity : class
    {
        Task<IEnumerable<Entity>> GetAll();
        Task<Entity> Get(int id);
        Task<Entity> Create(Entity entity);
        Task<bool> Delete(int id);
        Task<Entity> Update(Entity updatedEntity, int id);
    }
}
